using UnityEngine;
using System.Collections;

public class DestroyParticleSystem : MonoBehaviour {
    void Awake()
    {
        
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //If there are no more particles...
        if (!this.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
