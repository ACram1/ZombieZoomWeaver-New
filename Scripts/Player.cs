using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static int _ammo = 10;
    public int playerLayer, obstacleLayer, raycastLayer;

    [SerializeField] private float moveSpeed = 7.5f;
    [SerializeField] private float jumpForce = 9f;
    public float groundCheckRadius = 0.1f;

    private bool isGrounded;
    private bool onObstacle;
    private bool onRaycast;
    public bool facedRight = true;
    public bool atDoor;
    public bool touchZom;

    public Transform LaunchOffset;
    public Transform groundCheck;

    private Rigidbody2D rb;

    public BulletBehaviour bulletPrefab;
    public CinemachineCamera vcam;
    [SerializeField] private UIManager _uiManager;
    public SoundEffectsPlayer src;
    public Collider2D coll;
    

    public LayerMask groundLayer;
    public LayerMask obstacles;
    public LayerMask raycast;


    void Start()
    {
        //Display ammo amount, get player collider and rigidbody, get layers player will interact with
        _uiManager.UpdateAmmo(_ammo);
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Default");
        obstacleLayer = LayerMask.NameToLayer("Obstacles");
        raycastLayer = LayerMask.NameToLayer("Ignore Raycast");
    }

    void Update()
    {
        //Determines what the player is standing on
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        onObstacle = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, obstacles);
        onRaycast = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, raycast);

        //Which way the player is moving (right = +, Left = -) and movement speed
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        //If player is falling let them land on obstacles and if they are not in the air they ignore obstacles
        if (rb.linearVelocityY <= -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, false);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerLayer, obstacleLayer, true);
        }

        //Changing x movement changes which way Player is facing
        if (moveInput > 0 && facedRight == false && !touchZom)
        {
            Flip();
        }
        else if (moveInput < 0 && facedRight == true && !touchZom)
        {
            Flip();
        }

        //Player can only jump if they are alive and standing one of these layers
        if (Input.GetKeyDown(KeyCode.W) && (isGrounded || onObstacle || onRaycast) && !touchZom)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            if (Input.GetKeyDown(KeyCode.Space) && _ammo > 0)
            {
                src.ShootSounds();                 //Prioritzes shooting sound over jumpin sound
            }
            else
            {
                src.JumpSounds();
            }

        }

        //Shoot a bullet and decrease ammo amount by 1 and display new ammount
        if (Input.GetKeyDown(KeyCode.Space) && _ammo > 0 && !touchZom)
        {
            src.ShootSounds();
            Instantiate(bulletPrefab, LaunchOffset.position, transform.rotation);
            --_ammo;
            _uiManager.UpdateAmmo(_ammo);
        }

        //Teleports player to the Watda boss fight at the end of the game
        if (Input.GetKeyDown(KeyCode.E) && atDoor && !touchZom)
        {
            transform.position = new Vector3(5, 24, 0);

            Vector3 offset = new Vector3(0, 1, 0);
            Vector3 pos = transform.position - vcam.transform.position;
            pos.z = 0;
            vcam.OnTargetObjectWarped(transform, pos + offset);
            vcam.Lens.OrthographicSize = 10;
            atDoor = false;
        }
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facedRight = !facedRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if Player touches zombie, ammo and facedRight are reset and they are frozen in place, takes player to death screen through Invoke
        if (collision.gameObject.tag == "Zombie")
        {
            Rigidbody2D rbz = collision.gameObject.GetComponent<Rigidbody2D>();
            rbz.constraints = RigidbodyConstraints2D.FreezePositionX;
            facedRight = true;
            _ammo = 10;
            touchZom = true;
            src.DeathSound();
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
            coll.enabled = false;
            Invoke(nameof(DelayDeathScreen), 5f);
        }

        //When Player picks up ammo
        if (collision.gameObject.tag == "Ammo")
        {
            Destroy(collision.gameObject);
            _ammo += 10;
            _uiManager.UpdateAmmo(_ammo);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If Player is hit by Goo ball, ammo and facedRight are reset and they are frozen in place, takes player to death screen through Invoke 
        if (collision.gameObject.tag == "Zombie")
        {
            facedRight = true;
            _ammo = 10;
            touchZom = true;
            src.DeathSound();
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY
                         | RigidbodyConstraints2D.FreezeRotation;
            coll.enabled = false;
            Invoke(nameof(DelayDeathScreen), 5f);
        }

        //Constant check if Player is currently at Boss door so they can't reach the door, backtrack the press E somewhere else
        if (collision.gameObject.tag != "Door")
        {
            atDoor = false;
        }
        if (collision.gameObject.tag == "Door")
        {
            atDoor = true;
        }
        
        //Loads Escaped screen
        if (collision.gameObject.tag == "Escape")
        {
            SceneManager.LoadScene(5);
        }
    }

    //Delays destruction of Player object until player reaches death screen
    private void DelayDeathScreen()
    {
        SceneManager.LoadScene(4);
        Destroy(gameObject);
    }
}
