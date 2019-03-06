using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int enemyHealth;

    public void TakeDamage(int damageAmount)
    {
        enemyHealth -= damageAmount;

        //Debug.Log("enemy hit" + enemyHealth);
        if (enemyHealth <= 0)
        {
            //play enemy die animation here
            //then destroy the game object
            Destroy(gameObject);
        }
    }
}
