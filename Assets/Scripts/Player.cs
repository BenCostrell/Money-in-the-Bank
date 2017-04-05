using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int playerNum;
    public float accel;
    public float maxSpeed;
    private Rigidbody2D rb;
    private Coin coinInHand;
    public float coinPositionOffset;
    private int banksOnHand;
    public int totalBanks;
    private Transform hammerPivot;
    private Collider2D hammerCollider;

	void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hammerPivot = transform.GetChild(0);
        hammerCollider = hammerPivot.GetChild(0).gameObject.GetComponent<Collider2D>();
    }

    // Use this for initialization
	void Start () {
        banksOnHand = totalBanks;
	}
	
	// Update is called once per frame
	void Update () {
        Move();

        if (Input.GetButtonDown("CreateBank_P" + playerNum))
        {
            if (banksOnHand > 0)
            {
                CreateBank();
            }
        }
	}

    void Move()
    {
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal_P" + playerNum), Input.GetAxis("Vertical_P" + playerNum));
        if (inputVector.magnitude > 0.1f)
        {
            rb.AddForce(accel * inputVector);
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
        coin.transform.localPosition = coinPositionOffset * Vector2.right;
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
        banksOnHand -= 1;
        GameObject bankObj = Instantiate(Services.PrefabDB.Bank, transform.position, Quaternion.identity);
        Bank bank = bankObj.GetComponent<Bank>();
        bank.Init(this);
    }

    void Smash()
    {
        rb.velocity = Vector2.zero;
    }

    public void GetBankBack()
    {
        banksOnHand += 1;
    }
}
