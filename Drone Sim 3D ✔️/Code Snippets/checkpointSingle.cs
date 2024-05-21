using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attached to every checkpoint so that its arrow points to the next checkpoint

public class checkpointSingle : MonoBehaviour
{
    [SerializeField] GameObject _next;

    [SerializeField] GameObject _arrow;

    [SerializeField] arrowPointer _arrowPointer;

    private void Awake()
    {
        _arrowPointer = gameObject.GetComponentInChildren<arrowPointer>();

        _arrow = _arrowPointer.gameObject;

        Debug.Log(_arrow);
    }

    public void SetObjectActive(bool _isActive)
    {
        Transform _childOutTransform = this.transform.Find("out");

        Transform _childInTransform = this.transform.Find("in");

        Transform _childArrowTransform = this.transform.Find("arrow1");

        if (_childOutTransform)
        {
            GameObject _gameObject = _childOutTransform.gameObject;
            _gameObject.SetActive(_isActive);
        }
        else
        {
            Debug.LogWarning("Child Out Object Not Found under Parent");
        }

        if(_childInTransform)
        {
            GameObject gameObject = _childInTransform.gameObject;
            gameObject.SetActive(_isActive);
        }
        else
        {
            Debug.LogWarning("Child In Object Not Found under Parent");
        }

        if (_childArrowTransform)
        {
            MeshRenderer _meshRenderer = _childArrowTransform.GetComponent<MeshRenderer>();

            _meshRenderer.enabled = _isActive;
        }
        else
        {
            Debug.LogWarning("Child Arrow Object Not Found under Parent");
        }

    }

    public void SetNext(checkpointSingle _nextCheckpoint)
    {
        _next = _nextCheckpoint.gameObject;

        _arrow.transform.LookAt(_next.transform);
    }

}
