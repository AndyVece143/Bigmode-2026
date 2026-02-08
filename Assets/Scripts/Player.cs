using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Player : MonoBehaviour
{
    public float jumpForce;
    public float speed;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask groundLayer;

    public bool canMove = true;
    public bool grounded;
    private Vector2 movement;

    public Animator anim;

    private bool inKnockback;
    [SerializeField] private float knockbackTimer;
    [SerializeField] private float knockbackSpeed;

    private GameManager gameManager;
    public Vector2 initialRespawnPosition;

    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip deathSound;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        gameManager = GameManager.FindAnyObjectByType(typeof(GameManager)) as GameManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Movement();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            body.AddForce(new Vector2(horizontalInput * speed * 1, 0f), ForceMode2D.Force);
            body.linearVelocity = new Vector2(Mathf.Clamp(body.linearVelocity.x, -speed, speed), body.linearVelocity.y);
        }
    }

    private void Movement()
    {
        if (inKnockback)
        {
            boxCollider.enabled = false;
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                inKnockback = false;
                anim.SetBool("hurt", false);
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                if (IsFacingRight())
                {
                    body.linearVelocity = new Vector2(knockbackSpeed, 0);
                }
                else
                {
                    body.linearVelocity = new Vector2(-knockbackSpeed, 0);
                }
            }
        }
        else
        {
            if (canMove)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                //movement.x = horizontalInput * speed * Time.deltaTime;
                //transform.Translate(movement);
                //body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
                boxCollider.enabled = true;

                //Flip Sprite
                if (horizontalInput > 0.01f)
                {
                    transform.localScale = Vector3.one;
                }

                else if (horizontalInput < -0.01f)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                if (Input.GetKey(KeyCode.Space) && isGrounded())
                {
                    Debug.Log("Jump");
                    Jump();
                }

                //if (Input.GetKey(KeyCode.K))
                //{
                //    StartKnockback();
                //}

                //if (Input.GetKey(KeyCode.Y))
                //{
                //    Death();
                //}
                    

                grounded = isGrounded();
                //Animation
                anim.SetBool("grounded", grounded);
                anim.SetBool("move", horizontalInput != 0f);
            }
        }
        anim.SetBool("hurt", inKnockback);

    }

    public void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        grounded = false;
        SoundManager.instance.PlaySound(jumpSound);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public void StopMoving()
    {
        body.linearVelocity = new Vector2(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Danger")
        {
            canMove = false;
            body.linearVelocity = new Vector2(0, 0);
            SoundManager.instance.PlaySound(deathSound);
            Death();
            //Respawn();
        }

        if (collision.collider.tag == "Enemy")
        {
            SoundManager.instance.PlaySound(deathSound);
            StartKnockback();
        }
    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Danger")
    //    {
    //        canMove = false;
    //        body.linearVelocity = new Vector2(0, 0);
    //        Death();
    //        //Respawn();
    //    }
    //}

    public void StartKnockback()
    {
        inKnockback = true;
        knockbackTimer = 0.3f;
    }

    private bool IsFacingRight()
    {
        if (transform.localScale.x == -1)
        {
            return true;
        }
        return false;
    }

    private void Death()
    {
        body.linearVelocity = new Vector2(0, 0);
        canMove = false;
        body.gravityScale = 0;
        boxCollider.enabled = false;
        anim.SetBool("dead", true);
        StartCoroutine(waiterDeath(1f));
    }

    private void Respawn(SpriteRenderer renderer, Color color)
    {
        anim.SetBool("dead", false);
        renderer.color = new Color32(255, 255, 255, 255);
        boxCollider.enabled = true;

        if (gameManager.activeCheckpoint == null)
        {
            body.transform.position = initialRespawnPosition;
        }
        else
        {
            Vector2 respawnPoint = gameManager.activeCheckpoint.transform.position;
            body.transform.position = respawnPoint;
        }
        canMove = true;
        body.gravityScale = 1;
    }

    IEnumerator waiterDeath(float duration)
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        Color startColor = renderer.color;
        //Debug.Log(startColor);
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            renderer.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }
        Respawn(renderer, startColor);
    }
}
