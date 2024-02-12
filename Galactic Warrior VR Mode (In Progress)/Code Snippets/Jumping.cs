using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Used to jump in VR

public class Jumping : MonoBehaviour
{
    [SerializeField] private InputActionProperty _jumpButton;
    [SerializeField] private float _jumpHeight = 3f;
    [SerializeField] private CharacterController _cc;
    [SerializeField] private LayerMask _groundLayers;


    private float _gravity = Physics.gravity.y;
    Vector3 _movement;

    // Update is called once per frame
    void Update()
    {
        bool _isGrounded = IsGrounded();

        if (_jumpButton.action.WasPressedThisFrame() && _isGrounded)
            Jump();

        _movement.y += _gravity * Time.deltaTime;

        _cc.Move(_movement * Time.deltaTime);

    }

    private void Jump()
    {
        _movement.y = Mathf.Sqrt(_jumpHeight * -3 * _gravity);
    }


    private bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.2f, _groundLayers);
    }

}
