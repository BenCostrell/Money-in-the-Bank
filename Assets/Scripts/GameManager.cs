using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Player[] players;
    public Vector2[] playerSpawns;
    public Color[] playerColors;

	void Awake()
    {
        InitializeServices();
    }
    
    // Use this for initialization
	void Start () {
        InitializePlayers();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset"))
        {
            Reset();
        }
	}

    void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void InitializeServices()
    {
        Services.EventManager = new EventManager();
        Services.TaskManager = new TaskManager();
        Services.PrefabDB = Resources.Load<PrefabDB>("Prefabs/PrefabDB");
        Services.GameManager = this;
        Services.CoinManager = GameObject.FindGameObjectWithTag("CoinManager").GetComponent<CoinManager>();
    }

    void InitializePlayers()
    {
        players = new Player[2]
        {
            InitializePlayer(1),
            InitializePlayer(2)
        };
    }

    Player InitializePlayer(int playerNum)
    {
        GameObject playerObj = Instantiate(Services.PrefabDB.Player, playerSpawns[playerNum - 1], Quaternion.identity) as GameObject;
        Player player = playerObj.GetComponent<Player>();
        player.playerNum = playerNum;
        playerObj.GetComponent<SpriteRenderer>().color = playerColors[playerNum - 1];
        return player;
    }
}
