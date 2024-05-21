using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] float _timeToDestroy = 3f;

    private void OnEnable()  //destory the ball in 3s after being shot
    {
        Destroy(gameObject, _timeToDestroy);
    }

}

