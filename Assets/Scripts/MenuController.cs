using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject winScreen;
    public GameObject gameOverScreen;

    
    private bool isPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && isPaused == false)
        {
            Pause();
        }
        else if (Input.GetButtonDown("Cancel") && isPaused == true)
        {
            Continue();
        }
    }
    public void MainMenuOnStartClick()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   
    }

}
