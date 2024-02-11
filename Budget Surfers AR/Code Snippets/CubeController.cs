using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script is attached on every normal Cube

public class CubeController : MonoBehaviour
{
    [SerializeField] float _speed = 1;

    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb= GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rb.velocity = -transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)  //Destroy the normal cube if it hits the start object
    {
        if(other.gameObject.tag == "Start")
        {
            Destroy(gameObject);
        }
    }
}
