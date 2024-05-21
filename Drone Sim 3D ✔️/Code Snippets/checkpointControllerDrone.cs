using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Controls each and every checkpoint of the level, also the entry and exit of drone in the checkpoint loop

public class checkpointControllerDrone : MonoBehaviour
{
    public static bool _correctEntryExit = false;

    checkpointController _checkpointController;
    
    [SerializeField] checkpointSingle[] _checkpoints;

    bool _lastIsRaceOn = false;

    [SerializeField] GameManager _gameManager;

    bool _correctEnter = false;

    bool _wrongWay = false;

    int _activeCheckpoint;
   
    private void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        SetNextCheckpoint();

    }

    private void Update()
    {
        if (GameManager._isRaceOn != _lastIsRaceOn) //start if race is on
        {
            Debug.Log("Race On: " + GameManager._isRaceOn);

            if (GameManager._isRaceOn)
            {
                _activeCheckpoint = 0;
                _checkpoints[_activeCheckpoint].SetObjectActive(true);
            }
            else
            {
                _activeCheckpoint = 0;
                for (int i = 0; i < _checkpoints.Length; i++)
                {
                    _checkpoints[i].SetObjectActive(false);
                }
            }

            _lastIsRaceOn = GameManager._isRaceOn;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "in")
        {
            if(_wrongWay) //Wrong entry
            {
                Debug.Log("Wrong Entry and Exit ");
                _wrongWay = false;
            }
            else
            {
                _correctEnter = true;
            }
        }

        if (other.gameObject.tag == "out")
        {
            if(_correctEnter) //correct entry
            {
                Debug.Log("Correct Entry and Exit");

                _correctEntryExit = true;
                if(_activeCheckpoint< _checkpoints.Length)
                {
                    Passed();
                }

                if (_activeCheckpoint == _checkpoints.Length) //Final checkpoint reached, you win this level
                {
                    Debug.Log("Race Completed");

                    _gameManager.StopRace();

                }

                _correctEnter = false;
            }
            else
            {
                _wrongWay = true;

            }
        }
    }

    void Passed()  //Go to next checkpoint
    {
        _checkpoints[_activeCheckpoint].SetObjectActive(false);

        _activeCheckpoint++;

        if (_activeCheckpoint < _checkpoints.Length)
        {
            _checkpoints[_activeCheckpoint].SetObjectActive(true);
        }

    }

    void SetNextCheckpoint()  //initialise next checkpoints
    {
        for(int i = 0; i < (_checkpoints.Length - 1); i++)
        {
            _checkpoints[i].SetNext(_checkpoints[i + 1]);
        }

    }
}
