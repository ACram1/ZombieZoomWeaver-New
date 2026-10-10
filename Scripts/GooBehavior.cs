using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class GooBehavior : MonoBehaviour
{
    private float bulletSpeed = 8f;
    private GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        //Gets Goos rigidbody and identifies the Player
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        //Travels towards the Player's position when it was shot
        Vector3 direction = player.transform.position - transform.position;
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * bulletSpeed;

        //Rotates the object so it has a proper rotation depending on the angle it's shot at, not important anymore but originally has gonna have extra "goo" trailing the big one
        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);

        //Destroys Goo after 4 seconds
        Destroy(gameObject, 4f);
        
    }
}
