using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour {

    public int coinsStored;
    public Player owner;
    public Color[] colors;
    public float growthScale;
    public float coinScatterDistance;
    private TextMesh coinCountText;

	// Use this for initialization
	void Start () {
        coinCountText = GetComponentInChildren<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(Player player)
    {
        owner = player;
        Color color = colors[owner.playerNum - 1];
        GetComponent<SpriteRenderer>().color = color;
    }

    public void DepositCoin()
    {
        coinsStored += 1;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        coinCountText.text = coinsStored.ToString();
        transform.localScale = 2 * Mathf.Pow(1.1f, coinsStored) * Vector3.one;
    }

    public void GetSmashed()
    {
        if (coinsStored <= 2)
        {
            DestroyThis();
        }
        else {
            int coinsToSpill = Mathf.CeilToInt(coinsStored / 2f);
            SpillCoins(coinsToSpill);
        }
    }

    void SpillCoins (int numCoins)
    {
        float angle;
        if (numCoins > 0)
        {
            for (int i = 0; i < numCoins; i++)
            {
                angle = Random.Range(0, 360);
                Vector2 pos = transform.position;
                Vector2 location = pos + (coinScatterDistance * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
                Services.CoinManager.CreateCoin(location);
            }
        }
        owner.score -= numCoins;
        coinsStored -= numCoins;
        UpdateVisuals();
        
    }

    void DestroyThis()
    {
        SpillCoins(coinsStored);
        owner.GetBankBack(this);
        Destroy(gameObject);
    }


}
