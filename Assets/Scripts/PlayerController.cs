using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    //public Vector2 xVelocity;
    public int playerId = 1;
    public bool facingRight = true;
    public float force = 10f;
    public float jumpForce = 400f;
    public float jumpHeight = 10f;
    public bool isInAir = false;
    private Vector3 initialPosition;
    public Animator animator;
    public bool defenseMode;
    [SerializeField]
    private Rigidbody2D rb;
    private float h;
    private float v;
    private PlayerAttackRange par;
    // Use this for initialization

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        par = GetComponent<PlayerAttackRange>();
        Debug.Log(defenseMode);
        Debug.Log("Script started");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        defenseMode = par.defenseMode;

        if (isInAir && transform.position.y <= initialPosition.y)
        {
            //rb.transform.Translate(transform.position.x, initialPosition.y, 0);
            Debug.Log("Transform Position :" + transform.position.y);
            rb.gravityScale = 0;
            //rb.velocity = Vector3.zero;
            isInAir = false;
            animator.SetBool("inAir", false);
        }
        else if (!isInAir && !defenseMode)
        {
            Debug.Log("not in air");
            //float h = CrossPlatformInputManager.GetAxis("Horizontal");
            //float v = CrossPlatformInputManager.GetAxis("Vertical");
            if (playerId == 1)
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
            }
            else
            {
                h = Input.GetAxis("HorizontalPlayer2");
                v = Input.GetAxis("VerticalPlayer2");
            }

            rb.velocity = new Vector2(h, v) * force * Time.deltaTime;
            Debug.Log(h + v);
            animator.SetFloat("Speed", Mathf.Abs(h + v));

            if ((h > 0 && !facingRight) || (h < 0 && facingRight))
            {
                Flip();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector2 lastVelocity = rb.velocity;
                Debug.Log("last Velocity " + lastVelocity);
                Debug.Log("Space Pressed");
                isInAir = true;
                animator.SetBool("inAir", true);
                initialPosition = transform.position;
                Debug.Log("initialPosition" + initialPosition);
                //rb.velocity += Vector2.up * jumpForce;
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                Debug.Log("Multiplying Vector " + lastVelocity);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
                //rb.velocity = lastVelocity * force * Time.deltaTime;
                //rb.velocity = xVelocity * force * Time.deltaTime;
                //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.gravityScale = 2f;
                //int i = 0;
                //rb.gravityScale = 0;
                //isInAir = false;
            }
        }
    }

    private void Flip()
    {
        Debug.Log("flipped");
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
