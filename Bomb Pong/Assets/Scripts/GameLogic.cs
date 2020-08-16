using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public enum Player
{
    Human, // =0
    Ai, // =1
    None //=2
}

public class GameLogic : MonoBehaviour
{
    public float countdown;
    public Text playerScoreTxt;
    public Text opponentScoreTxt;
    public Text countdownTxt;

    public GameObject canvas;

    public GameObject bomb;

    public GameObject serveCarefullyBtn;
    public GameObject pointWinnerPanel;
    public Text pointWinnerTxt;
    private string winnerString;

    public static Scene initScene;


    private Transform ballInitTransf;
    private Transform bombInitTransf;
    private static int playerScore = 0;
    private static int opponentScore = 0;
    private Player winsPoint;
    private Player lastTouchedBall;
    private Player winsGame;
    private int tableTouches;



    // Start is called before the first frame update
    void Start()
    {
        initScene = SceneManager.GetActiveScene();
        
        tableTouches = 0;
        countdown = (float) PlayerPrefs.GetInt("countdown");
        countdownTxt.text = ShowCountdown();

        ballInitTransf = transform;
        bombInitTransf = bomb.transform;

        lastTouchedBall = Player.Human;

        winsGame = Player.None;

    }

    // Update is called once per frame
    void Update()
    {
        playerScoreTxt.text = playerScore.ToString();
        opponentScoreTxt.text = opponentScore.ToString();

        countdown -= Time.deltaTime;
        countdownTxt.text = ShowCountdown();

        if (playerScore > 4.5f)
        {
            winsGame = Player.Human;
            EndGame();
        }
        else if (opponentScore > 4.5f)
        {
            winsGame = Player.Ai;
            EndGame();
        }

        if (countdown<0.0f)
        {
            Debug.Log("BOOM!");
            SoundManager.PlaySound("bomb_explosion");
            winsPoint = Player.Human;
            AssignPoint(winsPoint);

            winnerString = "Computer Blown Up!";

            NewRally();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Out") || other.CompareTag("Wall"))
        {
            SoundManager.PlaySound("bomb_explosion");
            if (tableTouches == 0)
            {
                if (lastTouchedBall == Player.Ai)
                {
                    Debug.Log("AI OUT! Point for Human");
                    winsPoint = Player.Human;
                    winnerString = "OUT!";
                }
                else
                {
                    Debug.Log("Human OUT! Point for AI");
                    winsPoint = Player.Ai;
                    winnerString = "OUT!";

                }

                AssignPoint(winsPoint);

                NewRally();
            }
            if (tableTouches >= 1)
            {
                if (lastTouchedBall == Player.Ai)
                {
                    Debug.Log("Player missed! Point for AI");
                    winnerString = "You missed!";
                }
                else
                {
                    Debug.Log("AI missed! Point for Player");
                    winnerString = "Computer missed!";
                }
                winsPoint = lastTouchedBall;
                AssignPoint(winsPoint);

                NewRally();
            }
            
        }

        if (other.CompareTag("Player"))
        {
            lastTouchedBall = Player.Human;
            tableTouches = 0;
            return;
        }

        if (other.CompareTag("Opponent"))
        {
            lastTouchedBall = Player.Ai;
            tableTouches = 0;
            return;
        }

        if (other.CompareTag("Table"))
        {
            tableTouches++;
            if (tableTouches >= 2)
            {
                winsPoint = lastTouchedBall;
                AssignPoint(winsPoint);
                winnerString = "You missed!";
                NewRally();
            }
        }
    }

    void AssignPoint (Player p)
    {
        if ((int)p == (int)Player.Human)
        {
            playerScore++;
        }
        else if ((int)p == (int)Player.Ai)
        {
            opponentScore++;
        }
    }

    public void NewRally() //reset to initial positions
    {
        tableTouches = 0;
        countdown = 10;
        if (playerScore > 4.5f || opponentScore > 4.5f)
            return;


        Debug.Log("Starting New Rally!");
        //Setup 'Serve Carefully' screen (with text that says who won the point)
        pointWinnerTxt.text = winnerString;
        SetupServeCarefully();

        
    }

    void SetupServeCarefully()
    {
        gameObject.SetActive(false);
        serveCarefullyBtn.SetActive(true);
        pointWinnerPanel.SetActive(true);
    }

    void EndGame()
    {
        if (winsGame == Player.Human)
            PlayerPrefs.SetString("winnerTxt", "YOU WIN!");
        else if (winsGame == Player.Ai)
            PlayerPrefs.SetString("winnerTxt", "YOU LOSE!");
        else
            Debug.Log("ERROR: Unknown Winner");

        countdown = (float) PlayerPrefs.GetInt("countdown");
        playerScore = 0;
        opponentScore = 0;

        tableTouches = 0;
        
        
        SceneManager.LoadScene("EndScene");
    }

    string ShowCountdown()
    {
        int floorCtd = Mathf.FloorToInt(countdown);
        int counter = Mathf.Max(floorCtd, 0);
        return counter.ToString();
    }
}
