using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Used to control a long range patrolling enemy AI

public class EnemyAI_Patrolling : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent; //only done to show sphere in the scene view
    Transform _player;

    [SerializeField] float _enemyHealth = 100f;
    [SerializeField] float _sightRange = 10f;
    [SerializeField] float _agentStoppingDistance = 20f;
    [SerializeField] float _timeBetweenAttacks = 2f;
    [SerializeField] float _lookoutTime = 3f;
    [SerializeField] float _lookAroundTime = 0.01f;
    [SerializeField] float _viewAngle;
    [SerializeField] LayerMask _obstacleMask;

    [SerializeField] bool _isPatrolling;
    [SerializeField] Transform[] _transPatrollingPoints;
    int _newIntPatrolPos;
    int _oldIntPatrolPos = 100;
    Vector3 _patrolDestination;
    float _patrolPtDistance;

    bool _changePos = false;
    bool _lookingAround = false;
    bool _lookingAroundTurn = false;
    [SerializeField] float _lookAroundAngle = 20f;
    int _looKAroundIterator = 2;

    //Gun Variables
    [SerializeField] GameObject _enemyBulletPrefab;
    [SerializeField] Transform _enemyBarrelTransform;
    [SerializeField] Transform _bulletPrefabParent;

    float _playerEnemyDistance;
    [SerializeField] GameObject _lastKnownPosGO;
    Vector3 _lastKnownPlayerPosition = Vector3.zero;
    Quaternion _targetLookAroundAngle = Quaternion.Euler(0f, 0f, 0f);

    bool _alreadyAttacked = false;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _agent.stoppingDistance = 0f;

        RandomPatrolling();

    }

    // Update is called once per frame
    void Update()
    {
        if (_isPatrolling )
        {
            _patrolPtDistance = Vector3.Distance(transform.position, _transPatrollingPoints[_newIntPatrolPos].position);

            if (_patrolPtDistance < 0.2f && !_changePos && _isPatrolling)
            {
                _changePos = true;

                LookAround();

            }
        }

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
                else if(!_isPatrolling)//not visble but in detection range
                { 
                    //Hunt For the Player                    

                    _agent.stoppingDistance = 0f;

                    _agent.SetDestination(_lastKnownPlayerPosition);//players last known position                    

                    Debug.Log("Hunting 1");

                    Debug.DrawRay(transform.position, _dirToPlayer * _playerEnemyDistance, Color.green);

                }

            }
            else if (!_isPatrolling) //if player is not in viewing cone but in detection range
            { 
                //Hunt For the Player               

                _agent.stoppingDistance = 0f;

                _agent.SetDestination(_lastKnownPlayerPosition);//players last known position                    

                Debug.Log("Hunting 2");

                Debug.DrawRay(transform.position, _dirToPlayer * _playerEnemyDistance, Color.green);

            }
        }
        else
        {
            _agent.stoppingDistance = 0f;
        }

        if (Vector3.Distance(_lastKnownPlayerPosition, transform.position) < 0.6f && !_isPatrolling) //this statement can be optimized
        {
            _agent.stoppingDistance = 0f;

            if (!_lookingAround )
            {
                LookAround();
                Debug.Log("calling 1");
            }           

        }

        if (_lookingAroundTurn)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, _targetLookAroundAngle, _lookAroundTime);

            Debug.Log("Looking Around");
            

            if(transform.eulerAngles.y > (_targetLookAroundAngle.eulerAngles.y - 1f) && _looKAroundIterator == 2)
            {
                Debug.Log("Entered");
                
                Vector3 _tempAngle = Vector3.zero;

                _tempAngle.y = transform.eulerAngles.y - 2*_lookAroundAngle;

                _targetLookAroundAngle = Quaternion.Euler(0f, _tempAngle.y, 0f);

                _looKAroundIterator--;

            }

            if (transform.eulerAngles.y < (_targetLookAroundAngle.eulerAngles.y + 1f) && _looKAroundIterator == 1)
            {
                RandomPatrolling();
                Debug.Log("Called Random Patrolling");
            }
        }
    }

    void RandomPatrolling()  //Random Patrolling script
    {

        _isPatrolling = true;
        _agent.stoppingDistance = 0f;
        _newIntPatrolPos = Random.Range(0, _transPatrollingPoints.Length);
        if(_newIntPatrolPos == _oldIntPatrolPos)
        {
            RandomPatrolling();
        }
        else
        {
            _oldIntPatrolPos = _newIntPatrolPos;

            _agent.destination = _transPatrollingPoints[_newIntPatrolPos].position;

            _changePos = false;

            _lookingAroundTurn = false;

            _lookingAround = false;
        }        
    }

    void AttackPlayer()  //Attack the player code
    {
        _agent.SetDestination(transform.position);

        if (!_alreadyAttacked)
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

    void FaceTarget()  //Facing the player
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

    void LookAround()  //Looking around during patrolling and hunting
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
