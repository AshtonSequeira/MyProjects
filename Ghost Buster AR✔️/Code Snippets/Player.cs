using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//This script is attached to the AR player

public class Player : MonoBehaviour
{
    public float _playerHealth = 100f;
    [SerializeField] bool _inPlayArea = false;
    [SerializeField] AR_Curser _arCurser;
    [SerializeField] TMP_Text _warningText;
    [SerializeField] TMP_Text _healthText;
    [SerializeField] GameObject _youreDead;
    [SerializeField] GameObject _warning;
    [SerializeField] Image _bloodOverlay;
    Color _bloodOverlayAlpha1 = new Color(1f, 1f, 1f, 1f);
    Color _bloodOverlayAlpha0 = new Color(1f, 1f, 1f, 0f);
    public float _timer = 5.5f;

    // Update is called once per frame
    void Update()
    {
        if (_playerHealth < 1)
        {
            YoureDead();
        }

        if (_arCurser._levelReady) //if level is ready
        {
            if (!_inPlayArea)//if player continues to stay outside the playArea, Start giving warning sign for 5s
            {
                _warningText.text = "Enter Play Area\r\n in " + _timer.ToString("F2") + "s!";
                _timer -= Time.deltaTime / 0.1f;
            }
            else if (_timer < 5.5f)
            {
                _timer = 5.5f;
            }
        }

        _healthText.text = "Health: " + _playerHealth; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Zombie") //if Zombie/Ghost hits the player, decrease player health
        {
            _playerHealth--;

            StartCoroutine(ChangeHealthColour());           
            
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Guardian")  //if player is in the PlayArea
        {
            _inPlayArea = true;

            if (_arCurser._levelReady)
            {
                ResumeGame();
            }
                
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Guardian")   //if player is out of the PlayArea
        {
            _inPlayArea = false;

            if (_arCurser._levelReady)
            {
                PauseGame();
            }

        }
    }

    void PauseGame()   //Show warning and slow down time
    {
        _warning.SetActive(true);
        StartCoroutine(WaitBefore());
        Time.timeScale = 0.1f;
    }

    void ResumeGame()    //Stop showing warning and normalise down time
    {
        _warning.SetActive(false);
        StopAllCoroutines();
        Time.timeScale = 1f;
    }
    void YoureDead()   //Game Over
    {
        _playerHealth = 0;
        StopAllCoroutines();
        _youreDead.SetActive(true);
        Time.timeScale = 0;
        _arCurser._levelReady = false;

        Debug.Log("Youre Dead!");

    }

    IEnumerator ChangeHealthColour()   //Visual and haptic feedback when player health is decreased
    {
        _healthText.color = Color.red;

        _bloodOverlay.color = _bloodOverlayAlpha1;

        Handheld.Vibrate();

        yield return new WaitForSeconds(0.3f);

        Handheld.Vibrate();

        _bloodOverlay.color = _bloodOverlayAlpha0;

        _healthText.color = Color.white;
    }

    IEnumerator WaitBefore()   //5s warning before game over if player is out of the playArea
    {
        yield return new WaitForSeconds(0.5f);

        if (!_inPlayArea && _arCurser._levelReady)
        {
            _warning.SetActive(false);
            YoureDead();
        }

    }
}
