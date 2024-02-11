using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

//This Script is used to control the ball

public class BallBehaviour : MonoBehaviour
{
    public Rigidbody2D _rigidbody;

    GameManager _gm;

    public float _ballSpeed = 20f;

    Vector3 _lastVelocity , _direction;

    [SerializeField] private AudioSource _bounce;

    [SerializeField] private AudioSource _death;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();

        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        ResetBall();  //Reset the ball at the start position at the start of the level
    }

    IEnumerator WaitBefore() //Wait for 1s before starting the level
    {
        Vector2 _force = Vector2.zero;
        _force.x = Random.Range(-1f, 1f);    //Add force in a random x direction on every run of the level
        _force.y = 1f;

        yield return new WaitForSeconds(1f);
                
        _rigidbody.velocity = _force.normalized * _ballSpeed;

    }

    public void ResetBall()  //Ball's standard position 
    {
        transform.position = new Vector2( 0f , -5.75f);
        _rigidbody.velocity = Vector2.zero;

        StartCoroutine(WaitBefore()); 
    }

    private void OnCollisionEnter2D(Collision2D collision)  //when ball hits a gameObject
    {
        if (MusicSFXcontainer._sfxOn)
        {
            if (collision.gameObject.tag == "Finish")  //if it hits the bottom line, decrease 1 life and play death sound
            {
                if (_gm._lives > 1)
                    _death.Play();
            }
            else _bounce.Play();    //else play bounce sound
        }
    }

}
