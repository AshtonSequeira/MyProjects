using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Used to control long ranged enemy

public class EnemyAI : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent; //only done to show sphere in the scene view
    Transform _player;

    [SerializeField] float _enemyHealth = 100f;
    [SerializeField] float _sightRange = 10f;
    [SerializeField] float _agentStoppingDistance = 20f;
    [SerializeField] float _timeBetweenAttacks = 2f;
    [SerializeField] float _viewAngle;
    [SerializeField] LayerMask _obstacleMask;

    //Gun Variables
    [SerializeField] GameObject _enemyBulletPrefab;
    [SerializeField] Transform _enemyBarrelTransform;
    [SerializeField] Transform _bulletPrefabParent;

    float _playerEnemyDistance;
    [SerializeField] GameObject _lastKnownPosGO;
    Vector3 _lastKnownPlayerPosition = Vector3.zero;

    bool _alreadyAttacked = false;
    bool _findPlayer = false;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_findPlayer);

        _lastKnownPosGO.transform.position = _lastKnownPlayerPosition;

        _playerEnemyDistance = Vector3.Distance(transform.position, _player.position);

        if (_playerEnemyDistance < _sightRange)
        {
            Vector3 _dirToPlayer = (_player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, _dirToPlayer) < _viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, _dirToPlayer, _playerEnemyDistance, _obstacleMask)) //if playler is visible
                {
                    Debug.DrawRay(transform.position, _dirToPlayer * _playerEnemyDistance, Color.red);
                    _agent.stoppingDistance = _agentStoppingDistance;
                    _agent.SetDestination(_player.position);

                    _findPlayer = false;

                    _lastKnownPlayerPosition = _player.position;

                    if (_playerEnemyDistance < _agent.stoppingDistance)
                    {
                        AttackPlayer();

                        FaceTarget();
                    }

                }
                else
                {
                    //Look Around
                    _agent.stoppingDistance = 0f;

                    _agent.SetDestination(_lastKnownPlayerPosition);//players last known position

                    _findPlayer = true;

                    Debug.DrawRay(transform.position, _dirToPlayer * _playerEnemyDistance, Color.green);

                }

            }
        }
        else
        {
            _agent.stoppingDistance = 0f;
        }

        if (Vector3.Distance(_lastKnownPlayerPosition, transform.position) < 0.3f && _findPlayer)
        {
            transform.RotateAround(transform.position, transform.up, -30f);//look around function
            Debug.Log("Rotated");

            _findPlayer = false;

            //If Player Not Found go back to patrolling
        }
    }

    void AttackPlayer()
    {
        _agent.SetDestination(transform.position);

        if( !_alreadyAttacked )
        {
            //Attack Code

            Rigidbody _bullet = Instantiate(_enemyBulletPrefab, _enemyBarrelTransform.position, _enemyBarrelTransform.rotation, _bulletPrefabParent).GetComponent<Rigidbody>();
            
            BulletController _bulletController = _bullet.GetComponent<BulletController>();
            _bulletController._target = _player;

            _alreadyAttacked = true;

            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }

    }

    void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    void FaceTarget()
    {
        Vector3 _direction = (_player.position - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(new Vector3(_direction.x, 0, _direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(int _damage)
    {
        _enemyHealth -= _damage;

        if (_enemyHealth < 0)
        {
            DestroyEnemy(); //You can also use Invoke to add Delay
        }
    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()  //Visualise the enemies attack and look radius
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _agent.stoppingDistance);
    }
}
