using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using System;

//This is to control the race timer script

public class timer : MonoBehaviour
{
    [SerializeField]bool _timerActive = false;
    float _currentTime;
    int _startTime = 0;

    [SerializeField] TMP_Text _timer;

    // Start is called before the first frame update
    void Start()
    {
        _currentTime = _startTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerActive )
        {
            _currentTime += Time.deltaTime;
        }
        
        TimeSpan _time = TimeSpan.FromSeconds(_currentTime);

        _timer.text = _time.ToString(@"mm\:ss\:ff");
    }

    public void StartTimer()
    {
        _timerActive = true;
    }

    public void StopTimer()
    {
        _timerActive = false;
    }

    public void SetTimer(int t)
    {
        _currentTime = t;
    }
}
