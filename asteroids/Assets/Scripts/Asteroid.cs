using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Asteroid : MonoBehaviour
{
    public ParticleSystem explosion_prefab_;
    public GameObject asteroid_prefab_;
    
    private int type_;
    private int size_ = 3; //3= Big, 2 = Medium, 1 = Small;
    private ParticleSystem explosion_instance_;
    private bool is_alive_;


    public bool IsAlive()
    {
        return is_alive_;
    }

    public void SetAsteroidSize(int size)
    {
        size_ = size;
        switch (size_)
        {
            case 1:
                transform.localScale = new Vector3(0.15f, 0.15f, 1.0f);
                break;
            case 2:
                transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
                break;
            case 3:
                transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
                break;
        }
    }

    // Use this for initialization
    void Start()
    {
        is_alive_ = true;
        int type = Random.Range(1, 5);
        Camera.main.GetComponent<AsteroidRenderer>().AddObject(this, type);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        explosion_instance_ = (ParticleSystem)Instantiate(explosion_prefab_, transform.position, transform.rotation);   
        explosion_instance_.enableEmission = true;
        gameObject.GetComponent<AudioSource>().Play();
        
        int score = 0;
        switch (size_)
        {
            case 1:
                score = 100;
                explosion_instance_.startSize = 0.08f;
                explosion_instance_.startLifetime = 2;
                break;
            case 2:
                score = 50;
                explosion_instance_.startSize = 0.1f;
                explosion_instance_.startLifetime = 5;
                break;
            case 3:
                score = 20;
                explosion_instance_.startSize = 0.2f;
                explosion_instance_.startLifetime = 7;
                break;
        }
        if (c.gameObject.tag == "PlayerProjectile" || c.gameObject.tag == "Player")
        {
            GameObject.Find("GameController").GetComponent<GameController>().AddScore(score);
        }
        if (size_ > 1)
        {
            GameObject child_asteroid_1 = (GameObject)Instantiate(asteroid_prefab_, transform.position, Quaternion.identity);
            child_asteroid_1.GetComponent<Asteroid>().SetAsteroidSize(size_ - 1);
            child_asteroid_1.rigidbody2D.velocity = Quaternion.Euler(0, 0, -30) * rigidbody2D.velocity;
            GameObject child_asteroid_2 = (GameObject)Instantiate(asteroid_prefab_, transform.position, Quaternion.identity);
            child_asteroid_2.GetComponent<Asteroid>().SetAsteroidSize(size_ - 1);
            child_asteroid_2.rigidbody2D.velocity = Quaternion.Euler(0, 0, 30) * rigidbody2D.velocity;


        }
        //This has to be disabled because the object will only be destroyed
        //after the sound effect is finished
        gameObject.GetComponent<BoxCollider2D>().enabled = false;            
        Destroy(gameObject, gameObject.GetComponent<AudioSource>().clip.length);
        is_alive_ = false;
    }   
}
