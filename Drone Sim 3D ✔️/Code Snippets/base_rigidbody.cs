using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script holds the rigitBody component of the drone

[RequireComponent(typeof(Rigidbody))]

public class base_rigidbody : MonoBehaviour
{
    [SerializeField]private float _weightInKg = 1.0f;

    protected Rigidbody _rb;

    protected float _startDrag;
    protected float _startAngularDrag;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass= _weightInKg;
        _startDrag = _rb.drag;
        _startAngularDrag= _rb.angularDrag;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_rb)
        {
            return;
        }

        HandlePhysics();
    }

    protected virtual void HandlePhysics() { }
}
