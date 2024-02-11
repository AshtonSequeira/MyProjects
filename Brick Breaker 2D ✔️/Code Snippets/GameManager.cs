using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms;
using UnityEngine.Audio;

//Manages the full game, includes gameplay, UI, Level selection, Level Loading, Pause menu, Quit game

public class GameManager : MonoBehaviour
{
    public BallBehaviour _ball;

    public PlayerController _player;

    public BlockBehaviour[] _block;

    public bool _gameOver = false;

    public int _score = 0;

    public string _finalScore;

    public int _lives = 3;

    public string _level;

    public bool _isPaused = false;

    public GameObject _pauseMenu;

    private GameUiManager _uiManager;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnLevelLoaded;

        Debug.Log("Scene Loaded");
    }

    private void Start()
    {
        NewGame();   
    }

    private void NewGame()   //Starts a new game by resetting scores, lives and selecting the level 
    { 
        this._score = 0;
        this._lives = 3;

        this._level = LevelContainer._level;

        LoadLevel(this._level);
        
    }

    private void LoadLevel(string level)  //Used to load a level
    {
        this._level = level;

        _uiManager = GameObject.Find("Canvas").GetComponent<GameUiManager>();

        _uiManager._uiLives = this._lives;

        SceneManager.LoadScene(_level);
    }

    private void OnLevelLoaded(Scene _scene, LoadSceneMode _mode)   //Initializing the level
    {
        _ball = FindObjectOfType<BallBehaviour>();
        _player = FindObjectOfType<PlayerController>();
        _block = FindObjectsOfType<BlockBehaviour>();

    }

    private void GameOver(string _score)   //Loading the Game Over scene
    {
        Destroy(this.gameObject);

        ScoreContainer._score = _score;

        SceneManager.LoadScene("GameOver");
    }

    private void ResetLevel()    //Restarting a level
    {
        _player.ResetPlayer();
        _ball.ResetBall();
    }

    public void Hit(BlockBehaviour Block)    //Updating the score
    {
        this._score += Block._points;

        _uiManager._uiScore = this._score;

        if(Cleared())
        {
            Destroy(this.gameObject);

            _finalScore = _score.ToString();

            ScoreContainer._score = _finalScore;

            SceneManager.LoadScene("WinScene");
        }
    }

    private bool Cleared()  //Checking if all the blocks have been distroyed
    {
        for(int i = 0; i < _block.Length; i++)
        {
            if (_block[i].gameObject.activeInHierarchy && !_block[i]._unbreakable)
            {
                return false;
            }

        }
        return true;
    }

    public void Dead()   //Checking if Player is Dead
    {
        this._lives --;

        _uiManager._uiLives = this._lives;

        if(_lives <= 0)
        {
            Debug.Log("Game Over");
            _finalScore = _score.ToString();

            GameOver(_finalScore);
        }
        else
        {
            Debug.Log("Reset Level");
            ResetLevel();
        }
    }

    void Update()
    {
        if(_ball!= null)
        {
            if (!_isPaused)  //Clamping the ball's velocity
            {
                _ball._rigidbody.velocity = ClampMag(_ball._rigidbody.velocity, _ball._ballSpeed, _ball._ballSpeed);
                Debug.Log("Clamped Ball Velocty");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))   //Used for pausing the game
        {
            Debug.Log("Errror");
            if (_isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }



    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        _isPaused = false;
    }

    void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        _isPaused = true;
    }
    public void Retry()
    {
        Debug.Log(this.gameObject);
        Destroy(this.gameObject);
        Debug.Log(this.gameObject);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Global");
    }

    public void QuitToLevelMenu()
    {
        Destroy(this.gameObject);
        Time.timeScale = 1f;

        SceneManager.LoadScene("LevelSelection");
    }

    public static Vector3 ClampMag(Vector3 value, float max, float min)
    {
        double sm = value.sqrMagnitude;
        if (sm > (double)max * (double)max)
        {
            return value.normalized * max;
        }
        else if (sm < (double)min * (double)min)
        {
            return value.normalized * min;
        }

        return value;
    }

    public void CallGameOver()
    {
        _finalScore = _score.ToString();
        GameOver(_finalScore);
    }

}
