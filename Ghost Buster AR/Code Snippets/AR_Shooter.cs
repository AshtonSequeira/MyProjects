using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

//Used to control the gun and the bullets

public class AR_Shooter : MonoBehaviour
{
    public Transform _aimPos;
    [SerializeField] float _aimSmoothSpeed = 0.5f;
    [SerializeField] LayerMask _aimMask;
    [SerializeField] AR_Curser _arCurser;
    [SerializeField] Transform _gun;
    [SerializeField] GameObject _ballPrefab;
    [SerializeField] Transform _barrelTransform;
    [SerializeField] Transform _ballPrefabParent;
    [SerializeField] float _bulletVelocity;
    [SerializeField] ParticleSystem _muzzleFlash;

    Vector3 _hitPos;

    // Update is called once per frame
    void Update()
    {
        if(_arCurser._levelReady)  //Works only if the level is ready
        {
            UpdateAimPos();
        } 
    }

    void UpdateAimPos()   //Gun Tracking and shooting mechanics
    {
        Vector2 _screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));

        Ray _ray = Camera.main.ScreenPointToRay(_screenPosition);

        if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity, _aimMask))
        {
            _aimPos.position = Vector3.Lerp(_aimPos.position, hit.point, _aimSmoothSpeed);
            _hitPos = _aimPos.position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _barrelTransform.LookAt(_hitPos);

            GameObject _ball = GameObject.Instantiate(_ballPrefab, _barrelTransform.position, _barrelTransform.rotation, _ballPrefabParent);
            Rigidbody _rb = _ball.GetComponentInChildren<Rigidbody>();

            _rb.AddForce(_barrelTransform.forward * _bulletVelocity, ForceMode.Impulse);   //Bullet is shot

            _muzzleFlash.Play();

            StartCoroutine(Recoil());
            

        }
    }
    IEnumerator Recoil()  //Gun Recoil is played
    {
        _gun.Rotate(new Vector3(1, 0, 0), -5);

        yield return new WaitForSeconds(0.1f);

        _gun.Rotate(new Vector3(1, 0, 0), 5);

    }
}
