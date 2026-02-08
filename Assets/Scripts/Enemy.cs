using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;
    public float speed;
    public Transform ledgeDetector;
    public LayerMask groundLayer;
    public float raycastDistance;
    public float wallDistance;
    private BoxCollider2D boxCollider;

    private bool facingRight = true;
    private Vector2 forwards;
    private bool canMove = true;
    public BoxCollider2D playerCollision;
    public BoxCollider2D weakpoint;

    [SerializeField] private AudioClip deathSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canMove)
        {
            Movement();
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            body.linearVelocity = new Vector2(speed, body.linearVelocity.y);
        }

    }

    private void Movement()
    {
        if (facingRight)
        {
            forwards = Vector2.right;
        }
        else
        {
            forwards = Vector2.left;
        }
        RaycastHit2D hit = Physics2D.Raycast(ledgeDetector.position, Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D hitWall = Physics2D.Raycast(ledgeDetector.position, forwards, wallDistance, groundLayer);

        if (hit.collider == null || hitWall == true)
        {
            //Debug.Log("Dude rotate");
            Rotate();
        }
    }

    void Rotate()
    {
        transform.Rotate(0, 180, 0);
        speed = -speed;

        if (facingRight)
        {
            facingRight = false;
        }
        else
        {
            facingRight = true;
        }
    }

    public void Death()
    {
        SoundManager.instance.PlaySound(deathSound);
        canMove = false;
        body.bodyType = RigidbodyType2D.Static;
        body.gravityScale = 0;
        boxCollider.enabled = false;
        weakpoint.enabled = false;
        playerCollision.enabled = false;
        anim.SetTrigger("dead");
        StartCoroutine(waiterDeath(1f));
    }

    IEnumerator waiterDeath(float duration)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        Color startColor = renderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            renderer.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
