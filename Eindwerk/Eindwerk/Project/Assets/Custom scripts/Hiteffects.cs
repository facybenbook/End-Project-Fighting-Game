using UnityEngine;
using System.Collections;

public class Hiteffects : MonoBehaviour {
	
	public ParticleSystem particle;

	// Use this for initialization
	void Start () {
	particle=(ParticleSystem)this.gameObject.GetComponent("ParticleSystem");
	}
	
	// Update is called once per frame
	void Update () {
		if(particle.isStopped==true)
		{
			Destroy(this.gameObject);
		}
	}
}
