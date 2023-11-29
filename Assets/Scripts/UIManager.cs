using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI blueWinsText;
    public TextMeshProUGUI greenWinsText;
    public TextMeshProUGUI gameTieText;
    public GameObject gameOverPanel;

    public Button restartButton;
    public Button lobbyButton;

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (lobbyButton != null)
        {
            lobbyButton.onClick.AddListener(ReturnToLobby);
        }
    }

    public void GameOver(int blueScore, int greenScore)
    {
        if(gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (blueScore > greenScore)
            {
                blueWinsText.gameObject.SetActive(true);
                greenWinsText.gameObject.SetActive(false);
                gameTieText.gameObject.SetActive(false);
            }
            else if (greenScore > blueScore)
            {
                greenWinsText.gameObject.SetActive(true);
                blueWinsText.gameObject.SetActive(false);
                gameTieText.gameObject.SetActive(false);
            }
            else
            {
                gameTieText.gameObject.SetActive(true);
                greenWinsText.gameObject.SetActive(false);
                blueWinsText.gameObject.SetActive(false);
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void HandleSnakeDeath(string snakeTag)
    {
        SnakeController blueSnake = GameObject.FindGameObjectWithTag("Player1").GetComponent<SnakeController>();
        SnakeController greenSnake = GameObject.FindGameObjectWithTag("Player2").GetComponent<SnakeController>();

        if (blueSnake != null && greenSnake != null)
        {
            int blueScore = blueSnake.score;
            int greenScore = greenSnake.score;

            GameOver(blueScore, greenScore);
        }
    }

    // private void OnEnable()
    // {
    //     GameObject restartButton = GameObject.Find("RestartButton");
    //     Button restartBtn = restartButton.GetComponent<Button>();
    //     restartBtn.onClick.AddListener(RestartGame);

    //     GameObject lobbyButton = GameObject.Find("LobbyButton");
    //     Button lobbyBtn = lobbyButton.GetComponent<Button>();
    //     lobbyBtn.onClick.AddListener(ReturnToLobby);
    // }

    // private void OnDisable()
    // {
    //     GameObject restartButton = GameObject.Find("RestartButton");
    //     Button restartBtn = restartButton.GetComponent<Button>();
    //     restartBtn.onClick.RemoveAllListeners();

    //     GameObject lobbyButton = GameObject.Find("LobbyButton");
    //     Button lobbyBtn = lobbyButton.GetComponent<Button>();
    //     lobbyBtn.onClick.RemoveAllListeners();
    // }
}