using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneName;
    public LevelLoader loader;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            loader.LoadNextLevel(sceneName);
            collision.GetComponent<Player>().StopMoving();
            collision.GetComponent<Player>().canMove = false;
        }
    }
}