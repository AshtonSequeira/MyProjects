using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Used to control the boss AI

public class BossAI2 : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent; //only done to show sphere in the scene view
    Transform _player;

    [SerializeField] float _enemyHealth = 100f;
    [SerializeField] float _sightRange = 10f;
    [SerializeField] float _agentStoppingDistance = 20f;
    [SerializeField] float _timeBetweenAttacks = 2f;
    [SerializeField] float _viewAngle;
    [SerializeField] float _lookAroundTime = 0.01f;
    [SerializeField] LayerMask _obstacleMask;
    [SerializeField] bool _isPatrolling;
    bool _lookingAround = false;
    bool _lookingAroundTurn = false;
    [SerializeField] float _lookAroundAngle = 20f;
    int _looKAroundIterator = 2;

    //Spear Variables
    [SerializeField] GameObject _spear;

    float _playerEnemyDistance;
    [SerializeField] GameObject _lastKnownPosGO;
    Vector3 _lastKnownPlayerPosition = Vector3.zero;
    Quaternion _targetLookAroundAngle = Quaternion.Euler(0f, 0f, 0f);
    [SerializeField] Vector3 _homeLocation = Vector3.zero;

    bool _alreadyAttacked1 = false;

    [SerializeField] float _enemyAndLastknownposOffset = 0.6f;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = 0f;
        _isPatrolling = true;
    }

    // Update is called once per frame
    void Update()
    {
       _lastKnownPosGO.transform.position = _lastKnownPlayerPosition;

        _playerEnemyDistance = Vector3.Distance(transform.position, _player.position);

        if (_playerEnemyDistance < _sightRange)
        {
            Vector3 _dirToPlayer = (_player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, _dirToPlayer) < _viewAngle / 2) // if player is in viewing cone
            {
                if (!Physics.Raycast(transform.position, _dirToPlayer, _playerEnemyDistance, _obstacleMask)) //if playler is visible
                {
                    _isPatrolling = false;

                    _lookingAroundTurn = false;

                    _lookingAround = false;

                    Debug.DrawRay(transform.position, _dirToPlayer * _playerEnemyDistance, Color.red); //Draw line of sight

                    _agent.stoppingDistance = _agentStoppingDistance;

                    _agent.SetDestination(_player.position);

                    _lastKnownPlayerPosition = _player.position;

                    if (_playerEnemyDistance < _agent.stoppingDistance)
                    {
                        AttackPlayer();

                        FaceTarget();
                    }

                }
                else if (!_isPatrolling)//not visble but in detection range
                {   //Hunt For the Player

                    _agent.stoppingDistance = 0f;

                    _agent.SetDestination(_lastKnownPlayerPosition);//players last known position 

                    Debug.DrawRay(transform.position, _dirToPlayer * _playerEnemyDistance, Color.green);

                }

            }
            else if (!_isPatrolling) //if player is not in viewing cone but in detection range
            {   //Hunt For the Player

                _agent.stoppingDistance = 0f;

                _agent.SetDestination(_lastKnownPlayerPosition);//players last known position 

                Debug.DrawRay(transform.position, _dirToPlayer * _playerEnemyDistance, Color.green);

            }
        }
        else
        {
            _agent.stoppingDistance = 0f;
        }

        if (Vector3.Distance(_lastKnownPlayerPosition, transform.position) < _enemyAndLastknownposOffset && !_isPatrolling) //this statement can be optimized
        {
            _agent.stoppingDistance = 0f;
            Debug.Log("calling 2");

            if (!_lookingAround)
            {
                LookAround();
                Debug.Log("calling 1");
            }

        }

        if (_lookingAroundTurn)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _targetLookAroundAngle, _lookAroundTime);

            if (transform.eulerAngles.y > (_targetLookAroundAngle.eulerAngles.y - 1f) && _looKAroundIterator == 2)
            {
                Debug.Log("Entered");

                Vector3 _tempAngle = Vector3.zero;

                _tempAngle.y = transform.eulerAngles.y - 2 * _lookAroundAngle;

                _targetLookAroundAngle = Quaternion.Euler(0f, _tempAngle.y, 0f);

                _looKAroundIterator--;

            }

            if (transform.eulerAngles.y < (_targetLookAroundAngle.eulerAngles.y + 1f) && _looKAroundIterator == 1 && !_isPatrolling)
            {
                _isPatrolling = true;

                _agent.SetDestination(_homeLocation);

                Debug.Log("Go Back Home Loc");
            }
        }
    }


    void AttackPlayer()
    {
        //types of attacks
        //smash
        //cosecutive punches

        _agent.SetDestination(transform.position); //stay in position

        if (!_alreadyAttacked1)
        {
            //Attack Code

            _spear.transform.position += _spear.transform.forward * 2;

            _alreadyAttacked1 = true;

            Invoke(nameof(Attack2), _timeBetweenAttacks);

            Debug.Log("Attack 1");
        }

    }

    void ResetAttack()
    {
        _spear.transform.position -= _spear.transform.up * 2;
        _alreadyAttacked1 = false;

        Debug.Log("Reset Attack");
    }

    void Attack2()  //Enemies 2nd attack
    {
        _spear.transform.position -= _spear.transform.forward * 2;

        _spear.transform.position += _spear.transform.up * 2;

        Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        
        Debug.Log("Attack 2");
    }

    void FaceTarget()   //used to face the target
    {
        Vector3 _direction = (_player.position - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(new Vector3(_direction.x, 0, _direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 5f);
    }

    public void TakeDamage(int _damage)
    {
        _enemyHealth -= _damage;

        //reset attack

        if (_enemyHealth < 0)
        {
            DestroyEnemy(); //You can also use Invoke to add Delay
        }

    }

    void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void LookAround()
    {
        _looKAroundIterator = 2;

        _lookingAround = true;

        Vector3 _tempAngle = Vector3.zero;

        _tempAngle.y = transform.eulerAngles.y + _lookAroundAngle;

        _targetLookAroundAngle = Quaternion.Euler(0f, _tempAngle.y, 0f);

        _lookingAroundTurn = true;       
    }

    private void OnDrawGizmosSelected()  //Visualise the enemies attack and look radius
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _agent.stoppingDistance);
    }
}
