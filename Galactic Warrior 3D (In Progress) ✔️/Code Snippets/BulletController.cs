using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//Used to control the bullet and create a bullet hole

public class BulletController : MonoBehaviour
{
    [SerializeField] float _bulletspeed = 100f;
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] float _timeToDestroy = 3f;
    public Vector3 _target;
    public bool _hit;

    private void OnEnable()
    {
        Destroy(gameObject, _timeToDestroy);        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, _bulletspeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag !="enemy")
        { GameObject bulletHole = Instantiate(bulletHolePrefab, transform.position, Quaternion.identity);
          bulletHole.transform.rotation = Quaternion.FromToRotation(Vector3.up, collision.contacts[0].normal);
          bulletHole.transform.SetParent(collision.transform);

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Enemy hit");
            Destroy(gameObject);
        }
    }
  

}
