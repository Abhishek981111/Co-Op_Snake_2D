using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pausePanel;
    public Button pauseButton;
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;

    private bool isGamePaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        pauseButton.onClick.AddListener(TogglePause);
        resumeButton.onClick.AddListener(ResumeGame);
        resumeButton.gameObject.SetActive(false);
        restartButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            SoundManager.Instance.Play(Sounds.ButtonClick);
            Time.timeScale = 0f; 
            pausePanel.SetActive(true);
            pauseButton.gameObject.SetActive(false); 
            resumeButton.gameObject.SetActive(true); 
        }
        else
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Time.timeScale = 1f; 
        pausePanel.SetActive(false);
        resumeButton.gameObject.SetActive(false); 
        pauseButton.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; 
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene("Lobby"); 
    }

    public void OnDestroy()
    {
        pauseButton.onClick.RemoveAllListeners();
        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
    }
}
