using UnityEngine;
using System.Collections;

public class specialsmoke : MonoBehaviour {
	
	public PlayerController V42;
	public string state;
	public ParticleSystem particle;
	
	// Use this for initialization
	void Start () {
	particle=(ParticleSystem)this.gameObject.GetComponent("ParticleSystem");
	}
	
	// Update is called once per frame
	void Update () {
	state=V42.characterstate;
	if(state=="special")
		{
			particle.startSpeed=3;
			particle.startSize=20;
			particle.emissionRate=800;
		}
		else
		{
			particle.startSpeed=0.512121f;
			particle.startSize=1;
			particle.emissionRate=20;
		}
	}
}
