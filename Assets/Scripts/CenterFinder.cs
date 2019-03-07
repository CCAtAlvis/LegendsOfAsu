using UnityEngine;

public class CenterFinder : MonoBehaviour
{
    public Camera myCamera;
    public float cameraZoom = 20f;
    public Transform player1;
    public Transform player2;
    Vector3 center;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (player1.position + player2.position) / 2;
        myCamera.orthographicSize = cameraZoom;
    }
}
