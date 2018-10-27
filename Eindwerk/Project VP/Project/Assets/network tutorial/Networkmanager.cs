using UnityEngine;
using System.Collections;

public class Networkmanager : MonoBehaviour {

	private float btnX;
	private float btnY;
	private float btnW;
	private float btnH;
	private int i;
	public string gameTypeName= "Benjamin Van den Broeck Fighting Game";
	public string gameName="Fighting Game VDB";
	public string servercomment="This is a fightinggame made by Benjamin Van den Broeck";
	public int ammountofPlayers=2;
	public int listenPort= 25001;
	
	private bool refreshing=false;
	private HostData[] hostData;
	
	// Use this for initialization
	void Start () {
		btnX= Screen.width * 0.05f;
		btnY= Screen.width * 0.05f;
		btnW= Screen.width * 0.1f;
		btnH= Screen.width * 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		if(refreshing)
		{
			if(MasterServer.PollHostList().Length >0)
			{
				refreshing=false;
				Debug.Log(MasterServer.PollHostList().Length);
				hostData=MasterServer.PollHostList();
			}
		}
	}
	
	void startServer()
	{
		Network.InitializeServer(ammountofPlayers, listenPort, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameTypeName, gameName, servercomment);
	}
	
	void refreshHostList()
	{
		MasterServer.RequestHostList(gameTypeName);
		refreshing = true;
	
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server initialized");
	}
	
	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse== MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log ("Registrated server");
		}
	}
	
	void OnGUI()
	{
		if(!Network.isClient && !Network.isServer)
		{
			if(GUI.Button(new Rect(btnX,btnY,btnW,btnH), "Start server"))
			{
				Debug.Log ("Starting server");
				startServer();
			}
			if(GUI.Button(new Rect(btnX,btnY+ btnH+ 0.5f,btnW*1.2f ,btnH), "Refresh hosts"))
			{
				Debug.Log ("Refreshing hosts");
				refreshHostList();
			}
			
			if( hostData!=null)
			{
				for(i=0; i<hostData.Length;i++)
				{
					if(GUI.Button(new Rect(btnX *2.0f+ btnW,btnY*1.2f+ (btnH*i), btnW*3.0f, btnH ),hostData[i].gameName))
					{
						Network.Connect(hostData[i]);
					}
				}
			}
		}
	}
}
