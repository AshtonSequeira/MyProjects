using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//used to check if all the zombies/ghosts are killed

public class CheckIfCleared : MonoBehaviour
{
    GameObject[] objectsWithTag;
    [SerializeField] AR_Curser _arCurser;

    public bool _levelCleared = false;

    private void Start()
    {
        _arCurser = GameObject.Find("AR Curser").GetComponent<AR_Curser>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!_levelCleared)
        {
            Cleared();
        }
        else
        {
            Time.timeScale = 0;
           _arCurser._levelReady = false;
        }

    }

    void Cleared() //checks if zombies/ghosts are active in the scene
    {
        objectsWithTag = GameObject.FindGameObjectsWithTag("Zombie");

        if (objectsWithTag.Length == 0)
        {
            _levelCleared = true;
        }
                
    }
}
