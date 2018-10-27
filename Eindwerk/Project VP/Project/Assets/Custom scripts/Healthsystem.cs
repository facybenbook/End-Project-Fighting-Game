using UnityEngine;
using System.Collections;
//[RequireComponent (typeof (PlayerController))]
public class Healthsystem : MonoBehaviour {
	private Transform myPlayer;
	public string[] enemy; // the one who deals damage to your character
	public int maxhealth;
	public float currhealth;
	public int playernumber;
	private Player2controller controller2;
	private PlayerController controller1;
	public string state;
	
	// Use this for initialization
	void Start () {
		myPlayer= this.transform;
		controller2= (Player2controller)GameObject.Find("Player2").GetComponent("Player2controller");
		controller1= (PlayerController)GameObject.Find("Player1").GetComponent("PlayerController");
	}
	
	// Update is called once per frame
	void Update () {
		state= controller1.characterstate;
	}
	
	
	void OnTriggerEnter(Collider theCollision)
	{	
		for(int i=0; i<enemy.Length;i++)
		{		
			if(playernumber==2 && theCollision.transform==GameObject.Find(enemy[i]).transform && controller2!=null && controller1.characterstate!="idle" && controller1.characterstate!="walking" && controller1.characterstate!="hurting" && controller1.characterstate!="jumping")
			{
				controller2.characterstate="hurting";
				controller2.hurtcombo=true;
				controller2.hurttimer=0;
				currhealth-=5;
				if(currhealth<=0)
				{
					currhealth=0;
				}
			}
			if(playernumber==1 && theCollision.transform==GameObject.Find(enemy[i]).transform&& controller1!=null && controller2.characterstate!="idle" && controller2.characterstate!="walking"&& controller2.characterstate!="hurting" && controller2.characterstate!="jumping")
			{
				controller1.characterstate="hurting";
				controller1.hurtcombo=true;
				controller1.hurttimer=0;
				currhealth-=5;
				if(currhealth<=0)
				{
					currhealth=0;
				}
			
			}
		}
	}
}
