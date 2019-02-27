using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyManager : MonoBehaviour {

    public int numOfEnemies = 5;
    public int startTime = 1;
    public int delayTime = 100;
    private int playerId = 1;
    public Transform spawnPoint;
    public GameObject enemy;
    private bool waiting;
	// Use this for initialization
	void Start () {
        waiting = true;
	}
	void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
        newEnemy.GetComponent<EnemyAI>().followPlayer = playerId;
        numOfEnemies--;
        playerId *= -1;
    }
	// Update is called once per frame
	void Update () {

        while (numOfEnemies != 0)
        {
            InvokeRepeating("SpawnEnemy", startTime,delayTime);
            /*
            //waiting = false;
            StartCoroutine(waitTime());
            if (!waiting)
            {
                GameObject newEnemy = Instantiate(enemy, spawnPoint.position, Quaternion.identity);
                newEnemy.GetComponent<EnemyAI>().followPlayer = playerId;
                numOfEnemies--;
                playerId *= -1;
            }
            */
        }
    }

    IEnumerator waitTime()
    {
        Debug.Log("Coroutine started");
        Debug.Log(Time.time);
        yield return new WaitForSeconds(delayTime);
        Debug.Log(Time.time);
        //waiting = true;
    }
}
