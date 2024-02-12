using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Used to fire a bullet from the gun when the trigger is pressed

public class FireBulletOnActivate : MonoBehaviour
{
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _barrelPos;
    [SerializeField] float _bulletSpeed = 20;
    [SerializeField] AudioSource _shootingAudio;
    [SerializeField] ParticleSystem _muzzleFlash;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable _grabbable = GetComponent<XRGrabInteractable>();
        _grabbable.activated.AddListener(FireBullet);
    }

    public void FireBullet(ActivateEventArgs args)
    {
        _shootingAudio.Play();
        _muzzleFlash.Play();
        GameObject _spawnBullet = Instantiate(_bulletPrefab, _barrelPos.position, _barrelPos.rotation);
        _spawnBullet.GetComponent<Rigidbody>().velocity = _barrelPos.forward * _bulletSpeed;
        Destroy(_spawnBullet ,5);
    }
}
