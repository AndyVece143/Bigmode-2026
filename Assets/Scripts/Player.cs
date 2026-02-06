using UnityEngine;
using UnityEngine.UIElements;

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

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
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
        float horizontalInput = Input.GetAxis("Horizontal");
        body.AddForce(new Vector2(horizontalInput * speed * 1, 0f), ForceMode2D.Force);
        body.linearVelocity = new Vector2(Mathf.Clamp(body.linearVelocity.x, -speed, speed), body.linearVelocity.y);
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        //movement.x = horizontalInput * speed * Time.deltaTime;
        //transform.Translate(movement);
        //body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);

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

        grounded = isGrounded();
        //Animation
        anim.SetBool("grounded", grounded);
        anim.SetBool("move", horizontalInput != 0f);

    }

    public void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        grounded = false;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
