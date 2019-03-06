using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackRange : MonoBehaviour
{

    private bool playerFacingRight;
    private float timeBtwAttack;
    public float startTimeBtwAttack;
    public int playerAttackPower = 1;
    public Transform attackPos;
    public Transform longAttackPos;
    public Transform allSideAttackPos;
    public LayerMask whatIsEnemies;
    public float basicAttackRangeX = 1;
    public float attackRangeY = 1;
    public float longAttackRangeX = 3;
    public float attackRadius = 1;
    EnemyScript enemyScript;
    public bool enemyInRange = false;
    public bool defenseMode;
    public bool inAir;
    public Animator animator;
    private PlayerController pc;

    void Start ()
    {
        //playerFacingRight = gameObject.GetComponent<PlayerController>().facingRight;
        pc = GetComponent<PlayerController>();
        defenseMode = false;
    }

    void Update()
    {
        inAir = pc.isInAir;
        animator.SetBool("slashAttack", false);
        animator.SetBool("defense", false);
        if (timeBtwAttack <= 0)
        {
            timeBtwAttack = startTimeBtwAttack;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.SetBool("slashAttack", true);
                //attackRangeX = 1;
                Debug.Log("Q pressed");
                Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(basicAttackRangeX, attackRangeY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    enemiesToHit[i].gameObject.GetComponent<EnemyScript>().takeDamage(playerAttackPower);

                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("R pressed");
                Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(attackPos.position, new Vector2(basicAttackRangeX, attackRangeY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    enemiesToHit[i].gameObject.GetComponent<EnemyScript>().takeDamage(playerAttackPower * 2);

                }
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                /*
                if(playerFacingRight)
                    attackPos.position += Vector3.right*0.5f;
                if(!playerFacingRight)
                    attackPos.position += Vector3.right * 0.5f;
                */
                //attackRangeX = 3;
                Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(longAttackPos.position, new Vector2(longAttackRangeX, attackRangeY), 0, whatIsEnemies);
                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    enemiesToHit[i].gameObject.GetComponent<EnemyScript>().takeDamage(playerAttackPower);

                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //attackRangeX = 2;
                Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(allSideAttackPos.position, attackRadius, whatIsEnemies);
                for (int i = 0; i < enemiesToHit.Length; i++)
                {
                    enemiesToHit[i].gameObject.GetComponent<EnemyScript>().takeDamage(playerAttackPower);

                }
            }
            if (Input.GetKey(KeyCode.T))
            {
                defenseMode = true;
                animator.SetBool("defense", true);
                Debug.Log("defenseMode ON");
            }
            if (Input.GetKeyUp(KeyCode.T))
            {
                Debug.Log("defenseMode OFF");
                defenseMode = false;
            }
        }
        else
        {
            timeBtwAttack -= Time.deltaTime;
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
