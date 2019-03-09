using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public EnemyManager enemyManager;
    public GameObject bossPrefab;
    public GameObject spawnPoint;
    private bool canSpawnBoss = false;
    private bool notSpawnBoss = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canSpawnBoss = enemyManager.cameraTransition;
        if(canSpawnBoss && notSpawnBoss)
        {
            SpawnBoss();
            notSpawnBoss = false;
        }


    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, spawnPoint.transform.position, Quaternion.identity);
    }
}
