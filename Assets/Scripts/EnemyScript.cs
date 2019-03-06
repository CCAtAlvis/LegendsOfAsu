using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int enemyHealth = 10;

    public void takeDamage(int damageAmount)
    {
        Debug.Log("enemy hit" + enemyHealth);
        enemyHealth -= damageAmount;

        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
