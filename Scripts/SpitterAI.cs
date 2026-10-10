using UnityEngine;
using System.Collections;

public class SpitterAI : MonoBehaviour
{
    private float timer;
    public bool facedRight = true;
    public bool firstSight = false;
    private bool hasLineOfSight;

    [SerializeField] private GameObject player;
    public Transform LaunchOffset;
    public GameObject gooPrefab;


    //Very similar to regular Zombie

    void Update()
    {
        //Distance between Spitter and Player
        float distance = Vector2.Distance(transform.position, player.transform.position);

        //If Spitter game object can "see" the Player
        if (hasLineOfSight)
        {
            if (!firstSight) //If this is the first time a Spitter has seen the Player, wait a small amount the "spit"
            {
                timer += Time.deltaTime;
                if (timer > 0.5)
                {
                    shoot();
                    firstSight = true;
                    timer = 0;
                }
            }
            //Spit at Player within this range
            if (distance < 20)
            {
                timer += Time.deltaTime;
                if (timer > 3)
                {
                    shoot();
                    timer = 0;
                }
            }
            //Turns to face Player if behind the Spitter
            if ((player.transform.position.x < transform.position.x) && facedRight)
            {
                Flip();
            }
            else if ((player.transform.position.x > gameObject.transform.position.x) && !facedRight)
            {
                Flip();
            }

        }
    }

    private void FixedUpdate()
    {
        //Determines if any object is in between a Zombie and the Player
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

    //Creates Goo spit
    void shoot()
    {
        Instantiate(gooPrefab, LaunchOffset.position, Quaternion.identity);
    }
}
