using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{
    public int playerAttackPower = 1;

    public int basicAttackPower = 2;
    public int longAttackPower = 3;
    public int radiusAttackPower = 1;

    public float basicAttackRangeX = 1;
    public float longAttackRangeX = 3;
    public float attackRadius = 1;

    public float attackRangeY = 1;

    public float longAttackPressTime = 5f;
    private float longAttackTimer = 0f;

    public int baseScore = 100;
    public int baseScoreMultiplerHits = 10;
    public float maxTimeWithoutHit = 50f;
    private int hitCount = 0;
    //private int ScoreMultiplerHits = 10;
    private int scoreMultipler = 1;
    private float hitTimer = 0f;
    private bool didPlayerHit = false;

    public Transform attackPos;
    public Transform longAttackPos;
    public Transform allSideAttackPos;

    public bool enemyInRange = false;
    public bool defenseMode;
    public bool inAir;
    public bool playerIsHit;

    public Animator animator;
    public LayerMask whatIsEnemies;
    public GameManager gameManager;

    private EnemyAI enemy;
    private PlayerController pc;
    private bool playerFacingRight;

    private string basicAttackKey;
    private string longAttackKey;
    private string allSideAttackKey;
    private string powerAttackKey;
    private string defenceKey;

    void Start()
    {
        //playerFacingRight = gameObject.GetComponent<PlayerController>().facingRight;
        pc = GetComponent<PlayerController>();

        defenseMode = false;

        if (pc.playerId == 1)
        {
            basicAttackKey = "Fire1Player1";
            longAttackKey = "Fire2Player1";
            allSideAttackKey = "Fire3Player1";
            powerAttackKey = "Fire3Player1";
            defenceKey = "DefencePlayer1";
        }
        else
        {
            basicAttackKey = "Fire1Player2";
            longAttackKey = "Fire2Player2";
            allSideAttackKey = "Fire3Player2";
            powerAttackKey = "Fire3Player2";
            defenceKey = "DefencePlayer2";
        }
    }

    void Update()
    {
        playerIsHit = pc.playerIsHit;
        inAir = pc.isInAir;
        animator.SetBool("slashAttack", false);
        animator.SetBool("defense", false);

        if (hitTimer >= maxTimeWithoutHit)
        {
            ResetScoreMultipler();
            hitTimer = 0f;
            hitCount = 0;
        }

        if (didPlayerHit)
        {
            hitTimer = 0f;
            didPlayerHit = false;
        }

        if (hitCount > baseScoreMultiplerHits * scoreMultipler)
        {
            hitCount = 0;
            hitTimer = 0;
            scoreMultipler++;
            //Debug.Log(scoreMultipler);
        }

        hitTimer += Time.deltaTime;

        //simple attack
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown(basicAttackKey))&& !playerIsHit)
        {
            animator.SetBool("slashAttack", true);
            Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(basicAttackRangeX, attackRangeY), 0, whatIsEnemies);

            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemy = enemiesToHit[i].gameObject.GetComponent<EnemyAI>();
                enemy.TakeDamage(playerAttackPower);

                gameManager.AddScore(baseScore * scoreMultipler);
                hitCount++;
                didPlayerHit = true;
            }
        }

        //
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Debug.Log("R pressed");
            Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(basicAttackRangeX, attackRangeY), 0, whatIsEnemies);

            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemy = enemiesToHit[i].gameObject.GetComponent<EnemyAI>();
                enemy.TakeDamage(playerAttackPower);

                gameManager.AddScore(baseScore * scoreMultipler);
                hitCount++;
                didPlayerHit = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(allSideAttackPos.position, attackRadius, whatIsEnemies);

            for (int i = 0; i < enemiesToHit.Length; i++)
            {
                enemy = enemiesToHit[i].gameObject.GetComponent<EnemyAI>();
                enemy.TakeDamage(playerAttackPower);

                gameManager.AddScore(baseScore * scoreMultipler);
                hitCount++;
                didPlayerHit = true;
            }
        }

        if (Input.GetButton(longAttackKey))
        {
            longAttackTimer += Time.deltaTime;

            if (longAttackTimer >= longAttackPressTime)
            {
                //TODO:
                //do long attack
                Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(basicAttackRangeX, attackRangeY), 0, whatIsEnemies);

                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    enemy = enemiesToHit[i].gameObject.GetComponent<EnemyAI>();
                    enemy.TakeDamage(playerAttackPower);

                    gameManager.AddScore(baseScore * scoreMultipler);
                    hitCount++;
                    didPlayerHit = true;
                }

            }
        }
        if (Input.GetButtonUp(longAttackKey))
        {
            longAttackTimer = 0f;
        }


        if (Input.GetKey(KeyCode.T))
        {
            defenseMode = true;
            animator.SetBool("defense", true);
            //Debug.Log("defenseMode ON");
        }
        if (Input.GetKeyUp(KeyCode.T))
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
    }

    public void ResetScoreMultipler()
    {
        //Debug.Log("reset multupler");
        scoreMultipler = 1;
    }
}
