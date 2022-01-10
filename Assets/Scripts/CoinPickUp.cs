using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinValue = 100;
    GameSession gameSession;
    bool wasCollected = false;

    private void Awake() 
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            gameSession.AddToScore(coinValue);

            AudioSource.PlayClipAtPoint(coinPickupSFX, 
                Camera.main.transform.position);
            
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
