using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {
	
	private GameObject Player1;
	private GameObject Player2;
	private Healthsystem x1;
	private Healthsystem x2;
	private GUI box1;
	private GUI box2;
	public Texture2D bgImage;
	public Texture2D fgImage;
	public string menuscene;
	private bool end=false;
	// Use this for initialization
	void Start () {
		Player1= GameObject.Find("Player1");
		Player2= GameObject.Find("Player2");
		x1=(Healthsystem)Player1.GetComponent("Healthsystem");
		x2=(Healthsystem)Player2.GetComponent("Healthsystem");
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
		if(x1.currhealth ==0)
		{
			GUI.Box(new Rect((Screen.width/2)-50,(Screen.height/2)-60, 100,20), "Player 2 wins!");
			end=true;
		}
		else
		{
			if(x2.currhealth ==0)
			{
				GUI.Box(new Rect((Screen.width/2)-50,(Screen.height/2)-60, 100,20), "Player 1 wins!");
				end=true;
			}
		}
		
			// Adjust the first 2 coordinates to place it somewhere else on-screen
		GUI.BeginGroup (new Rect (0,0,400,32));

		// Draw the background image
		GUI.Box (new Rect (-10,0,400,32), bgImage);

			// Create a second Group which will be clipped
			// We want to clip the image and not scale it, which is why we need the second Group
		GUI.BeginGroup (new Rect (0,0,x1.currhealth/100 * 400, 32));

			// Draw the foreground image
			GUI.Box (new Rect (-10,0,400,32), fgImage);


			// End both Groups
			GUI.EndGroup ();

		GUI.EndGroup ();

		GUI.BeginGroup (new Rect (Screen.width-390,0,400,32));

		// Draw the background image
		GUI.Box (new Rect (-10,0,400,32), bgImage);

			// Create a second Group which will be clipped
			// We want to clip the image and not scale it, which is why we need the second Group
		GUI.BeginGroup (new Rect (0,0,x2.currhealth/100 * 400, 32));

		// Draw the foreground image
		GUI.Box (new Rect (-10,0,400,32), fgImage);

		// End both Groups
		GUI.EndGroup ();

		GUI.EndGroup ();
		
	}
}
