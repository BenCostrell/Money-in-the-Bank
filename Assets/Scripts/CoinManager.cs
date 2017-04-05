using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour {

    private List<Coin> coins;
    public int numCoins;
    public float minAcceptableDistance;
    public float numTries;
    public Vector2 topRightCorner;
    public Vector2 bottomLeftCorner;

	// Use this for initialization
	void Start () {
        GenerateInitialCoinSetup();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateInitialCoinSetup()
    {
        coins = new List<Coin>();
        for (int i = 0; i < numCoins; i++)
        {
            Vector2 location = GenerateValidLocation();
            if (location != Vector2.zero)
            {
                CreateCoin(location);
            }
            else
            {
                Debug.Log("only made " + i + " coins");
                break;
            }
        }
    }

    public void CreateCoin(Vector2 location)
    {
        GameObject coinObj = Instantiate(Services.PrefabDB.Coin, location, Quaternion.identity) as GameObject;
        Coin coin = coinObj.GetComponent<Coin>();
        coins.Add(coin);
    }

    public void DestroyCoin(Coin coin)
    {
        coins.Remove(coin);
        Destroy(coin.gameObject);
    }

    Vector2 GenerateValidLocation()
    {
        Vector2 location = GenerateRandomLocation();
        bool isValid = IsLocationValid(location);
        for (int i = 0; i < numTries; i++)
        {
            if (!isValid)
            {
                location = GenerateRandomLocation();
                isValid = IsLocationValid(location);
            }
            else
            {
                return location;
            }
        }

        return Vector2.zero;
    }

    Vector2 GenerateRandomLocation()
    {
        float xLocation = Random.Range(bottomLeftCorner[0], topRightCorner[0]);
        float yLocation = Random.Range(bottomLeftCorner[1], topRightCorner[1]);
        return new Vector2(xLocation, yLocation);
    }

    bool IsLocationValid(Vector2 location)
    {
        bool valid = true;
        if (coins.Count > 0)
        {
            foreach(Coin coin in coins)
            {
                if (Vector2.Distance(location, coin.transform.position) < minAcceptableDistance)
                {
                    valid = false;
                    break;
                }
            }
        }

        return valid;
    }
}
