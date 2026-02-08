using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private AudioClip pickUpSound;

    private void Start()
    {
        gameManager = GameManager.FindAnyObjectByType(typeof(GameManager)) as GameManager;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.PlaySound(pickUpSound);
            gameManager.GainCoin();
            Destroy(gameObject);
        }
    }
}