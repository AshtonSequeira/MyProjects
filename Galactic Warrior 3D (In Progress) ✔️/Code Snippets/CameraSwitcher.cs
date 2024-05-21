using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Microsoft.Unity.VisualStudio.Editor;

//Used to switch between the Free look and Aim camera

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _actionCam;
    [SerializeField] CinemachineFreeLook _freeLookCam;
    [SerializeField] GameObject _crossHair;

    AimCamManager _aimCamManager;

    PlayerController _player;

    Vector3 _direction = new Vector3(0, 0, 0); 

    public bool _actionCamEnabled = false;
    private void Start()
    {
        _player = GetComponent<PlayerController>();
        _aimCamManager = GetComponent<AimCamManager>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _aimCamManager._isCodeDisabled = false;

            if (!_actionCamEnabled)
            {
                _aimCamManager._Xvalue = _freeLookCam.m_XAxis.Value;
                _aimCamManager._yAxis.Value = -2.35f;
                _actionCam.Priority = 100;

                _actionCamEnabled = true;

                _freeLookCam.Priority = 1;

                Invoke(nameof(ShowReticle), 0.2f);
            }
            
        }
        else
        {
            if(_actionCamEnabled)
            {
                _freeLookCam.Priority = 100;                
                _actionCam.Priority = 1;
                if (_player._horizontalInput != 0 || _player._verticalInput < 0)
                {
                    _freeLookCam.m_XAxis.Value = _aimCamManager._xAxis.Value;
                    _freeLookCam.m_YAxis.Value = 0.5f;
                }
                else
                {
                    _freeLookCam.m_XAxis.Value = transform.rotation.eulerAngles.y;
                    _freeLookCam.m_YAxis.Value = 0.5f;
                }

                Invoke(nameof(ChangeCam), 0.3f);

                _aimCamManager._alreadyCalledOnce = false;
                _aimCamManager._coroutineEnded = false;
                _crossHair.SetActive(false);

            }
        }
    }

    void DisableScript()
    {
        _aimCamManager._isCodeDisabled = true;
    }

    void ShowReticle()
    {
        if(_aimCamManager._isCodeDisabled == false)
        {
            _crossHair.SetActive(true);
        }
    }

    void ChangeCam()
    {
        _actionCamEnabled = false;
        _aimCamManager._isCodeDisabled = true;
    }
}
