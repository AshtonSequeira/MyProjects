using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to control the Player character controller

public class PlayerController: MonoBehaviour
{
    [SerializeField] CharacterController _characterController;

    [SerializeField] float _moveSpeed ;

    [SerializeField] Transform _camera;

    [SerializeField] float _gravity;

    [SerializeField] float _jumpHeight;

    [SerializeField] float _jumpSpeed;

    [SerializeField] int _jumpInAirCount;

    [SerializeField] Transform _groundCheck;

    [SerializeField] float _groundDistance;

    [SerializeField] LayerMask _groundMask;

    [SerializeField]public bool _isGrounded,aim1, _shift;

    [SerializeField] bool _isGroundedCheck;

    [SerializeField] Vector3 offset;

    [SerializeField] GameObject player,pl_fwd_ref;

    [SerializeField] Vector3 crouch_size;    

    [SerializeField] GameObject rb,aim,aim_ref;

    [SerializeField] public bool movement = true;


    public float _horizontalInput;
    public float _verticalInput;
    float _targetAngle;
    float _newTargetAngle;
    float _angle;
    float _smoothingTime = 0.1f;
    float _smoothVelocity;
    int _jumpInAirCounter;


    public Animator _animator;

    public Vector3 _direction;
    Vector3 _moveDirection;
    Vector3 _gravityVelocity = Vector3.zero;
    Vector3 _jumpVelocity = Vector3.zero;

    private void Awake()  //Initialise gravity
    {
        _gravityVelocity.y = _gravity* Time.deltaTime;

        _jumpInAirCounter = _jumpInAirCount;

        Cursor.lockState = CursorLockMode.Locked;

        crouch_size = new Vector3(1f, 0.7f, 1f);        
    }


    void Update()
    {
        if (movement)
        {
            ApplyGravityAndJump();
            MoveAndRotate();
        }
    }

    void MoveAndRotate()  //Used for movement of the character with respect to the camera
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        _direction = new Vector3(_horizontalInput, 0f, _verticalInput).normalized;
        
        if (_direction.magnitude >= 0.1f)
        {
            _animator.SetBool("run", true);
            _targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
           
            _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _smoothVelocity, _smoothingTime);
            transform.rotation = Quaternion.Euler(0f, _angle, 0f);

            
            _moveDirection = Quaternion.Euler(0f, _targetAngle, 0f) * Vector3.forward;
            _characterController.Move(_moveDirection.normalized * _moveSpeed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("run", false);
        }
    }

    void ApplyGravityAndJump()  //used to apply gravity on the player and jumping
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask); 

        _isGroundedCheck = _characterController.isGrounded;               

        if (Input.GetMouseButtonDown(1))
        {
            _animator.SetBool("aim", true);
            aim1 = true;
           
        }
        if (Input.GetMouseButtonUp(1))
        {
            _animator.SetBool("aim", false);
            aim1 = false;
            aim_ref.transform.position = rb.transform.position+new Vector3(1f,0f,3f);
        }

        if (_isGrounded && _gravityVelocity.y < 0)
        {
            _animator.SetBool("land", true);
            _gravityVelocity.y = -2f;

            _jumpInAirCounter = _jumpInAirCount;
        }
        if (_isGrounded == false)
        {
            _animator.SetBool("land", false);

        }

        if (Input.GetKeyDown(KeyCode.LeftControl)) //used for crouching
        {
            _animator.SetBool("crouch", true);
            _shift = false;
            _characterController.height = 1f;
            player.transform.localPosition =  new Vector3(0f, -0.564999f, 0f);
            _moveSpeed = 5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            _animator.SetBool("crouch", false);
            _shift = true;
            _characterController.height = 2f;
            player.transform.localPosition = new Vector3(0, -1.10300004f, 0);
            _moveSpeed = 10f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded) //used for jumping
        {
            _animator.SetTrigger("jump");
            _animator.SetBool("land", false);
            _gravityVelocity.y = Mathf.Sqrt(_jumpHeight * 2 * _jumpSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && _jumpInAirCounter > 0 && !_isGrounded)
        {
            _gravityVelocity.y = Mathf.Sqrt(_jumpHeight * 2 * _jumpSpeed);

            _jumpInAirCounter--;
        }

        _gravityVelocity.y -= _gravity * Time.deltaTime;

        _characterController.Move(_gravityVelocity * Time.deltaTime);

        if (aim1)  //used for aiming the gun
        {
            aim_ref.transform.position = aim.transform.position;
        }
        else
        {
            aim_ref.transform.position = pl_fwd_ref.transform.position;
        }
    }

}
