using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject levelSelectionPanel;

    public Button startButton;
    public Button exitButton;
    public Button level1Button;
    public Button level2Button;
    public Button backButton;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(QuitGame);
        level1Button.onClick.AddListener(LoadLevel1);
        level2Button.onClick.AddListener(LoadLevel2);
        backButton.onClick.AddListener(GoBackToLobby);

        startPanel.SetActive(true);
        levelSelectionPanel.SetActive(false);
    }

    public void StartGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        startPanel.SetActive(false);
        levelSelectionPanel.SetActive(true);
    }

    public void LoadLevel1()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene("Single_Player_Gameplay");
    }

    public void LoadLevel2()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        SceneManager.LoadScene("Co-Op_Gameplay"); 
    }

    public void GoBackToLobby()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        levelSelectionPanel.SetActive(false);
        startPanel.SetActive(true);
    }

    public void QuitGame()
    {
        SoundManager.Instance.Play(Sounds.ButtonClick);
        Application.Quit();
        Debug.Log("Application Quit !!!");
    }

    private void OnDestroy()
    {
        startButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
        level1Button.onClick.RemoveAllListeners();
        level2Button.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}
