using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Player
{
    Human, // =0
    Ai // =1
}

public class GameLogic : MonoBehaviour
{
    public float countdown;
    public Text playerScoreTxt;
    public Text opponentScoreTxt;
    public Text countdownTxt;

    public GameObject canvas;

    public GameObject bomb;

    private Scene initScene;


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

    }

    // Update is called once per frame
    void Update()
    {
        playerScoreTxt.text = playerScore.ToString();
        opponentScoreTxt.text = opponentScore.ToString();

        countdown -= Time.deltaTime;
        countdownTxt.text = ShowCountdown();
        if (countdown<0.0f)
        {
            Debug.Log("BOOM!");
            winsPoint = Player.Human;
            AssignPoint(winsPoint);

            NewRally();
        }

        if (playerScore>4.5f)
        {
            winsGame = Player.Human;
            EndGame();
        }
        else if (opponentScore > 4.5f)
        {
            winsGame = Player.Ai;
            EndGame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Out") || other.CompareTag("Wall"))
        {
            if (tableTouches == 0)
            {
                if (lastTouchedBall == Player.Ai)
                {
                    Debug.Log("AI OUT! Point for Human");
                    winsPoint = Player.Human;
                }
                else
                {
                    Debug.Log("Human OUT! Point for AI");
                    winsPoint = Player.Ai;
                }

                AssignPoint(winsPoint);

                NewRally();
            }
            if (tableTouches >= 1)
            {
                if (lastTouchedBall == Player.Ai)
                {
                    Debug.Log("Player missed! Point for AI");
                }
                else
                {
                    Debug.Log("AI missed! Point for Player");
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
        Debug.Log("Starting New Rally!");
        tableTouches = 0;
        countdown = 10;

        SceneManager.LoadScene(initScene.name);
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
