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
    public GameObject winBackground;
    public GameObject[] scoreObjects;
    private Text timerText;
    private Text[] scoreText;
    public float timeLimit;
    private float timeRemaining;
    private bool gameOver;

	void Awake()
    {
        InitializeServices();
        timerText = timer.GetComponent<Text>();
        scoreText = new Text[2];
        scoreText[0] = scoreObjects[0].GetComponent<Text>();
        scoreText[1] = scoreObjects[1].GetComponent<Text>();
    }
    
    // Use this for initialization
	void Start () {
        InitializePlayers();
        timeRemaining = timeLimit;
        winBackground.SetActive(false);
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
            UpdateScore();
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

    void UpdateScore()
    {
        for (int i = 0; i < 2; i++)
        {
            scoreText[i].text = players[i].score.ToString();
        }
    }

    void EndGame()
    {
        gameOver = true;
        Services.EventManager.Fire(new GameOver());
        winBackground.SetActive(true);
        players[0].actionable = false;
        players[1].actionable = false;
        if (players[0].score > players[1].score)
        {
            winText.GetComponent<Text>().text = "PLAYER 1 WINS";
        }
        else if (players[1].score > players[0].score)
        {
            winText.GetComponent<Text>().text = "PLAYER 2 WINS";
        }
        else
        {
            winText.GetComponent<Text>().text = "TIE GAME";
        }
    }
}
