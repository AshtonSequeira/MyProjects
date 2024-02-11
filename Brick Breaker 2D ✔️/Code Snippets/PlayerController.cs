using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Controls the Paddle player

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;

    public Rigidbody2D _rigidbody;

    float _moveDirection;

    public float _maxBounceAngle = 75f;

    public float _moveSpeed = 5f;

    Vector2 _moveVelocity;

    Vector3 _lastVelocity, _direction;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        TouchInput(); //used for touch inputs on the phone

        //WASDinputs();   //used for WASD or Arrow inputs on the PC

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallBehaviour _ball = collision.gameObject.GetComponent<BallBehaviour>();

        if (collision.gameObject.tag == "Block")  //if ball hits the end block, call game over
        {
            FindObjectOfType<GameManager>().CallGameOver();
            Debug.Log("Dead");
        }

        if (_ball != null) //if ball hits the paddle player, calculate new bounce angle
        {
                Vector3 _playerPosition = this.transform.position;
                Vector2 _contactPoint = collision.GetContact(0).point;

                float _offset = _playerPosition.x - _contactPoint.x;
                float _width = collision.otherCollider.bounds.size.x / 2;

                float _currentAngle = Vector2.SignedAngle(Vector2.up, _ball._rigidbody.velocity);
                float _bounceAngle = (_offset / _width) * this._maxBounceAngle;
                float _newAngle = Mathf.Clamp(_currentAngle + _bounceAngle, -_maxBounceAngle, _maxBounceAngle);

                _ball._rigidbody.velocity = ClampMag(_ball._rigidbody.velocity, _ball._ballSpeed, _ball._ballSpeed);

                Quaternion _rotation = Quaternion.AngleAxis(_newAngle, Vector3.forward);
                _ball._rigidbody.velocity = _rotation * Vector2.up * _ball._rigidbody.velocity.magnitude;
                        
        }
    }

    public static Vector3 ClampMag(Vector3 value, float max, float min)   //used to clamp the speed of the ball
    {
        double sm = value.sqrMagnitude;
        if(sm > (double)max * (double)max)
        {
            return value.normalized * max;
        }
        else if(sm < (double)min * (double)min)
        {
            return value.normalized * min;
        }

        return value;
    }

    public void ResetPlayer()  //Used to reset player position at the start of a level/run
    {
        transform.position = new Vector2(0f , -7.58f);

        _rigidbody.velocity = Vector2.zero;

    }

    void TouchInput()  //used to take touch inputs from a touch screen
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 _touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            _touchPos.z = 0f;

            if(_touchPos.x > 1f)
            {
                _moveVelocity = new Vector2( 1f * _moveSpeed, 0);

                _rigidbody.velocity = _moveVelocity;
            }
            else if(_touchPos.x < -1f)
            {
                _moveVelocity = new Vector2( -1f * _moveSpeed, 0);

                _rigidbody.velocity = _moveVelocity;
            }

        }
    }

    void WASDinputs()  //used to take the WASD/arrow inputs from the PC version of the game
    {
        _moveDirection = Input.GetAxisRaw("Horizontal");

        if (_moveDirection != 0)
        {
            _moveVelocity = new Vector2(_moveDirection * _moveSpeed, 0);

            _rigidbody.velocity = _moveVelocity;

        }
    }


}
