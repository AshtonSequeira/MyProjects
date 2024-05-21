using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached on the last Block

public class FinishController : MonoBehaviour
{
    [SerializeField] float _speed = 25;
    [SerializeField] public bool _levelCleared = false;

    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = -transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Start")  //if it hits the Start Block you win
        {
            Debug.Log("You Win!");
            _levelCleared = true;
            _speed = 0;
        }
    }

}
