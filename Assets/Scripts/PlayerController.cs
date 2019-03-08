using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public Vector2 xVelocity;
    public int playerId = 1;
    public int playerHealth = 10;
    private int playerHealthMax;
    public float disableTime = 10;
    public bool isPlayerDisable = false;
    public float fadeAlpha = 50;

    public float force = 10f;
    public float jumpForce = 400f;
    public float jumpHeight = 10f;

    public bool isFacingRight = true;
    public bool isInAir = false;
    public bool isInDefenseMode;
    public bool isPlayerHit;

    public Animator animator;
    public SpriteRenderer sprite;
    public Slider healthBar;

    [SerializeField]
    public Rigidbody2D rb;
    public PlayerAttackRange par;
    public BoxCollider2D boxCollider;

    private Vector3 initialPosition;
    private float h;
    private float v;

    private string horizontal;
    private string vertical;
    private string jump;

    private bool canMove = true;

    // Use this for initialization
    void Start()
    {
        //Debug.Log(isInDefenseMode);
        //Debug.Log("Script started");
        rb = GetComponent<Rigidbody2D>();
        par = GetComponent<PlayerAttackRange>();
        boxCollider = GetComponent<BoxCollider2D>();

        playerHealthMax = playerHealth;

        if (playerId == 1)
        {
            horizontal = "HorizontalPlayer1";
            vertical = "VerticalPlayer1";
            jump = "JumpPlayer1";
        }
        else
        {
            horizontal = "HorizontalPlayer2";
            vertical = "VerticalPlayer2";
            jump = "JumpPlayer2";
        }

        playerHealthMax = playerHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!canMove)
        //    return;

        isInDefenseMode = par.defenseMode;

        if (isInAir && transform.position.y <= initialPosition.y && !isPlayerDisable)
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
        else if (!isInAir && !isPlayerDisable)
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

            //if (Input.GetButtonDown(jump))
            //{
            //    Vector2 lastVelocity = rb.velocity;
            //    //Debug.Log("last Velocity " + lastVelocity);
            //    //Debug.Log("Space Pressed");
            //    isInAir = true;
            //    animator.SetBool("inAir", true);
            //    initialPosition = transform.position;
            //    //Debug.Log("initialPosition" + initialPosition);

            //    //Debug.Log("Multiplying Vector " + lastVelocity);
            //    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);

            //    rb.gravityScale = 2f;
            //    boxCollider.enabled = false;
            //}
        }

        healthBar.value = (float)playerHealth / (float)playerHealthMax;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale *= new Vector2(-1, 1);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        if(col.tag.Equals("Player"))
        {
            canMove = false;
            rb.velocity = Vector2.zero;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        if (col.tag.Equals("Player"))
        {
            //Debug.Log("collision with other player");
            canMove = false;
        }
        //else if (col.tag.Equals("Enemy"))
        //{
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        canMove = true;
    }

    public void TakeDamage(int damageAmount)
    {
        if (isPlayerDisable)
            return;

        if (!isPlayerDisable)
        {
            bool isInDefence = par.defenseMode;
            if (isInDefence)
                return;

            //animator.SetBool("hit",true);
            animator.SetTrigger("hit 0");
            isPlayerHit = true;
            playerHealth -= damageAmount;
            //print("Player Health :" + playerHealth);
            par.ResetScoreMultipler();
            animator.SetBool("idle", true);
            isPlayerHit = false;
        }

        if (playerHealth <= 0)
        {
            if (!isPlayerDisable)
            {
                Debug.Log("playerIsDisabled");
                StartCoroutine(PlayerDisable(disableTime));
            }
            // player die anim here
            // respawn player with playerHealth = playerHealthMax
            //Time.timeScale = 0;
        }
    }

    IEnumerator PlayerDisable(float pauseDuration)
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
        isPlayerDisable = true;
        Debug.Log("waiting");
        animator.SetFloat("Speed", 0.0f);
        animator.SetBool("idle", true);

        rb.velocity = Vector2.zero; 

        yield return new WaitForSeconds(pauseDuration + 0.1f);

        playerHealth = playerHealthMax;
        Debug.Log("finished waiting");
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 255);
        isPlayerDisable = false;
        //Destroy(gameObject);
    }
}
