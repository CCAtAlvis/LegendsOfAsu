using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public Vector2 xVelocity;
    public int playerId = 1;
    public int playerHealth = 10;
    private int playerHealthMax;

    public float force = 10f;
    public float jumpForce = 400f;
    public float jumpHeight = 10f;

    public bool isFacingRight = true;
    public bool isInAir = false;
    public bool isInDefenseMode;

    public Animator animator;

    [SerializeField]
    public Rigidbody2D rb;
    public PlayerAttackRange par;
    public BoxCollider2D boxCollider;

    private Vector3 initialPosition;
    private float h;
    private float v;

    private string horizontal;
    private string vertical;

    private bool canMove = true;

    // Use this for initialization
    void Start()
    {
        //Debug.Log(isInDefenseMode);
        //Debug.Log("Script started");
        rb = GetComponent<Rigidbody2D>();
        par = GetComponent<PlayerAttackRange>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (playerId == 1)
        {
            horizontal = "HorizontalPlayer1";
            vertical = "VerticalPlayer1";
        }
        else
        {
            horizontal = "HorizontalPlayer2";
            vertical = "VerticalPlayer2";
        }

        playerHealthMax = playerHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!canMove)
        //    return;

        isInDefenseMode = par.defenseMode;

        if (isInAir && transform.position.y <= initialPosition.y)
        {
            //Debug.Log("Transform Position :" + transform.position.y);
            Debug.Log(initialPosition.y + " : " + transform.position.y);
            rb.gravityScale = 0;
            //rb.velocity = Vector3.zero;
            isInAir = false;
            animator.SetBool("inAir", false);
            //Debug.Log(initialPosition);
            transform.position = new Vector2(transform.position.x, initialPosition.y);
            boxCollider.enabled = true;
        }
        else if (!isInAir)
        {
            //Debug.Log("not in air and not in defence mode");

            h = Input.GetAxis(horizontal);
            v = Input.GetAxis(vertical);

            rb.velocity = new Vector2(h, v) * force * Time.deltaTime;
            //Debug.Log(h + v);
            animator.SetFloat("Speed", Mathf.Abs(h + v));

            if ((h > 0 && !isFacingRight) || (h < 0 && isFacingRight))
            {
                Flip();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 lastVelocity = rb.velocity;
                //Debug.Log("last Velocity " + lastVelocity);
                //Debug.Log("Space Pressed");
                isInAir = true;
                animator.SetBool("inAir", true);
                initialPosition = transform.position;
                //Debug.Log("initialPosition" + initialPosition);

                //Debug.Log("Multiplying Vector " + lastVelocity);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);

                rb.gravityScale = 2f;
                boxCollider.enabled = false;
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale *= new Vector2(-1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        if (col.tag.Equals("Player"))
        {
            //Debug.Log("collision with other player");
            canMove = false;
        }
        else if (col.tag.Equals("Enemy"))
        {
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canMove = true;
    }

    public void TakeDamage(int damageAmount)
    {
        bool isInDefence = par.defenseMode;
        if (isInDefence)
            return;
        //animator.SetTrigger("hit");
        playerHealth -= damageAmount;
        par.ResetScoreMultipler();

        if (playerHealth <= 0)
        {
            // player die anim here
            // respawn player with playerHealth = playerHealthMax
        }
    }
}
