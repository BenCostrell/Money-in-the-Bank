using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Player[] players;
    public Vector2[] playerSpawns;
    public Color[] playerColors;
    public GameObject timer;
    public GameObject winText;
    private Text timerText;
    public float timeLimit;
    private float timeRemaining;
    private bool gameOver;

	void Awake()
    {
        InitializeServices();
        timerText = timer.GetComponent<Text>();
    }
    
    // Use this for initialization
	void Start () {
        InitializePlayers();
        timeRemaining = timeLimit;
        winText.SetActive(false);
        gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Reset"))
        {
            Reset();
        }
        Services.TaskManager.Update();
        if (!gameOver)
        {
            UpdateTimer();
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

    void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        int roundedTime = Mathf.CeilToInt(timeRemaining);
        timerText.text = roundedTime.ToString();
        if (roundedTime == 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        gameOver = true;
        Services.EventManager.Fire(new GameOver());
        int[] points = TallyPoints();
        winText.SetActive(true);
        players[0].actionable = false;
        players[1].actionable = false;
        if (points[0] > points[1])
        {
            winText.GetComponent<Text>().text = "PLAYER 1 WINS";
        }
        else if (points[1] > points[0])
        {
            winText.GetComponent<Text>().text = "PLAYER 2 WINS";
        }
        else
        {
            winText.GetComponent<Text>().text = "TIE GAME";
        }
    }

    int[] TallyPoints()
    {
        int[] points = new int[2] { 0, 0 };

        for (int i = 0; i < 2; i++)
        {
            List<Bank> bankList = players[i].activeBanks;
            if (bankList.Count > 0)
            {
                foreach (Bank bank in bankList)
                {
                    points[i] += bank.coinsStored;
                }
            }
        }

        return points;
    }
}
