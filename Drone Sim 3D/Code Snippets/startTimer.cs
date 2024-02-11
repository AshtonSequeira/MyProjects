using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

//Used to start/stop the race timer

public class startTimer : MonoBehaviour
{
    bool _startTimerActive = false;
    float _currentTime;
    int _startTime = 4;

    [SerializeField] TMP_Text _startTimer;

    [SerializeField] GameObject _startTimerUI;

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = _startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_startTimerActive)
        {
            _currentTime -= Time.deltaTime;

            TimeSpan _time = TimeSpan.FromSeconds(_currentTime);

            if (_currentTime < 1)
            {
                _startTimer.text = "GO!";

                if(_currentTime < -1)
                {
                    StopTimer();
                }
            }
            else 
            {
                _startTimer.text = _time.Seconds.ToString();
            }
        }



    }
    public void StartTimer()
    {
        _currentTime = 4;
        _startTimerUI.SetActive(true);
        _startTimerActive = true;
    }

    public void StopTimer()
    {
        _startTimerUI.SetActive(false);
        _startTimerActive = false;
    }

}
