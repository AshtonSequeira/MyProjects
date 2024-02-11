using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Controls the Health of the enemy

public class Enemy_Health : MonoBehaviour
{
    public float _enemyHealth = 100f;
    public Animator _animator;
    NavMeshAgent _agent;

    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemyHealth <= 0f)
        {
            _animator.SetTrigger("Dead");
            _agent.SetDestination(_agent.transform.position);
            _collider.enabled = false;

            Destroy(gameObject, 5f);

        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            _enemyHealth -= 25f;
            Debug.Log("Enemy took Damage");
        }
    }
}
