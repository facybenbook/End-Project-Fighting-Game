using UnityEngine;
using System.Collections;

public class Menucontrol : MonoBehaviour {
	
	public int ammount;// Must be the same as the size as your targets
	private Transform thisIndicator;
	public Transform[] targets;
	private int i=0;
	private float targetposition;
	private float smooth = 3.0f;
	
	// Use this for initialization
	void Start () {
		Time.timeScale=1;
		thisIndicator=this.transform;
		thisIndicator.position= new Vector3(thisIndicator.position.x,targets[i].position.y,thisIndicator.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
		float h= Input.GetAxisRaw("Vertical1");
		if(h>0 && i!= 0 && (thisIndicator.position.y>=targets[i].position.y-0.05f && thisIndicator.position.y<=targets[i].position.y+0.05f))
		{
			
			i--;
		}
		else
		{
			if(h<0 && i!=ammount-1 && (thisIndicator.position.y>=targets[i].position.y-0.05f && thisIndicator.position.y<=targets[i].position.y+0.05f))
			{
				i++;
			}
		}
		
		targetposition= Mathf.Lerp(thisIndicator.position.y,targets[i].position.y,smooth*Time.deltaTime);
		thisIndicator.position= new Vector3(thisIndicator.position.x,targetposition,thisIndicator.position.z);
		
		if(Input.GetButtonDown("Punch1"))
		{
			switch(i)
			{
			case 0:
				Application.LoadLevel("fightscene");
				break;
			case 1:
				Application.LoadLevel("Instructions");
				break;
			case 2:
				
				break;
			}
		}
	}
}
