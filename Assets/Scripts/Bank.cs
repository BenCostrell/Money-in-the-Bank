using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour {

    public int coinsStored;
    public Player owner;
    public Color[] colors;
    public float growthScale;
    public float coinScatterDistance;

	// Use this for initialization
	void Start () {
		
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
        transform.localScale *= growthScale;
    }

    public void GetSmashed()
    {
        owner.GetBankBack(this);
        float angle;
        if (coinsStored > 0) {
            for (int i = 0; i < coinsStored; i++)
            {
                angle = (360f * i) / coinsStored;
                Vector2 pos = transform.position;
                Vector2 location = pos + (coinScatterDistance * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
                Services.CoinManager.CreateCoin(location);
            }
        }
        Destroy(gameObject);
    }
}
