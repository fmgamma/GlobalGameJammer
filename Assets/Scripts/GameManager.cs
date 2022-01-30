using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MENU,
        PLAYING,
        PAUSED,
        GAMEOVER
    };

    public static GameManager Instance => instance;
    public GameState sGameState = GameState.PLAYING;

    public GameObject pauseUI;
    public GameObject gameOverUI;

    public Text winText;

    private static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(sGameState == GameState.PLAYING)
            {
                PauseGame();
            }
            else if(sGameState == GameState.PAUSED)
            {
                PlayGame();
            }
        }
    }

    public void PlayGame()
    {
        LevelScript ls = GameObject.FindGameObjectWithTag("LevelScript").GetComponent<LevelScript>();
        SoundManager sm = SoundManager.Instance;

        sm.UnPauseSound(ls.LevelMusic);
        sm.StopSound(SoundManager.SoundNames.pauseMenuMusic);
        ls.SetBackgroundVolume(1);

        sGameState = GameState.PLAYING;
        Time.timeScale = 1.0f;
        pauseUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        LevelScript ls = GameObject.FindGameObjectWithTag("LevelScript").GetComponent<LevelScript>();
        SoundManager sm = SoundManager.Instance;
        sm.PlaySound(SoundManager.SoundNames.gamePaused);
        sm.PauseSound(ls.LevelMusic);
        //ls.StopBackgroundNoise(); //If you do notwant background on pause
        ls.SetBackgroundVolume(0.1f);
        sm.PlaySound(SoundManager.SoundNames.pauseMenuMusic);

        sGameState = GameState.PAUSED;
        Time.timeScale = 0.0f;
        pauseUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void EndGame(bool leftSideWon)
    {
        if(sGameState == GameState.GAMEOVER)
        {
            return;
        }
        sGameState = GameState.GAMEOVER;
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        gameOverUI.SetActive(true);
        if(leftSideWon)
        {
            winText.text = "Player 1 WINS!";
        }
        else
        {
            winText.text = "Player 2 WINS!";
        }
        
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        sGameState = GameState.PLAYING;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
