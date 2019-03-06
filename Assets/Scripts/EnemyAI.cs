using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public int followPlayer;
    public float offsetValue = 1.5f;
    public float speed;

    private bool enemyInRange;
    private bool facingRight = true;
    private GameObject[] targetPlayers;
    private GameObject targetPlayer;
    private Transform target;
    private bool inAir;

    private Vector2 nearerOffset;
    private Vector2 rightOffset;
    private Vector2 leftOffset;

    private Rigidbody2D rb;

   // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        targetPlayers = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < targetPlayers.Length; i++)
        {
            if (targetPlayers[i].GetComponent<PlayerController>().playerId == followPlayer)
            {
                targetPlayer = targetPlayers[i];
                //target = targetPlayer.GetComponent<Transform>();
                target = targetPlayer.gameObject.transform;
                //Debug.Log("a enemy following player " + followPlayer);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        inAir = targetPlayer.GetComponent<PlayerController>().isInAir;

        if (!inAir)
        {
            rightOffset = target.position + new Vector3(1, 0, 0) * offsetValue;
            leftOffset = target.position + new Vector3(-1, 0, 0) * offsetValue;

            if (Vector2.Distance(rightOffset, transform.position) > Vector2.Distance(leftOffset, transform.position))
            {
                nearerOffset = leftOffset;
            }
            else
            {
                nearerOffset = rightOffset;
            }
        }

        if (Vector2.Distance(nearerOffset, transform.position) > 0)
        {
            //Debug.Log("player on ground");
            if (transform.position.x < target.position.x && !facingRight)
                Flip();
            else if (transform.position.x > target.position.x && facingRight)
                Flip();

            transform.position = Vector2.MoveTowards(transform.position, nearerOffset, speed * Time.deltaTime);
        }
    }

    void Flip()
    {
        //Debug.Log("flipped");
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        if (col.gameObject.tag.Equals("Player"))
        {
            //Debug.Log("triggerhit");
            enemyInRange = true;
            followPlayer = col.gameObject.GetComponent<PlayerController>().playerId;
            target = col.gameObject.transform;
        }
    }
}
