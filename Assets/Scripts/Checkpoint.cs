using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private GameManager gameManager;
    private Player player;
    private float distance;
    private bool active = false;
    public Animator anim;
    [SerializeField] private AudioClip powerUp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.FindAnyObjectByType(typeof(GameManager)) as GameManager;
        player = Player.FindAnyObjectByType<Player>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDistance();
        anim.SetBool("active", active);
    }

    void PlayerDistance()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (distance <= 0.5f && active == false)
        {
            active = true;
            gameManager.activeCheckpoint = gameObject.GetComponent<Checkpoint>();
            SoundManager.instance.PlaySound(powerUp);
        }
    }
}
