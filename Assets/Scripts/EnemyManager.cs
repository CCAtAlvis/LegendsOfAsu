using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public int minEnemies = 3;
    //public int maxEnemies = 10;

    //public int numOfEnemies = 0;
    public int startTime = 1;
    public int delayTime = 3;
    public int matchTime = 8;

    private int playerId = 1;

    public Transform[] spawnPoint;
    public GameObject enemyPrefab;
    private bool waiting;

    // Use this for initialization
    void Start()
    {
        matchTime *= 60;
        InvokeRepeating("SpawnEnemy", startTime, delayTime);
    }

    void SpawnEnemy()
    {
        int i = Random.Range(0, spawnPoint.Length);
        //Debug.Log("spwaning enemy at position: " + i);

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint[i].position, Quaternion.identity);
        newEnemy.GetComponent<EnemyAI>().followPlayer = playerId;
        playerId *= -1;
    }
}
