using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is used for level 6

public class GoDown : MonoBehaviour
{
    float _timer = 7f;

    float _gap = 0.5f;

    Vector2 _position;

    [SerializeField]GameObject _bricks;

    // Start is called before the first frame update
    void Start()
    {
        _position = _bricks.transform.position;

        StartCoroutine(ComeDown()); //call the ComeDown function
    }

    IEnumerator ComeDown() //this function gets all the bricks down every 7s buy changing the position of the bricks
    {
        _position.y -= _gap;

        yield return new WaitForSeconds(_timer);

        _bricks.transform.position = _position;

        StartCoroutine(ComeDown());
        
    }
}
