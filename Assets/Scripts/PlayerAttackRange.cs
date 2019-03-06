using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    //public float startTimeBtwAttack;
    public int playerAttackPower = 1;

    public float basicAttackRangeX = 1;
    public float longAttackRangeX = 3;
    public float attackRadius = 1;
    public float attackRangeY = 1;

    public Transform attackPos;
    public Transform longAttackPos;
    public Transform allSideAttackPos;

    public bool enemyInRange = false;
    public bool defenseMode;
    public bool inAir;

    public Animator animator;
    public LayerMask whatIsEnemies;

    private EnemyScript enemyScript;
    private PlayerController pc;
    //public PlayerController playerController;
    private bool playerFacingRight;
    //private float timeBtwAttack;

    private string basicAttackKey;
    private string longAttackKey;
    private string allSideAttackKey;
    private string powerAttackKey;

    void Start()
    {
        //playerFacingRight = gameObject.GetComponent<PlayerController>().facingRight;
        pc = GetComponent<PlayerController>();
        defenseMode = false;

        if (pc.playerId == 1)
        {
            basicAttackKey = "BasicAttackPlayer1";
            longAttackKey = "LongAttackPlayer1";
            allSideAttackKey = "AllSideAttackPlayer1";
            powerAttackKey = "PowerAttackPlayer1";
        }
        else
        {
            basicAttackKey = "BasicAttackPlayer2";
            longAttackKey = "LongAttackPlayer2";
            allSideAttackKey = "AllSideAttackPlayer2";
            powerAttackKey = "PowerAttackPlayer2";
        }
    }

    void Update()
    {
        inAir = pc.isInAir;
        animator.SetBool("slashAttack", false);
        animator.SetBool("defense", false);

        //simple attack
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown(basicAttackKey))
        {
            animator.SetBool("slashAttack", true);
            Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(basicAttackRangeX, attackRangeY), 0, whatIsEnemies);

            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemiesToHit[i].gameObject.GetComponent<EnemyScript>().TakeDamage(playerAttackPower);
            }
        }

        //
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Debug.Log("R pressed");
            Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(basicAttackRangeX, attackRangeY), 0, whatIsEnemies);

            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemiesToHit[i].gameObject.GetComponent<EnemyScript>().TakeDamage(playerAttackPower * 2);
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(longAttackPos.position, new Vector2(longAttackRangeX, attackRangeY), 0, whatIsEnemies);

            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemiesToHit[i].gameObject.GetComponent<EnemyScript>().TakeDamage(playerAttackPower);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(allSideAttackPos.position, attackRadius, whatIsEnemies);

            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemiesToHit[i].gameObject.GetComponent<EnemyScript>().TakeDamage(playerAttackPower);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            defenseMode = true;
            animator.SetBool("defense", true);
            //Debug.Log("defenseMode ON");
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            //Debug.Log("defenseMode OFF");
            defenseMode = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, new Vector3(basicAttackRangeX, attackRangeY, 0));
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(longAttackPos.position, new Vector3(longAttackRangeX, attackRangeY, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(allSideAttackPos.position, attackRadius);
        //Gizmos.DrawWireSphere();
    }
}
