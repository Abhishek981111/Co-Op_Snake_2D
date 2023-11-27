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

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void GameOver(int blueScore, int greenScore)
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

    // Function to restart the game (attached to the Restart button)
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Function to return to the lobby (attached to the Lobby button)
    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Lobby"); 
    }

    public void HandleSnakeDeath(string snakeTag)
    {
        if(snakeTag == "Player1" || snakeTag == "Player2")
        {
            SnakeController blueSnake = GameObject.FindGameObjectWithTag("Player1").GetComponent<SnakeController>();
            SnakeController greenSnake = GameObject.FindGameObjectWithTag("Player2").GetComponent<SnakeController>();

            int blueScore = blueSnake.score;
            int greenScore = greenSnake.score;

            GameOver(blueScore, greenScore);
        }
    }

    private void OnEnable()
    {
        GameObject restartButton = GameObject.Find("RestartButton");
        Button restartBtn = restartButton.GetComponent<Button>();
        restartBtn.onClick.AddListener(RestartGame);

        GameObject lobbyButton = GameObject.Find("LobbyButton");
        Button lobbyBtn = lobbyButton.GetComponent<Button>();
        lobbyBtn.onClick.AddListener(ReturnToLobby);
    }

    private void OnDisable()
    {
        GameObject restartButton = GameObject.Find("RestartButton"); 
        Button restartBtn = restartButton.GetComponent<Button>();
        restartBtn.onClick.RemoveAllListeners();

        GameObject lobbyButton = GameObject.Find("LobbyButton"); 
        Button lobbyBtn = lobbyButton.GetComponent<Button>();
        lobbyBtn.onClick.RemoveAllListeners();
    }
}
