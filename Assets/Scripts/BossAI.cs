using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Animator animator;
    public float bossHealth;
    public int hitDamage;
    public float deadAnimTime;
    public float takeDamagePauseTime;

    private bool isBossPaused;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DoAttack", 20f, 15f);
        InvokeRepeating("DoJump", 15f, 7f);
    }

    void DoJump()
    {
        if (isBossPaused)
            return;

        //TODO: JUMP
        //flip

        animator.SetTrigger("jump");
    }

    void DoAttack()
    {
        if (isBossPaused)
            return;

        Collider2D[] enemiesToHit = Physics2D.OverlapCircleAll(allSideAttackPos.position, allSideAttackRadius, whatIsEnemies);
        for (int i = 0; i < enemiesToHit.Length; i++)
        {
            PlayerController pc = enemiesToHit[i].gameObject.GetComponent<PlayerController>();
            pc.TakeDamage(hitDamage);
      
        }

        animator.SetTrigger("attack");
    }

    public void TakeDamage(int damageAmount)
    {
        bossHealth -= damageAmount;

        if (bossHealth <= 0)
        {
            //Pause game here
        }

        StartCoroutine(PauseBossMovement(takeDamagePauseTime));
    }

    IEnumerator PauseBossMovement(float pauseDuration)
    {
        isBossPaused = true;
        animator.SetBool("idle", true);

        yield return new WaitForSeconds(pauseDuration);

        isBossPaused = false;
        animator.SetBool("idle", false);
    }
}
