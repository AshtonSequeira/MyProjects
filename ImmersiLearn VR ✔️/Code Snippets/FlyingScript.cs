using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//This script is used to fly vertically in the VR space

public class FlyingScript : MonoBehaviour
{
    [SerializeField] InputActionProperty _flying;
    [SerializeField] private CharacterController _cc;
    [SerializeField] float _flySpeed = 2f;
    Vector3 _movement;
        
    // Update is called once per frame
    void Update()
    {
        Vector2 _axisVal = _flying.action.ReadValue<Vector2>();   //Reads joystick inputs from the controller
        if (_axisVal.y > 0.5f || _axisVal.y < -0.5f)    //DeadZone
        {
            Fly(); 
        }
        else
        {
            _movement.y = 0f;
        }

        _cc.Move(_movement * Time.deltaTime);

    }

    private void Fly()   //Fly with respect to the joystick value
    {
        Vector2 _axisVal = _flying.action.ReadValue<Vector2>(); 
        _movement.y = _axisVal.y * _flySpeed;
    }
}
