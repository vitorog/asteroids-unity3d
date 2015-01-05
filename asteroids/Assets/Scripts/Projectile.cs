using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float max_time_alive_; 
    private float time_alive_;

    private bool is_alive_;

    // Use this for initialization
    void Start()
    {
        is_alive_ = true;
        Camera.main.GetComponent<ProjectileRenderer>().AddProjectile(this);
    }

    // Update is called once per frame
    void Update()
    {        
        time_alive_ += Time.deltaTime;
        if (time_alive_ > max_time_alive_)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        is_alive_ = false;
        
    }

    //Moved destruction code to LateUpdate function because it seems there is a problem 
    //when handling collision between objects: if each object destroys itself, 
    //sometimes only one of the OnCollisionEnter is called (Unity bug?)
    //Not sure if it will work in all cases and could not find references...
    void LateUpdate()
    {
        if (!is_alive_)
        {
            Destroy(gameObject);
        }
    }
}
