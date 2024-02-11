using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]

    [SerializeField] CharacterController _characterController; //Character Controller of the Player Capsule

    [SerializeField] float _moveSpeed = 10f; //Speed to player

    [SerializeField] Transform _camera;

    [SerializeField] float _gravity = 20f;

    [SerializeField] float _jumpHeight = 10f;

    [SerializeField] float _jumpSpeed = 5f;

    [SerializeField] int _jumpInAirCount = 1;

    [SerializeField] Transform _groundCheck;

    [SerializeField] float _groundDistance = 0.15f;

    [SerializeField] LayerMask _groundMask;

    [SerializeField] bool _isGrounded;

    [SerializeField] bool _isGroundedCheck;

    [Header("Camera Settings")]

    public float _horizontalInput;
    public float _verticalInput;
    float _targetAngle;
    float _newTargetAngle;
    float _angle;
    float _smoothingTime = 0.1f;
    float _smoothVelocity;
    int _jumpInAirCounter;

    public Vector3 _direction;
    Vector3 _moveDirection;
    Vector3 _gravityVelocity = Vector3.zero;
    Vector3 _jumpVelocity = Vector3.zero;

    private void Awake()
    {
        _gravityVelocity.y = _gravity* Time.deltaTime;  //Add Gravity at the start

        _jumpInAirCounter = _jumpInAirCount;

        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        ApplyGravityAndJump(); //Used for gravity and jumping

        MoveAndRotate();  //Used to Move the player
    }

    void MoveAndRotate()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal"); //WASD or arrow key inputs
        _verticalInput = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(_horizontalInput, 0f, _verticalInput).normalized;
        
        if (_direction.magnitude >= 0.1f)
        {
            _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;     //Calculate the Target angle with respect to the camera rotation
           
            _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _smoothVelocity, _smoothingTime);
            transform.rotation = Quaternion.Euler(0f, _angle, 0f);

            
            _moveDirection = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;    
            _characterController.Move(_moveDirection.normalized * _moveSpeed * Time.deltaTime);     //Move the Player
        }
    }

    void ApplyGravityAndJump()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);    //To check if player is on the ground

        _isGroundedCheck = _characterController.isGrounded;

        if(_isGrounded && _gravityVelocity.y < 0)  //Stay on the ground
        {
            _gravityVelocity.y = -2f;

            _jumpInAirCounter = _jumpInAirCount;
        }

        if(Input.GetKeyDown(KeyCode.Space) && _isGrounded)   //Jump
        {
            _gravityVelocity.y = Mathf.Sqrt(_jumpHeight * 2 * _jumpSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _jumpInAirCounter > 0 && !_isGrounded)   //Jump Again
        {
            _gravityVelocity.y = Mathf.Sqrt(_jumpHeight * 2 * _jumpSpeed);

            _jumpInAirCounter--;
        }

        _gravityVelocity.y -= _gravity * Time.deltaTime;

        _characterController.Move(_gravityVelocity * Time.deltaTime);
    }
}
