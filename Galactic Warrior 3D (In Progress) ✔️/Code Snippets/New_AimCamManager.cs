using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to control the aim camera and shooting

public class New_AimCamManager : MonoBehaviour
{
    public Cinemachine.AxisState _xAxis, _yAxis;

    public bool _isCodeDisabled = true;

    public float _Xvalue;

    public Transform _aim;

    [SerializeField] float _mouseSense;

    public Transform _aimPos;
    [SerializeField] float _aimSmoothSpeed;
    [SerializeField] LayerMask _aimMask;
    [SerializeField] Transform _gun;
    [SerializeField] float _turnSpeed;

    Vector2 _screenCentre;

    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _barrelTransform;
    [SerializeField] Transform _bulletPrefabParent;

    public bool _alreadyCalledOnce = false;
    public bool _coroutineEnded = false;
    int recoilanim;
    [SerializeField] GameObject _crossHair;

    CameraSwitcher _cameraSwitcher;

    [SerializeField] CinemachineBrain _mainCam;

    private void Start()
    {
        _cameraSwitcher = GetComponent<CameraSwitcher>();
    }

    // Update is called once per frame
    void Update()
    {
        _Xvalue += Input.GetAxisRaw("Mouse X") * _mouseSense;

        _xAxis.Value = WrapAround(_Xvalue, -180f, +180f);
        _yAxis.Value -= Input.GetAxisRaw("Mouse Y") * _mouseSense;
        _yAxis.Value = Mathf.Clamp(_yAxis.Value, -60, 60);

        _screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray _ray = Camera.main.ScreenPointToRay(_screenCentre);

        if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity, _aimMask))
        {
            _aimPos.position = Vector3.Lerp(_aimPos.position, hit.point, _aimSmoothSpeed * Time.deltaTime);
        }

        if (!_isCodeDisabled)
        {
            if (!_alreadyCalledOnce)
            {
                _alreadyCalledOnce = true;
                StartCoroutine(WaitBefore());
            }
            else if (_coroutineEnded)
            {
                //Shoot
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameObject _bullet = GameObject.Instantiate(_bulletPrefab, _barrelTransform.position, _barrelTransform.rotation, _bulletPrefabParent);
                    BulletController _bulletController = _bullet.GetComponent<BulletController>();
                    _bulletController._target = hit.point;
                    _bulletController._hit = true;

                }

                //Gun Look At Lerp
                Vector3 relativePos = _aimPos.position - _gun.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
                _gun.rotation = Quaternion.Lerp(_gun.rotation, rotation, _turnSpeed);
            }

            //RotatePlayer
            _aim.localEulerAngles = new Vector3(_yAxis.Value, _aim.localEulerAngles.y, _aim.localEulerAngles.z);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, _xAxis.Value, transform.eulerAngles.z);
        }

    }
    private float WrapAround(float _value, float _minValue, float _maxValue)
    {
        float _range = _maxValue - _minValue;

        while (_value < _minValue)
        {
            _value += _range;
        }

        while (_value >= _maxValue)
        {
            _value -= _range;
        }

        return _value;
    }

    IEnumerator WaitBefore()
    {
        yield return new WaitForSeconds(_mainCam.m_DefaultBlend.m_Time);

        if (_cameraSwitcher._actionCamEnabled)
        {
            _crossHair.SetActive(true);
        }

        _coroutineEnded = true;
    }
}
