using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Control the Health of the player

public class Player_Health : MonoBehaviour
{
    public float _playerHealth = 100f;
    [SerializeField] GameObject _shot_canvas;
    [SerializeField] TMP_Text _healthText;
    [SerializeField] PlayerController _playerController;
    [SerializeField] CameraSwitcher _cameraSwitcher;

    // Start is called before the first frame update
    void Start()
    {
        _healthText.text = _playerHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerHealth <= 0f)
        {
            Debug.Log("Player Dead");
            _playerController.enabled = false;
            _cameraSwitcher.enabled = false;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerHealth > 0f)
        {
            if (other.gameObject.tag == "knife" || other.gameObject.tag == "EnemyBullet")
            {
                StartCoroutine(DamagePlayer());
            }
        }
    }

    private IEnumerator DamagePlayer()
    {
        _shot_canvas.SetActive(true);

        // Add delay here (for example, 1 second)
        yield return new WaitForSeconds(0.2f);

        _shot_canvas.SetActive(false);
        _playerHealth -= 10f;
        _healthText.text = _playerHealth.ToString();
        Debug.Log("Player took Damage");
    }
}
