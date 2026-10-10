using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private float bulletSpeed = 20f;

    void Start()
    {
        Destroy(gameObject, 4f);  //Destroys bullet after 4 seconds
    }

    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;  // Updates position every frame
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        if(collision.CompareTag("Zombie"))
        {
            Destroy(collision.gameObject);

        }
    }
}
