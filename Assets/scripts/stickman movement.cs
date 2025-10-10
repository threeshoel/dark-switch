using UnityEngine;

public class StickmanMovement : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D coll;
    float moveInput;
    public float speed = 5f;
    public float jumpForce = 5f;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Move()
    {
        isGrounded = Physics2D.IsTouchingLayers(coll, LayerMask.GetMask("Ground"));
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump();
        }
    }
    void jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        isGrounded = false;
    }

    void Start()
    {
        // Optional: Initialization code
    }

    void Update()
    {
        Move();
    }
}
