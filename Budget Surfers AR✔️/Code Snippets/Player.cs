using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This script is attached to the AR player

public class Player : MonoBehaviour
{
    public float _playerHealth = 100f;
    [SerializeField] bool _inPlayArea = false;
    [SerializeField] AR_Curser _arCurser;
    [SerializeField] TMP_Text _warningText;
    [SerializeField] GameObject _youreDead;
    [SerializeField] GameObject _warning;
    public float _timer = 5.5f;

    // Update is called once per frame
    void Update()
    {
        if(_arCurser._levelReady) //if level is ready
        {
            if (!_inPlayArea) //if player outside the playArea then pause the game
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }

            if (!_inPlayArea)  //if player continues to stay outside the playArea, Start giving warning sign for 5s
            {
                _warningText.text = "Enter Play Area\r\n in " + _timer.ToString("F2") + "s!";
                _timer -= Time.deltaTime / 0.1f;
            }
            else if (_timer < 5.5f)
            {
                _timer = 5.5f;
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BScube") //if player hits a Cube, Game Over
        {
            YoureDead();
        }

        if (other.gameObject.tag == "Guardian")   //if player is in the PlayArea
        {
            _inPlayArea = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Guardian")   //if player is out of the PlayArea
        {
            _inPlayArea = false;
        }
    }

    void PauseGame()  //Show warning and slow down time
    {
        _warning.SetActive(true);
        StartCoroutine(WaitBefore());
        Time.timeScale = 0.1f;
    }

    void ResumeGame()   //Stop showing warning and normalise down time
    {
        _warning.SetActive(false);
        StopAllCoroutines();
        Time.timeScale = 1f;
    }

    void YoureDead()    //Game Over
    {
        StopAllCoroutines();
        _youreDead.SetActive(true);
        Time.timeScale = 0;
        _arCurser._levelReady = false;

        Debug.Log("Youre Dead!");

    }

    IEnumerator WaitBefore()    //5s warning before game over if player is out of the playArea
    {
        yield return new WaitForSeconds(0.5f);

        if (!_inPlayArea && _arCurser._levelReady)
        {
            _warning.SetActive(false);
            YoureDead();
        }

    }

}
