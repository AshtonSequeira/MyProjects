using droneSim;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;

//Used to control the drone physics and also change the cameras

[RequireComponent(typeof(droneInputs))]

public class droneController : base_rigidbody
{
    [SerializeField]private float _minMaxPitch = 30f;
    [SerializeField] private float _minMaxRoll = 30f;
    [SerializeField] float _yawPower = 4f;
    [SerializeField] private float _lerpSpeed = 2f;
    [SerializeField] private float _maxPower = 10f;

    [SerializeField] CinemachineVirtualCamera _LookAtCamera;
    [SerializeField] CinemachineVirtualCamera _FPcamera;
    [SerializeField] CinemachineVirtualCamera _TPcamera;

    private float _finalPitch;
    private float _finalRoll;
    private float _finalYaw;
    private float _yaw;
    public bool _stopEngine = false;

    private int _liveCamContainer = 1;

    private droneInputs _input;

    private List<IEngine> _engines = new List<IEngine>(); 

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<droneInputs>();
        
        _engines = GetComponentsInChildren<IEngine>().ToList<IEngine>();
    }

    protected override void HandlePhysics()
    {
        if (!_stopEngine)
        {
            HandleEngines();
            HandleControls();
        }
        else return;
    }

    protected virtual void HandleEngines()
    {
       foreach(IEngine engine in _engines)
        {
            engine.updateEngine(_rb, _input, _maxPower);
        }
    }

    private void OnEnable()
    {
        cameraSwitcher.Register(_LookAtCamera);
        cameraSwitcher.Register(_FPcamera);
        cameraSwitcher.Register(_TPcamera);
    }

    private void OnDisable()
    {
        cameraSwitcher.UnRegister(_LookAtCamera);
        cameraSwitcher.UnRegister(_FPcamera);
        cameraSwitcher.UnRegister(_TPcamera);
    }


    private void Update()
    {
        if (_input.Reset)
        {
            Reset();
        }

        if (_liveCamContainer != _input.LiveCam)    //change camera
        {
            if (_input.LiveCam == 1)
            {
                cameraSwitcher.SwitchCamera(_LookAtCamera);
                Debug.Log("Cam 1 is Active");
                _liveCamContainer = 1;
            }
            else if (_input.LiveCam == 2)
            {
                cameraSwitcher.SwitchCamera(_FPcamera);
                Debug.Log("Cam 2 is Active");
                _liveCamContainer = 2;
            }
            else if (_input.LiveCam == 3)
            {
                cameraSwitcher.SwitchCamera(_TPcamera);
                Debug.Log("Cam 3 is Active");
                _liveCamContainer = 3;
            }
        }
    }

    protected virtual void HandleControls()  //Control the drone with new inputs
    {
        float _pitch = _input.Cyclic.y * _minMaxPitch;
        float _roll = -_input.Cyclic.x * _minMaxRoll;
        _yaw += _input.Pedals * _yawPower;

        _finalPitch = Mathf.Lerp(_finalPitch, _pitch, Time.deltaTime * _lerpSpeed);
        _finalRoll = Mathf.Lerp(_finalRoll, _roll, Time.deltaTime * _lerpSpeed);
        _finalYaw = Mathf.Lerp(_finalYaw, _yaw, Time.deltaTime * _lerpSpeed);

        Quaternion _rot = Quaternion.Euler(_finalPitch, _finalYaw, _finalRoll);

        _rb.MoveRotation(_rot);

    }

    public void Reset()  //Reset the position of the drone
    {
        Debug.Log("Reset Position");

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(0f, 3.72f, 25f);

        _finalPitch = 0f;
        _finalYaw = 0f;
        _yaw = 0f;
        _finalRoll = 0f;
        transform.localEulerAngles = Vector3.zero;
        _input.Reset = false;
    }

}
