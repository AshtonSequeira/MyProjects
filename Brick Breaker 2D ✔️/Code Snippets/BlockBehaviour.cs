using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is attached to the blocks in the game level

public class BlockBehaviour : MonoBehaviour
{
    public int _health { get; private set; } //heath of the block

    public Color[] _colors;

    public int _points = 100;

    public bool _unbreakable;

    SpriteRenderer _spriteRenderer;


    private void Awake()
    {
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if(!_unbreakable)
        {
            _health = _colors.Length;

            _spriteRenderer.color = this._colors[_health - 1];
        }
    }

    private void Hit(Collision2D _collision1)
    {
        if(_unbreakable) //if unbreakable, do nothing
        {
            return;
        }

        _health--;  //else decerease health

        if(_health <= 0)  //if health is 0, disable block
        {
            gameObject.SetActive(false);
        }
        else
        {
            _spriteRenderer.color = this._colors[_health - 1];  //else, change colour
        }

        FindObjectOfType<GameManager>().Hit(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)  
    {
        if(collision.gameObject.tag == "Ball")  // if hit by the ball
        {
            Hit(collision);

        }
 
    }
}
