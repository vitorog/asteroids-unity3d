using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float max_time_alive_; 
    private float time_alive_;

    // Use this for initialization
    void Start()
    {
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
}
