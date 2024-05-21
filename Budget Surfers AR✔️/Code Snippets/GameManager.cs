using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Manages the game

public class GameManager : MonoBehaviour
{
    [SerializeField] AR_Curser _arCurser;
    [SerializeField] FinishController _check;
    [SerializeField] GameObject _youWin;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        _arCurser = GameObject.Find("AR Curser").GetComponent<AR_Curser>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;  //screen never sleeps while playing
    }

    private void Update()
    {
        if(_arCurser._levelReady)  //if level is ready, check if you won the level
        {
            _check = GameObject.FindWithTag("Finish").GetComponent<FinishController>();
            
            if (_check._levelCleared)
            {
                _youWin.SetActive(true);
                Debug.Log("You win Active");
            }
        }
    }

    public void ResetScene()   //Reseting the Level
    {
        SceneManager.LoadScene("BeatSaberAR");
        
    }

}
