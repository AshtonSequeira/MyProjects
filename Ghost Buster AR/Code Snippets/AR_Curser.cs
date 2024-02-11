using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;

//Main script to start the level

public class AR_Curser : MonoBehaviour
{
    [SerializeField] GameObject _curserChildObject;
    [SerializeField] GameObject _TapToPlace;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] GameObject _crossHair;
    [SerializeField] GameObject _objectToPlace;
    GameObject _levelBlocks;
    [SerializeField] ARRaycastManager _raycastManager;
    [SerializeField] Camera _AR_camera;
    Player _player;
    [SerializeField] GameObject _youreDead;
    [SerializeField] GameObject _youWin;
    [SerializeField] GameObject _RestartLevel;

    [SerializeField] bool _useCurser = true;
    public bool _levelReady = false;

    float _distance = 0f;
    [SerializeField] TMP_Text _distanceText;
    [SerializeField] TMP_Text _meshText;

    // Start is called before the first frame update
    void Start()
    {
        _curserChildObject.SetActive(_useCurser);

        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
       if(_useCurser)  //used to place the guardian boundary/PlayArea
        {
            UpdateCurser();
       }

    }

    void UpdateCurser()
    {
        Vector2 _screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> _hits = new List<ARRaycastHit>();
        _raycastManager.Raycast(_screenPosition, _hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if(_hits.Count > 0)  //if floor is detected
        {
            _TapToPlace.SetActive(true);
            transform.position = _hits[0].pose.position;
            transform.rotation = _hits[0].pose.rotation;

            _curserChildObject.transform.position = _hits[0].pose.position;
            _curserChildObject.transform.rotation = _hits[0].pose.rotation;
        }
        else
        {
            _TapToPlace.SetActive(false);
        }

    }

    public void ResetLevelAndPlacement()  //Used to on a reset level and PlayArea button
    {
        Debug.Log("Reset Boundary button pressed");
        _RestartLevel.SetActive(false);
        _TapToPlace.SetActive(true);
        _youreDead.SetActive(false);
        _youWin.SetActive(false);
        _player._playerHealth = 100;
        Time.timeScale = 1f;
        Destroy(_levelBlocks);
        _useCurser = true;
        _curserChildObject.SetActive(true);
        _crossHair.SetActive(false);
        _levelReady = false;

    }
    public void TapToPlace()  //Used to place the guardian and Level
    {
        Debug.Log("ButtonTapped");
        _RestartLevel.SetActive(true);

        if (!_levelReady)
        {
            if (_useCurser)
            {
                _levelBlocks = GameObject.Instantiate(_objectToPlace, _spawnPoint.position, _spawnPoint.rotation);
                _useCurser = false;
                _levelReady = true;
                StartCoroutine(WaitBefore());
                _TapToPlace.SetActive(false);
            }
        }
    }

    public void RestartLevel()   //Used to restart the level
    {
        Debug.Log("Restart button pressed");

        _player._playerHealth = 100;
        _player._timer = 5.5f;
        Time.timeScale = 1f;
        Destroy(_levelBlocks);

        _levelBlocks = GameObject.Instantiate(_objectToPlace, _spawnPoint.position, _spawnPoint.rotation);
        _levelReady = true;
        _crossHair.SetActive(true);
        _youreDead.SetActive(false);
        _youWin.SetActive(false);

    }
    IEnumerator WaitBefore()
    {
        yield return new WaitForSeconds(0.5f);

        _crossHair.SetActive(true);
        _levelReady = true;

    }

}
