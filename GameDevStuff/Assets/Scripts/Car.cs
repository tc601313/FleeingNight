﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Car : MonoBehaviour {

    public float speed;
    public Text countText;
    public Text winText;
    public Text loseText;
    public GameObject FrontShot;
	public GameObject BackShot;
	public GameObject Player1;
	public GameObject Player2;
    public Transform ShotSpawn;
	public Transform ShotSpawn2;
	public Transform PlayerSpawn;
	public Transform PlayerSpawn2;
    //public SpriteRenderer playerRenderer1;
    //public SpriteRenderer playerRenderer2;
    public int health = 10;
    public float fireRate;
	private int playerCountStop;
    private float nextFire;
    private int ammo = 10;
    private Rigidbody2D rb2d;
	private RigidbodyConstraints2D constraints;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerCountStop = 0;
    }

    void Update()
    {
        //Fires the shot forward
        if (Input.GetButton ("Fire1") && Time.time > nextFire && ammo > 0)
        {
            nextFire = Time.time + fireRate;
            Instantiate(FrontShot, ShotSpawn.position, ShotSpawn.rotation);
            --ammo;
            SetAmmoText();
        }
        //Fires the shot backwards
		if (Input.GetButton ("Fire3") && Time.time > nextFire && ammo > 0) {
			nextFire = Time.time + fireRate;
			Instantiate (BackShot, ShotSpawn2.position, ShotSpawn2.rotation);
			--ammo;
            SetAmmoText();
		}
        //Destroys the player if Health is below 0
        if (health <= 0)
        {
            Destroy(this.gameObject);
            GameOver();
        }

		if (Input.GetKeyDown(KeyCode.Space) && playerCountStop == 0)
		{
            //Freezes the car and makes the two player appear
            Player1.transform.position = new Vector2(PlayerSpawn.position.x, PlayerSpawn.position.y);
            Player2.transform.position = new Vector2(PlayerSpawn2.position.x, PlayerSpawn2.position.y);
            //playerRenderer1.enabled = true;
           // playerRenderer2.enabled = true;
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
			Instantiate (Player1, PlayerSpawn.position, PlayerSpawn.rotation);
			Instantiate (Player2, PlayerSpawn2.position, PlayerSpawn2.rotation);
			playerCountStop++;
		}
    }

    void FixedUpdate()
    {
        //Moves the car Vertically and Horizontally
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);

        rb2d.velocity = movement * speed;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ammo"))
        {
            other.gameObject.SetActive(false);
            ammo += 10;
            SetAmmoText();
        }
        /*else if (other.gameObject.CompareTag("Part"))
        {
            other.gameObject.SetActive(false);
        }*/
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            --health;
            if (health <= 0)
            {
                GameOver();
            }
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            health = 0;
            this.gameObject.SetActive(false);
            GameOver();
        }
    }

    //Displays Game Over when the player dies
    void GameOver()
    {
        loseText.text = "Game Over";
    }

    //Displays the amount of ammo the player has
    void SetAmmoText()
    {
        countText.text = "Ammo: " + ammo.ToString();
    }
}
