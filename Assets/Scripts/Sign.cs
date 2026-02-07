using TMPro;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private Player player;
    public TMP_Text text;
    private float distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDistance();
    }

    void PlayerDistance()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (distance <= 1)
        {
            text.gameObject.SetActive(true);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}