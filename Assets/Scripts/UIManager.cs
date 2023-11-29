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
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (lobbyButton != null)
        {
            lobbyButton.onClick.AddListener(LoadLobby);
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

    public void LoadLobby()
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
            Debug.Log("Collision with other snake detected!!!");
        }
    }
}