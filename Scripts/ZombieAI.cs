using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    public float timer;
    public float zombieMoan;

    public bool facedRight = true;
    private bool hasLineOfSight;

    [SerializeField] private GameObject player;

    void Update()
    {
        //If Zombie game object can "see" the Player it walks towards them
        if (hasLineOfSight)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            //If Player is behind a Zombie it turns around
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
}
