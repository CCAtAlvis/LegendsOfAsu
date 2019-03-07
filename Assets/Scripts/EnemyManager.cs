using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    //public int minEnemies = 3;
    //public int maxEnemies = 10;

    //public int numOfEnemies = 0;
    public int startTime = 1;
    public int delayTime = 3;
    public GameManager gameManager;
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    private int playerId = 1;
    private int matchTime;
    private bool waiting;
    private float matchTimer = 0f;

    // Use this for initialization
    void Start()
    {
        matchTime = gameManager.matchTime;
        SpawnEnemy();
    }


    void FixedUpdate()
    {
        matchTimer += Time.fixedDeltaTime;
    }

    void SpawnEnemy()
    {
        int i = Random.Range(0, spawnPoints.Length);
        float p = Random.Range(0f, (float)enemyPrefabs.Length);
        p += (matchTimer / matchTime);
        int prob = (int)Mathf.Round(p);

        if (prob > 1)
            prob--;

        //Debug.Log("spwaning enemy at position: " + i);

        //Debug.Log(prob);
        GameObject newEnemy = Instantiate(enemyPrefabs[prob], spawnPoints[i].position, Quaternion.identity);
        newEnemy.GetComponent<EnemyAI>().followPlayer = playerId;
        playerId *= -1;

        StartCoroutine(SpawnNewEnemy(delayTime));
    }

    IEnumerator SpawnNewEnemy(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SpawnEnemy();
    }
}
