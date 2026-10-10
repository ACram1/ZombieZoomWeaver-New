using UnityEngine;
using UnityEngine.Rendering;

public class WatdaMonster : MonoBehaviour
{
    private int health = 100;

    [SerializeField] private float speed = 1f;
    public float timer;
    public float groundTimer;

    public bool facedRight = true;
    private bool hasLineOfSight;
    private bool rotated;
    private bool firstSight;

    [SerializeField] private GameObject player;
    public ZombieSounds src;

    void Update()
    {
        Vector3 buildingEdge = new Vector3(2.19f, 24f, 0f);  //Used as a stopping point when Watda bull-rushes the player

        //If Watda can "see" Player
        if (hasLineOfSight)
        {
            if (!firstSight) //When Watda first sees the Player it lets out a big "roar"
            {
                src.Roar();
                firstSight = true;
            }

            //If Watda has been defeated
            if (health <= 0)
            {
                timer += Time.deltaTime;

                //If Watda is doing its stomp attack it goes back down before bull-rushing
                if (rotated)
                {
                    transform.position = transform.position;
                    transform.Rotate(0, 0, -45);
                    rotated = false;
                }
                //Slight delay before rushing
                if (timer >= 2)
                {
                    transform.position = Vector2.MoveTowards(transform.position, buildingEdge, 5 * Time.deltaTime);
                }
            }
            //If Watda still has health
            else
            {
                //Walks towards player if not doing stomp attack
                if (!rotated)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    groundTimer += Time.deltaTime; //Timer so stomp attacks have a delay between them
                }
                //If distance between player and back "foot" of Watda is < 8, Watda does stomp attack
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < 8 && !rotated && groundTimer > 3)
                {
                    transform.position = transform.position;
                    transform.Rotate(0, 0, 45);
                    rotated = true;
                    groundTimer = 0; //Resets to start stomp-attack-delay
                }
                if (rotated)
                {
                    timer += Time.deltaTime;
                    if (timer >= 1) //Slight delay before bringing "foot" down
                    {
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                        transform.Rotate(0, 0, -45);
                        transform.position = transform.position; //Freezes Watda in place after attack
                        rotated = false;
                        timer = 0;
                    }
                }

                //Turns towards Player
                if ((player.transform.position.x < transform.position.x) && facedRight)
                {
                    Flip();
                }
                else if ((player.transform.position.x > gameObject.transform.position.x) && !facedRight && health > 0)
                {
                    Flip();
                }
            }

        }
    }

    private void FixedUpdate()
    {
        //Determines if any objects are being Player and Watda
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
        }
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facedRight = !facedRight;
    }

    //Loses 10 health every time a bullet hits it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            health -= 10;
        }
    }
}
