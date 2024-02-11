using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Manages the game

public class GameManager : MonoBehaviour
{
    [SerializeField] AR_Curser _arCurser;
    [SerializeField] CheckIfCleared _check;
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
        if(_arCurser._levelReady)     //if level is ready, check if you killed all the zombies/ghosts then you win
        {
            _check = GameObject.FindWithTag("Zombie Level").GetComponent<CheckIfCleared>();

            if(_check._levelCleared)
            {
                _youWin.SetActive(true);
            }
        }
    }

    public void ResetScene()   //Reseting the Level
    {
        SceneManager.LoadScene("Level 1");
        
    }

}
