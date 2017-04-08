using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int playerNum;
    public float accel;
    public float maxSpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Coin coinInHand;
    public float coinPositionOffset;
    public int totalBanks;
    public List<Bank> activeBanks;
    public Transform hammerPivot;
    public Collider2D hammerCollider;
    public bool actionable;
    public float hammerSwingTime;
    public float hammerRecoveryTime;
    public float hammerActiveFrameStart;

	void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hammerPivot = transform.GetChild(0);
        hammerCollider = hammerPivot.GetChild(0).gameObject.GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
	void Start () {
        actionable = true;
        activeBanks = new List<Bank>();
	}
	
	// Update is called once per frame
	void Update () {
        if (actionable)
        {
            Move();
            if (Input.GetButtonDown("CreateBank_P" + playerNum))
            {
                if (activeBanks.Count < totalBanks)
                {
                    CreateBank();
                }
            }
            if (Input.GetButtonDown("SwingHammer_P" + playerNum))
            {
                SwingHammer();
            }
        }
	}

    void Move()
    {
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal_P" + playerNum), Input.GetAxis("Vertical_P" + playerNum));
        if (inputVector.magnitude > 0.1f)
        {
            rb.AddForce(accel * inputVector);
            if (inputVector.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        //Debug.Log(rb.velocity.magnitude);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject obj = col.gameObject;
        if (obj.tag == "Coin")
        {
            Coin coin = obj.GetComponent<Coin>();
            if (coinInHand == null)
            {
                if (coin.owner == null)
                {
                    PickupCoin(coin);
                }
                else
                {
                    StealCoin(coin);
                }
            }
        }
        if (obj.tag == "Bank")
        {
            Bank bank = obj.GetComponent<Bank>();
            if (coinInHand != null && bank.owner == this)
            {
                DepositCoin(bank);
            }
        }
    }

    void PickupCoin(Coin coin)
    {
        coinInHand = coin;
        coin.transform.parent = transform;
        coin.transform.localPosition = coinPositionOffset * Vector2.down;
        coin.owner = this;
    }

    void StealCoin(Coin coin)
    {
        coin.owner.coinInHand = null;
        PickupCoin(coin);
    }

    void DepositCoin(Bank bank)
    {
        Services.CoinManager.DestroyCoin(coinInHand);
        coinInHand = null;
        bank.DepositCoin();
    }

    void CreateBank()
    {
        GameObject bankObj = Instantiate(Services.PrefabDB.Bank, transform.position, Quaternion.identity);
        Bank bank = bankObj.GetComponent<Bank>();
        bank.Init(this);
        activeBanks.Add(bank);
    }

    void SwingHammer()
    {
        rb.velocity = Vector2.zero;
        SwingHammer swingHammer = new SwingHammer(hammerSwingTime, hammerRecoveryTime, hammerActiveFrameStart, this);
        Services.TaskManager.AddTask(swingHammer);
    }

    public void GetBankBack(Bank bank)
    {
        activeBanks.Remove(bank);
    }

    public void GetKnockedBack(Vector2 kbVector)
    {
        rb.velocity = kbVector;
    }
}
