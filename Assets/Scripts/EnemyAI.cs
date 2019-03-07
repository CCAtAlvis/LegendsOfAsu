using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int followPlayer;
    public int hitDamage = 1;
    public int enemyHealth = 10;
    public float offsetValue = 0.1f;
    public float speed = 1.4f;
    public float afterAttackPauseTime = 2f;
    public float takeDamagePauseTime = 3f;
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
    private bool didEnemyHit = false;
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

        if (Vector2.Distance(nearerOffset, transform.position) > 0)
        {
            //Debug.Log("player on ground");
            if (transform.position.x < target.position.x && !facingRight)
                Flip();
            else if (transform.position.x > target.position.x && facingRight)
                Flip();

            transform.position = Vector2.MoveTowards(transform.position, nearerOffset, speed * Time.deltaTime);
            //animator.SetBool("walk", true);
        }
        else
        {
            //animator.SetBool("walk", false);
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
        //Collider2D col = collision.collider;
        if (col.tag.Equals("Player"))
        {
            enemyInRange = true;

            pc = col.gameObject.GetComponent<PlayerController>();
            followPlayer = pc.playerId;
            target = col.gameObject.transform;

            //some shit happening here "works hopefully"
            //first wait for some random time
            didEnemyHit = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (isEnemyPaused)
            return;

        if (didEnemyHit && pc != null)
        {
            //then get the distance between player and enemy
            if (Vector3.Distance(transform.position, pc.transform.position) <= offsetValue * 2)
            {
                //player takes damage
                pc.TakeDamage(hitDamage);

                //play the animation and while enemy is paused
                animator.SetTrigger("attackPlayer");
                StartCoroutine(PauseEnemyMovement(attackAnimTime));

                //pause the enemy for some more time
                //float pTime = Random.Range(afterAttackPauseTime - 0.5f, afterAttackPauseTime);
                //StartCoroutine(PauseEnemyMovement(afterAttackPauseTime));
                pc = null;
            }

            didEnemyHit = false;
            return;
        }

        if (Vector2.Distance(nearerOffset, transform.position) > 0)
        {
            //Debug.Log("IF");
            
            if (transform.position.x < target.position.x && !facingRight)
                Flip();
            else if (transform.position.x > target.position.x && facingRight)
                Flip();

            transform.position = Vector2.MoveTowards(transform.position, nearerOffset, speed * Time.deltaTime);
            
        }
        else
        {
            //Debug.Log("PlayerInRange");

            //Collider2D col = collision.collider;
            if (col.tag.Equals("Player"))
            {
                enemyInRange = true;

                pc = col.gameObject.GetComponent<PlayerController>();
                followPlayer = pc.playerId;
                target = col.gameObject.transform;

                //some shit happening here "works hopefully"
                //first wait for some random time
                didEnemyHit = true;

                float pTime = Random.Range(0.1f, 0.5f);
                StartCoroutine(PauseEnemyMovement(pTime));
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        //animator.SetBool("walk", true);
    }

    public void TakeDamage(int damageAmount)
    {
        enemyHealth -= damageAmount;

        //Debug.Log("enemy hit" + enemyHealth);
        if (enemyHealth <= 0)
        {
            //play enemy die animation here
            //animator.SetTrigger("dead");
            //then destroy the game object
            Destroy(gameObject);
        }

        //play the animation and while enemy is paused
        //animator.SetBool("hit",true);
        //animator.SetTrigger("hit");
        StartCoroutine(PauseEnemyMovement(hitAnimTime));

        StartCoroutine(PauseEnemyMovement(takeDamagePauseTime));
    }

    IEnumerator PauseEnemyMovement(float pauseDuration)
    {
        isEnemyPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        isEnemyPaused = false;
        //animator.SetBool("walk", true);
    }
}
