using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the zombie/ghost in the game

public class ZombieController : MonoBehaviour
{
    Transform _player;

    Rigidbody _rb;

    float _zombieSpeed = 15f;

    [SerializeField] float _health = 10f;

    [SerializeField] float _minSpeed = 15f, _maxSpeed = 20f;
    
    Color _zombieOriginalColor = new Color(0.02182829f, 1f, 0f);

    Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            // Get the material(s) attached to the Renderer.
            materials = renderer.materials;

        }
        else
        {
            Debug.LogError("No Renderer component found on this GameObject.");
        }

        _zombieSpeed = Random.Range(_minSpeed, _maxSpeed);

        _rb = GetComponent<Rigidbody>();

        _player = GameObject.Find("Main Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(_player);   //Look at the player
                
        _rb.AddForce(transform.forward * _zombieSpeed, ForceMode.Force);  //Move towards the player

        if(_health < 1)  //if zombie/ghost health is less than 0, destroy zombie/ghost
        {
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")  //if bullet hits zombie/ghost, deceased health and change colour
        {
            _health--;

            StartCoroutine(ChangeZombieColour());
        }

        if(collision.gameObject.tag == "Player")   //if zombie/ghost hits player, move away from the player for a second attack
        {
            _rb.AddForce(-transform.forward * _zombieSpeed * 2, ForceMode.Impulse);
        }

    }
    IEnumerator ChangeZombieColour()  // change zombie/ghost colour when hit by a bullet
    {
        foreach (Material material in materials)
        {
            material.color = Color.white;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Material material in materials)
        {
            material.color = _zombieOriginalColor;

        }
    }
}
