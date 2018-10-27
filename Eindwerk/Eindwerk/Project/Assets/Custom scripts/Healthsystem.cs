using UnityEngine;
using System.Collections;
//[RequireComponent (typeof (PlayerController))]
public class Healthsystem : MonoBehaviour {
	public string[] enemy; // the one who deals damage to your character
	public int maxhealth;
	public float currhealth;
	public int maxSpecial;
	public float currSpecial;
	public int playernumber;
	public PlayerController controller2;
	public PlayerController controller1;
	public Healthsystem player1;
	public Healthsystem player2;
	public string state;
	public AudioClip SoundP;
	public AudioClip SoundS;
	public string Opponentname;
	public int damage;
	public int specialDamage;
	public string name;
	//public GameObject particle;
	public GameObject particleeffectP; 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(playernumber==1 && controller1!=null)
		{
			state = controller1.characterstate;
		}
		else
		{
			if(controller2!=null)
			{
				state = controller2.characterstate;
			}
		}
	}
	
	
	void OnTriggerEnter(Collider theCollision)
	{	if(networkView.isMine)
		{
			for(int i=0; i<enemy.Length;i++)
			{		
				if(playernumber==2 && theCollision.transform==GameObject.Find(enemy[i]).transform && controller1!=null && controller1.characterstate!="idle" && controller1.characterstate!="walking" && controller1.characterstate!="hurting" && controller1.characterstate!="jumping" && controller1.characterstate!="guarding" && controller2.characterstate!="hurting")
				{
					if(controller2.characterstate=="guarding")
					{	
						networkView.RPC("AdjustSpecial",RPCMode.AllBuffered,1,3);
						networkView.RPC("AdjustSpecial",RPCMode.AllBuffered,2,-3);
						//AudioSource.PlayClipAtPoint(SoundP,GameObject.Find(enemy[i]).transform.position,0.5f);	
	
					}
					else
					{
						Debug.Log ("Hit");
						controller2.characterstate="hurting";
						if(controller1.characterstate=="special")
						{
							networkView.RPC("AdjustHealth",RPCMode.AllBuffered,2, 15, true, 0,i,theCollision.transform.position);
							//AudioSource.PlayClipAtPoint(SoundS,GameObject.Find(enemy[i]).transform.position,0.5f);
						}
						else
						{
							networkView.RPC("AdjustHealth",RPCMode.AllBuffered,2, 5, true, 0,i,theCollision.transform.position);
							networkView.RPC("AdjustSpecial",RPCMode.AllBuffered,1,5);
							//AudioSource.PlayClipAtPoint(SoundP,GameObject.Find(enemy[i]).transform.position,0.5f);
						}
					}
				}
				else
				{
					if(playernumber==1 && theCollision.transform==GameObject.Find(enemy[i]).transform && controller2!=null && controller2.characterstate!="idle" && controller2.characterstate!="walking"&& controller2.characterstate!="hurting" && controller2.characterstate!="jumping" && controller2.characterstate!="guarding" && controller1.characterstate!="hurting" )
					{
						if(controller1.characterstate=="guarding")
						{	
							networkView.RPC("AdjustSpecial",RPCMode.AllBuffered,2,damage-2);
							networkView.RPC("AdjustSpecial",RPCMode.AllBuffered,1,-(damage-2));
							//AudioSource.PlayClipAtPoint(SoundP,GameObject.Find(enemy[i]).transform.position,0.5f);
						}
						else
						{	
							Debug.Log ("Hit");
							controller1.characterstate="hurting";
							if(controller2.characterstate=="special")
							{
								networkView.RPC("AdjustHealth",RPCMode.AllBuffered,1, specialDamage, true, 0,i,theCollision.transform.position);
								//AudioSource.PlayClipAtPoint(SoundS,GameObject.Find(enemy[i]).transform.position,0.5f);
							}
							else
							{
								networkView.RPC("AdjustHealth",RPCMode.AllBuffered,1, damage, true, 0,i,theCollision.transform.position);
								networkView.RPC("AdjustSpecial",RPCMode.AllBuffered,2,damage);
								//AudioSource.PlayClipAtPoint(SoundP,GameObject.Findx(enemy[i]).transform.position,0.5f);
							}
						}
					}
				}
			}
		}
	}
	
	[RPC]
	void AdjustHealth(int playernumber, int damage, bool hurtcombo, int timer, int i, Vector3 position)
	{
		if(playernumber==1)
		{
			controller1.hurtcombo=hurtcombo;
			controller1.hurttimer=timer;
		}
		else
		{
			if(playernumber==2)
			{
				controller2.hurtcombo=hurtcombo;
				controller2.hurttimer=timer;
			}
		}
		if(controller2.characterstate=="special"||controller1.characterstate=="special")
		{
			if(Opponentname=="Frederica")
				AudioSource.PlayClipAtPoint(SoundS,GameObject.Find(enemy[i]).transform.position,0.4f);	
		}
		else
		{
			AudioSource.PlayClipAtPoint(SoundP,GameObject.Find(enemy[i]).transform.position,0.4f);
			Instantiate(particleeffectP,position,Quaternion.identity);
		}
		currhealth-=damage;
		if(currhealth<=0)
		{
			currhealth=0;
		}
	}
	
	
	[RPC]
	void AdjustSpecial(int playernumber, int special )
	{
		if(playernumber==1)
		{
			player1.currSpecial+=special;
			if(player1.currSpecial>=player1.maxSpecial)
			{
				player1.currSpecial=player1.maxSpecial;
			}
		}
		else
		{
			player2.currSpecial+=special;
			if(player2.currSpecial>=player2.maxSpecial)
			{
				player2.currSpecial=player2.maxSpecial;
			}
		}
	}
}
