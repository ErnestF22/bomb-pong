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
    public GameObject player, opponent;
    public static float countdown;
    public Text playerScoreTxt;
    public Text opponentScoreTxt;
    public Text countdownTxt;

    public GameObject canvas;

    public GameObject bomb;

    public GameObject serveCarefullyBtn;
    public GameObject pointWinnerPanel;
    public Text pointWinnerTxt;
    private string winnerString;

    public static bool isServing = true; //Note that it's the Player that always serves
    public Transform serveTargetPlayerSide;

    public int maxScore = 5;
    public static int playerScore = 0;
    public static int opponentScore = 0;

    public static Scene initScene;



    private float serveForce = 6.0f;

    private Transform ballInitTransf;
    private Transform bombInitTransf;

    private Player winsPoint;
    private Player lastTouchedBall;
    private Player winsGame;
    private int tableTouches;



    // Start is called before the first frame update
    void Start()
    {
        serveTargetPlayerSide.gameObject.GetComponent<Renderer>().enabled = false;

        initScene = SceneManager.GetActiveScene();

        Serve();

        tableTouches = 0;
        countdown = (float)PlayerPrefs.GetInt("countdown");
        countdownTxt.text = ShowCountdown();

        ballInitTransf = transform;
        bombInitTransf = bomb.transform;

        lastTouchedBall = Player.Human;

        winsPoint = Player.None;
        winsGame = Player.None;
    }

    // Update is called once per frame
    void Update()
    {
        playerScoreTxt.text = playerScore.ToString();
        opponentScoreTxt.text = opponentScore.ToString();

        countdown -= Time.deltaTime;
        countdownTxt.text = ShowCountdown();

        if (playerScore > maxScore - 0.5f)
        {
            winsGame = Player.Human;
            EndGame();
        }
        else if (opponentScore > maxScore - 0.5f)
        {
            winsGame = Player.Ai;
            EndGame();
        }

        if (countdown < 0.0f)
        {
            Debug.Log("BOOM!");

            //check on which side of the table the ball has blown up
            if (gameObject.transform.position.z >= 0.0f)
            {
                SoundManager.PlaySound("bomb_explosion");
                winsPoint = Player.Human;
                AssignPoint(winsPoint);
                winnerString = "Computer Blew Up!";
            }
            else
            {
                SoundManager.PlaySound("bomb_explosion");
                winsPoint = Player.Ai;
                AssignPoint(winsPoint);
                winnerString = "You Blew Up!";
            }

            NewRally();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isServing)
        {
            //Player is always serving
            if (collision.collider.CompareTag("Out") || collision.collider.CompareTag("Wall"))
            {
                SoundManager.PlaySound("bomb_explosion");
                if (tableTouches <= 1)
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
                if (tableTouches >= 2)
                {
                    SoundManager.PlaySound("bomb_explosion");
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


            if (collision.collider.CompareTag("Table"))
            {
                Debug.Log("TableTouches++");
                tableTouches++;
                if (tableTouches >= 3)
                {
                    SoundManager.PlaySound("bomb_explosion");
                    winsPoint = lastTouchedBall;
                    AssignPoint(winsPoint);
                    if (winsPoint == Player.Ai)
                        winnerString = "You missed!";
                    else if (winsPoint == Player.Human)
                        winnerString = "Great curve!";
                    NewRally();
                }
            }
        }
        else //if (!isServing)
        {
            if (collision.collider.CompareTag("Out") || collision.collider.CompareTag("Wall"))
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
                    SoundManager.PlaySound("bomb_explosion");
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


            if (collision.collider.CompareTag("Table"))
            {
                //check if ball hit on the table side of the player who just hit it
                if (tableTouches == 0)
                {
                    if (lastTouchedBall == Player.Ai && gameObject.transform.position.z >= 0.0f)
                    {
                        SoundManager.PlaySound("bomb_explosion");
                        winsPoint = Player.Human;
                        winnerString = "Need more strength!";
                        AssignPoint(winsPoint);
                        NewRally();
                    }
                    else if (lastTouchedBall == Player.Human && gameObject.transform.position.z < 0.0f)
                    {
                        SoundManager.PlaySound("bomb_explosion");
                        winsPoint = Player.Ai;
                        winnerString = "Need more strength!";
                        AssignPoint(winsPoint);
                        NewRally();
                    }
                }

                Debug.Log("TableTouches++");
                tableTouches++;
               
                if (tableTouches >= 2)
                {
                    SoundManager.PlaySound("bomb_explosion");
                    winsPoint = lastTouchedBall;
                    AssignPoint(winsPoint);
                    if (winsPoint == Player.Ai)
                        winnerString = "You missed!";
                    else if (winsPoint == Player.Human)
                        winnerString = "Great curve!";
                    NewRally();
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isServing)
        {
            if (other.CompareTag("Opponent"))
            {
                isServing = false;
                if (tableTouches <= 1)
                {
                    SoundManager.PlaySound("bomb_explosion");
                    winsPoint = Player.Human;
                    AssignPoint(winsPoint);
                    winnerString = "Bomb has to hit ground first!";
                    NewRally();
                    return;
                }
                Debug.Log("Resetting Table Touches");
                lastTouchedBall = Player.Ai;
                tableTouches = 0;
                return;
            }
        }
        else //if (!isServing)
        {
            if (other.CompareTag("Player"))
            {
                if (tableTouches == 0 && lastTouchedBall == Player.Ai)
                {
                    SoundManager.PlaySound("bomb_explosion");
                    winsPoint = Player.Ai;
                    AssignPoint(winsPoint);
                    winnerString = "Bomb has to hit ground";
                    NewRally();
                    return;
                }
                else if (tableTouches == 0 && lastTouchedBall == Player.Human)
                {
                    SoundManager.PlaySound("bomb_explosion");
                    winsPoint = Player.Ai;
                    AssignPoint(winsPoint);
                    winnerString = "Don't hit twice!";
                    NewRally();
                    return;
                }


                Debug.Log("Resetting Table Touches");
                lastTouchedBall = Player.Human;
                tableTouches = 0;
                return;
            }

            if (other.CompareTag("Opponent"))
            {
                if (tableTouches == 0 && lastTouchedBall == Player.Human)
                {
                    SoundManager.PlaySound("bomb_explosion");
                    winsPoint = Player.Human;
                    AssignPoint(winsPoint);
                    winnerString = "Bomb has to hit ground first!";
                    NewRally();
                    return;
                }
                else if (tableTouches == 0 && lastTouchedBall == Player.Ai)
                {
                    SoundManager.PlaySound("bomb_explosion");
                    winsPoint = Player.Human;
                    AssignPoint(winsPoint);
                    winnerString = "Don't hit twice!";
                    NewRally();
                    return;
                }

                Debug.Log("Resetting Table Touches");
                lastTouchedBall = Player.Ai;
                tableTouches = 0;
                return;
            }
        }
    }

    void Serve()
    {
        GameLogic.isServing = true;
        Debug.Log("Serving");
        Vector3 dir = serveTargetPlayerSide.position - transform.position;
        GetComponent<Rigidbody>().velocity = dir.normalized * serveForce + new Vector3(0.0f, -3.0f, 0.0f);
    }

    void AssignPoint(Player p)
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
        if (playerScore > maxScore - 0.5f || opponentScore > maxScore - 0.5f)
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
        isServing = true;
    }


    void EndGame()
    {
        if (winsGame == Player.Human)
            PlayerPrefs.SetString("winnerTxt", "YOU WIN!");
        else if (winsGame == Player.Ai)
            PlayerPrefs.SetString("winnerTxt", "YOU LOSE!");
        else
            Debug.Log("ERROR: Unknown Winner");

        countdown = (float)PlayerPrefs.GetInt("countdown");
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
