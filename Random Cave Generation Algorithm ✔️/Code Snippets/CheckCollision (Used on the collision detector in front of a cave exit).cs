using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour //this script
{
    [SerializeField] GameObject _parent;
    [SerializeField] GameObject _endPiece;
   
    private void OnTriggerEnter(Collider other)
    {
        Transform _mapObjects = GameObject.Find("Map Objects").GetComponent<Transform>();

        if(other.gameObject.tag == "DesCollider" || other.gameObject.tag == "Cave")  //if collision dectector has detected collision between another collision detector or other cave object.
        {
            Instantiate(_endPiece, _parent.transform.position - _parent.transform.forward, _parent.transform.rotation, _mapObjects);  //if yes then block the exit
            _parent.SetActive(false);
           
        }
    }

}
