using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public Vector2 xVelocity;
    public int playerId = 1;
    public bool facingRight = true;
    public float force = 10f;
    public float jumpForce = 400f;
    public float jumpHeight = 10f;
    public bool isInAir = false;
    public Animator animator;
    public bool isInDefenseMode;

    [SerializeField]
    public Rigidbody2D rb;
    public PlayerAttackRange par;
    public BoxCollider2D boxCollider;

    private Vector3 initialPosition;
    private float h;
    private float v;

    private string horizontal;
    private string vertical;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start()
    {
        par = GetComponent<PlayerAttackRange>();
        //Debug.Log(isInDefenseMode);
        //Debug.Log("Script started");
        boxCollider = GetComponent<BoxCollider2D>();

        if (playerId == 1)
        {
            horizontal = "Horizontal";
            vertical = "Vertical";
        }
        else
        {
            horizontal = "HorizontalPlayer2";
            vertical = "VerticalPlayer2";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isInDefenseMode = par.defenseMode;

        if (isInAir && transform.position.y <= initialPosition.y)
        {
            //rb.transform.Translate(transform.position.x, initialPosition.y, 0);
            Debug.Log("Transform Position :" + transform.position.y);
            rb.gravityScale = 0;
            //rb.velocity = Vector3.zero;
            isInAir = false;
            animator.SetBool("inAir", false);
            Debug.Log(initialPosition);
            transform.position = initialPosition;
            boxCollider.enabled = true;
        }
        else if (!isInAir && !isInDefenseMode)
        {
            //Debug.Log("not in air and not in defence mode");

            h = Input.GetAxis(horizontal);
            v = Input.GetAxis(vertical);

            rb.velocity = new Vector2(h, v) * force * Time.deltaTime;
            //Debug.Log(h + v);
            animator.SetFloat("Speed", Mathf.Abs(h + v));

            if ((h > 0 && !facingRight) || (h < 0 && facingRight))
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
        facingRight = !facingRight;
        transform.localScale *= new Vector2(-1, 1);
    }
}
