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
    private int phaseTime;
    private bool waiting;
    private float matchTimer = 0f;
    private float phaseTimer = 0f;
    private int phase;
    private int enemyRange;
    private bool evenPhase;

    // Use this for initialization
    void Start()
    {
        phase = 0;
        enemyRange = 0;
        //matchTime = 60*gameManager.matchTime;
        //phaseTime = 60*gameManager.phaseTime;
        matchTime = gameManager.matchTime;
        phaseTime = gameManager.phaseTime;
        SpawnEnemy();
    }


    void FixedUpdate()
    {
        matchTimer += Time.fixedDeltaTime;
        phaseTimer += Time.fixedDeltaTime;
        if(phaseTimer>phaseTime)
        {
            if (phase % 2 == 0)
                evenPhase = true;
            else
            {
                evenPhase = false;
                if (enemyRange < enemyPrefabs.Length-1)
                    enemyRange++;
            }
            phase++;
            Debug.Log("starting phase : " + phase);
            phaseTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        //Randomly selecting one of the spawn Points
        int i = Random.Range(0, spawnPoints.Length);

        //Randomly selecting a enemy
        /*
        float p = Random.Range(0f, (float)phase);
        p += (matchTimer / matchTime);
        int prob = (int)Mathf.Round(p);
        */
        int prob;
        if (evenPhase)
        {
            prob = enemyRange;
        }
        else
        {
            prob = Random.Range(0, enemyRange+1);
        }
        //if (prob > 1)
        //    prob--;

        //Debug.Log("spwaning enemy at position: " + i);

        Debug.Log(prob);
        GameObject newEnemy = Instantiate(enemyPrefabs[prob], spawnPoints[i].position, Quaternion.identity);
        newEnemy.GetComponent<EnemyAI>().followPlayer = playerId;
        playerId *= -1;

        // wait sometime before spawning next enemy
        StartCoroutine(SpawnNewEnemy(delayTime));
    }

    IEnumerator SpawnNewEnemy(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        SpawnEnemy();
    }
}
