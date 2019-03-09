using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float phaseOneTime = 120;
    public float phaseTwoTime = 120;
    public float phaseOneTimer = 0f;
    public float cameraSpeed = 2f;
    public bool canTransition = false;
    private bool phaseOne = true;
    private bool startedMoving = false;
    public EnemyManager enemyManager;
    Vector3 a;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        canTransition = enemyManager.cameraTransition;
        if (phaseOneTimer < phaseOneTime)
        {
            //phaseOne = false;
            phaseOneTimer += Time.deltaTime;
            a = transform.position;
            startTime = Time.time;
        }
        else
        {
            if (canTransition)
            {
                if(!startedMoving)
                {
                    startTime = Time.time;
                    startedMoving = true;
                }
                float distCovered = (Time.time - startTime) * cameraSpeed;
                float fracJourney = distCovered / 20f;
                transform.position = Vector3.Lerp(transform.position, new Vector3(a.x + 20f, a.y, a.z), fracJourney);
            }
        }
    }

}
