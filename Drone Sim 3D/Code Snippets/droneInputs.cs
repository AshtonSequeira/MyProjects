using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This script is used for the new input system

namespace droneSim
{
    [RequireComponent(typeof(PlayerInput))]

    public class droneInputs : MonoBehaviour
    {
        private Vector2 _cyclic;
        private float _pedals;
        private float _throttle;
        private int _liveCam = 1;
        private bool _isRaceOn = false;
        private bool _pauseGame = false;
        private bool reset = false;

        //first create a new input in the Input Actions, then initialize the variable int the script and create the function(idk) below
        public Vector2 Cyclic { get => _cyclic; }
        public float Pedals { get => _pedals; }
        public float Throttle { get => _throttle; }
        public int LiveCam { get => _liveCam; }
        public bool IsRaceOn { get => _isRaceOn; set => _isRaceOn = value; }
        public bool PauseGame { get => _pauseGame; }
        public bool Reset { get => reset; set => reset = value; }

        private void OnCyclic(InputValue value)
        {
            _cyclic = value.Get<Vector2>();

        }
        private void OnPedals(InputValue value)
        {
            _pedals = value.Get<float>();
        }

        private void OnThrottle(InputValue value)
        {
            _throttle = value.Get<float>();
        }

        private void OnLookAtCamera()
        {
            _liveCam = 1;
        }
        
        private void OnFPcamera()
        {
            _liveCam = 2;
        }

        private void OnTPcamera()
        {
            _liveCam = 3;
        }

        private void OnStartRace()
        {
            if(GameManager._noInputs) return;

            if(GameManager._isRaceOn)
            {
                GameManager._isRaceOn = false;
            }
            else _isRaceOn = true;

        }

        private void OnStopRace()
        {
            if (GameManager._noInputs) return;

            _isRaceOn = false;
        }

        private void OnReset()
        {
            if (GameManager._noInputs) return;
            Reset = true;
        }

        private void OnPauseButton()
        {
            _pauseGame = !_pauseGame;
        }        

    }
}
