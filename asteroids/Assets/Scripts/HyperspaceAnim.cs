using UnityEngine;
using System.Collections;

public class HyperspaceAnim : MonoBehaviour {
    private int type_;
    public void Init(int type)
    {
        type_ = type;
        if (type_ == 0)
        {
            InvokeRepeating("Shrink", 0.0f, 0.01f);
        }
        else
        {            
            transform.localScale = new Vector3(0.00015f, 0.00025f, 1.0f);            
            InvokeRepeating("Grow", 1.0f, 0.02f);
            GetComponent<ParticleSystem>().enableEmission = false;
            GetComponent<PlayerShipRenderer>().enabled = false;
        }
    }

	// Use this for initialization
	void Start () {        
	}    
	
	// Update is called once per frame
	void Update () {
        if (type_ == 0)
        {
            if (transform.localScale.x < 0.015f || transform.localScale.y < 0.025f)
            {                
                CancelInvoke("Shrink");
                Destroy(gameObject, 2.0f);
            }
        }
        else
        {
            if (transform.localScale.x >= 0.15f || transform.localScale.y >= 0.25f)
            {
                CancelInvoke("Grow");
                transform.localScale = new Vector3(0.15f, 0.25f, 1.0f);                
                Destroy(gameObject,0.25f);
            }
        }
	}

    void Shrink()
    {
        if (transform.localScale.x > 0.015f || transform.localScale.y > 0.025f)
        {
            transform.localScale = transform.localScale / 1.2f;
        }
    }

    void Grow()
    {
        if (!GetComponent<PlayerShipRenderer>().enabled)
        {
            GetComponent<PlayerShipRenderer>().enabled = true;
        }
        transform.localScale = transform.localScale * 1.2f;               
    }
}
