using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {
	
	public Healthsystem x1;
	public Healthsystem x2;
	public string menuscene;
	public GUIStyle[] style;
	private bool end=false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(end)
		{
			Time.timeScale=0;
			System.Threading.Thread.Sleep(2000);
			Application.LoadLevel(menuscene);
			}
	}
		
	void OnGUI()
	{
	   /*GUI.Box(new Rect (10,10,Screen.width-20,20), "Player 1: " + x1.currhealth.ToString() + "/"+ x1.maxhealth.ToString());
	    GUI.Box(new Rect (10,40,Screen.width-20,20), "Player 2: " + x2.currhealth.ToString() + "/"+ x2.maxhealth.ToString());
		*/
		if( x1!=null && x2!=null)
		{
			if(x1.currhealth ==0)
			{
				GUI.Box(new Rect((Screen.width/2)-125,(Screen.height/2)-61,305 ,122), x2.name+" wins!",style[8]);
				end=true;
			}
			else
			{
				if(x2.currhealth ==0)
				{
					GUI.Box(new Rect((Screen.width/2)-125,(Screen.height/2)-61,305 ,122), x1.name+" wins!",style[8]);
					end=true;
				}
			}
			
			//
			// Player 1 Health
			//
			
			GUI.Label(new Rect(30,10, 100,100), x1.name,style[7]);
			
				// Adjust the first 2 coordinates to place it somewhere else on-screen
			GUI.BeginGroup (new Rect (10,30,400,32),style[0]);
			// Draw the background image
			GUI.Box (new Rect (20,5,350,20), "",style[1]);
				// Create a second Group which will be clipped
				// We want to clip the image and not scale it, which is why we need the second Group
			GUI.BeginGroup (new Rect (0,0,x1.currhealth/x1.maxhealth * 400, 32),style[5]);
				// Draw the foreground image
				GUI.Box (new Rect (20,5,350,20),"", style[2]);
				// End both Groups
				GUI.EndGroup ();
			GUI.EndGroup ();
			//
			// Player 1 Special
			//
			
			GUI.BeginGroup (new Rect (20,70,100,16),style[0]);
			GUI.Box (new Rect(5,4,85,6),"", style[3]);
			GUI.BeginGroup (new Rect (0,0,x1.currSpecial/x1.maxSpecial * 100, 16),style[5]);
				GUI.Box (new Rect (5,4,85,6),"",style[4]);
				GUI.EndGroup ();
			GUI.EndGroup ();
			
			if(x1.currSpecial==x1.maxSpecial)
			{
				GUI.Box (new Rect(130,70,20,20),"",style[6]);
			}
			
			//
			// Player 2 Health
			//
			GUI.Label(new Rect(Screen.width-(x2.name.Length*10)-40 ,10, 100,100), x2.name,style[7]);
			GUI.BeginGroup (new Rect (Screen.width-420,30,400,32),style[0]);
			GUI.Box (new Rect (20,5,350,20),"",  style[1]);
			GUI.BeginGroup (new Rect (0,0,x2.currhealth/x2.maxhealth * 400, 32),style[5]);
				GUI.Box (new Rect (20,5,350,20),"", style[2]);
				GUI.EndGroup ();
			GUI.EndGroup ();
			
			//
			// Player 2 Special
 			//
			
			GUI.BeginGroup (new Rect (Screen.width-140,70,100,16),style[0]);
			GUI.Box (new Rect (5,4,85,6), "",style[3]);
			GUI.BeginGroup (new Rect (0,0,x2.currSpecial/x2.maxSpecial * 100, 16),style[5]);
				GUI.Box (new Rect (5,4,85,6),"", style[4]);
				GUI.EndGroup ();
			GUI.EndGroup ();
			
			if(x2.currSpecial==x2.maxSpecial)
			{
				GUI.Box (new Rect(Screen.width-170,70,20,20),"",style[6]);
			}
		}
		
	}
}
