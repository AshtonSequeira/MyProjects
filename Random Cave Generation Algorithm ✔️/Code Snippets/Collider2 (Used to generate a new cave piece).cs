using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider2 : MonoBehaviour
{
    [SerializeField] GameObject[] _mapPrefab;

    [SerializeField] GameObject _child;

    // Start is called before the first frame update
    void Start() 
    {
        //check if some gameobjects are inactive, then set them active

        if (gameObject.GetComponent<Collider>().enabled == false)
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }

        if (gameObject.GetComponent<MeshRenderer>().enabled == false)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        if (_child.GetComponent<Collider>().enabled == false)
        {
            _child.GetComponent<Collider>().enabled = true;
        }

        if (_child.GetComponent<MeshRenderer>().enabled == false)
        {
            _child.GetComponent<MeshRenderer>().enabled = true;
        }
                
        InstantiateNewMap(); //Call function to generate a new cave piece

    }

    void InstantiateNewMap()
    {
        StartCoroutine(WaitBefore()); // call the timer function
    }

    IEnumerator WaitBefore()
    {
        yield return new WaitForSeconds(0.5f);  //this causes the nice 0.5s interval between new cave piece generation

        int a = Random.Range(0, _mapPrefab.Length);
        GameObject _go = Instantiate(_mapPrefab[a], transform.position + transform.forward * 5.5f, transform.rotation);  //Generate a new cave piece

        foreach (Transform _chld in _go.transform)
        {
            _chld.gameObject.SetActive(true);  //set active all the child objects
        }

        //disable the collider component of this game object and child game objects so it does not interfere with the algorithm

        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;

        _child.GetComponent<Collider>().enabled = false;
        _child.GetComponent<MeshRenderer>().enabled = false;

    }
}
