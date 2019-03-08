using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int followPlayer;
    public int hitDamage = 1;
    public int enemyHealth = 10;
    public float offsetValue = 0.1f;
    public float speed = 1.4f;

    public float afterAttackPauseTime = 1f;
    public float takeDamagePauseTime = 1.6f;
    public float beforeFlipPauseTime = 1.1f;
    public float deathAnimTime = 05f;
    public bool isEnemyDead = false;

    public int attackAnimTime = 10;
    public int hitAnimTime = 10;

    public Animator animator;

    private bool enemyInRange;
    private bool facingRight = true;
    private GameObject[] targetPlayers;
    private GameObject targetPlayer;
    private Transform target;
    private bool inAir;

    private Vector2 nearerOffset;
    private Vector2 rightOffset;
    private Vector2 leftOffset;

    private Rigidbody2D rb;
    private PlayerController pc;
    private bool isEnemyPaused = false;
    private bool isPlayingAnimation = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        targetPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < targetPlayers.Length; i++)
        {
            if (targetPlayers[i].GetComponent<PlayerController>().playerId == followPlayer)
            {
                targetPlayer = targetPlayers[i];
                //target = targetPlayer.GetComponent<Transform>();
                target = targetPlayer.gameObject.transform;
                //Debug.Log("a enemy following player " + followPlayer);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemyPaused)
            return;

        inAir = targetPlayer.GetComponent<PlayerController>().isInAir;

        if (!inAir)
        {
            rightOffset = target.position + new Vector3(1, 0, 0) * offsetValue;
            leftOffset = target.position + new Vector3(-1, 0, 0) * offsetValue;
             
            if (Vector2.Distance(rightOffset, transform.position) > Vector2.Distance(leftOffset, transform.position))
            {
                nearerOffset = leftOffset;
            }
            else
            {
                nearerOffset = rightOffset;
            }
        }

        if (Vector2.Distance(nearerOffset, transform.position) > 0.2f)
        {
            //Debug.Log("player on ground");
            if (transform.position.x < target.position.x && !facingRight)
            {
                Flip();
                StartCoroutine(PauseEnemyMovement(beforeFlipPauseTime));
            }
            else if (transform.position.x > target.position.x && facingRight)
            {
                Flip();
                StartCoroutine(PauseEnemyMovement(beforeFlipPauseTime));
            }

            transform.position = Vector2.MoveTowards(transform.position, nearerOffset, speed * Time.deltaTime);
            animator.SetBool("walk", true);
        }
        else
        {
            animator.SetBool("walk", false);
            animator.SetBool("idle", true);
        }
    }

    void Flip()
    {
        //Debug.Log("flipped");
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isEnemyPaused)
            return;

        if (col.tag.Equals("Player") && IsPlayerInRange(col.transform.position))
        {
            pc = col.gameObject.GetComponent<PlayerController>();
            followPlayer = pc.playerId;
            target = col.gameObject.transform;

            //player takes damage
            pc.TakeDamage(hitDamage);

            //play the animation and while enemy is paused
            animator.SetBool("idle", false);
            animator.SetTrigger("attackPlayer");
            StartCoroutine(PauseEnemyMovement(afterAttackPauseTime));
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (isEnemyPaused)
            return;

        if (col.tag.Equals("Player") && IsPlayerInRange(col.transform.position))
        {
            pc = col.gameObject.GetComponent<PlayerController>();
            followPlayer = pc.playerId;
            target = col.gameObject.transform;

            //player takes damage
            pc.TakeDamage(hitDamage);

            //play the animation and while enemy is paused
            animator.SetBool("idle", false);
            animator.SetTrigger("attackPlayer");
            StartCoroutine(PauseEnemyMovement(afterAttackPauseTime));
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        animator.SetBool("walk", true);
    }

    public void TakeDamage(int damageAmount)
    {
        enemyHealth -= damageAmount;

        //Debug.Log("enemy hit" + enemyHealth);
        if (enemyHealth <= 0)
        {
            if (!isEnemyDead)
            {
                //play enemy die animation here
                animator.SetTrigger("dead");
                //then destroy the game object
                StartCoroutine(EnemyDead(deathAnimTime));
            }
       }

        //play the animation and while enemy is paused
        //animator.SetBool("hit",true);
        animator.SetTrigger("hit");

        StartCoroutine(PauseEnemyMovement(takeDamagePauseTime));
    }

    private bool IsPlayerInRange(Vector3 pos)
    {
        if ((transform.position.y > pos.y - 0.2f) && (transform.position.y < pos.y + 0.2f))
        {
            return true;
        }

        return false;
    }

    IEnumerator PauseEnemyMovement(float pauseDuration)
    {
        isEnemyPaused = true;

        animator.SetBool("walk", false);
        animator.SetBool("idle", true);

        yield return new WaitForSeconds(pauseDuration);

        isEnemyPaused = false;
        //animator.ResetTrigger("attackPlayer");
        //animator.ResetTrigger("hit");
        animator.SetBool("idle", false);
        animator.SetBool("walk", true);
    }


    IEnumerator EnemyDead(float pauseDuration)
    {
        isEnemyDead = true;
        isEnemyPaused = true;
        yield return new WaitForSeconds(pauseDuration+0.1f);
        Destroy(gameObject);
    }
}
