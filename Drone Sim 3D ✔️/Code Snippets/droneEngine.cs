using droneSim;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to each motor of the drone, controls the drone according to the inputs given

[RequireComponent(typeof(BoxCollider))]

public class droneEngine : MonoBehaviour, IEngine
{
    public void initEngine()
    {
        throw new System.NotImplementedException();
    }

    private Vector3 ConvertV2toV2(Vector2 v)
    {
        return new Vector3(v.x, 0f, v.y);
    }

    public void updateEngine(Rigidbody _rb, droneInputs input, float _maxPower)
    {
        Vector3 _engineForce = Vector3.zero;

        _engineForce = (transform.up * ((_rb.mass * Physics.gravity.magnitude) + (input.Throttle * _maxPower))) / 4f;

        _engineForce = _engineForce + (_rb.rotation * ConvertV2toV2(input.Cyclic) * _maxPower) / 4f;

        _rb.AddForce(_engineForce, ForceMode.Force);

    }
}
