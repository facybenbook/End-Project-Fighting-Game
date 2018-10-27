using UnityEngine;
using System.Collections;

public class Networkmanager : MonoBehaviour {
	
	public float btnX;
	public float btnY;
	public float btnW;
	public float btnH;
	private int i;
	private int j=0;
	public string gameTypeName= "Benjamin Van den Broeck Fighting Game";
	public string gameName="Fighting Game VDB";
	public string servercomment="This is a fightinggame made by Benjamin Van den Broeck";
	public int ammountofPlayers=1;
	public string listenPort= "25001";
	public string serverIp="";
	public GameObject playerPrefab;
	public Transform spawnObject;
	public Transform spawnObject2;
	public bool Player2connected=false;
	private Object player1Object;
	private Object player2Object;
	public PlayerController player1;
	public PlayerController player2;
	public Healthsystem player1Health;
	public Healthsystem player2Health;
	public Healthbar healthBar;
	private CameraFightScene camera;
	public string[] enemy;
	public string[] scenes;
	public Characters[] chars;
	private GameObject thisGameObject;
	private bool choosecharacter=false;
	private int playersplaying=0;
	public GUIStyle[] styles;
	private string buttons= "Start Server";
	private string x;
	private string checking;
	private string opponent;
	
	private bool refreshing=false;
	private bool fillindata=false;
	private bool fillinjoindata=false;
	private bool instructions=false;
	private bool noGames=false;
	public bool connectionfail=false;
	private HostData[] hostData;
	public AudioClip clicksound;
	
	public string playername= "Player name";
	
	// Use this for initialization
	void Start () {
		Time.timeScale=1;
		thisGameObject= transform.gameObject;
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
		if(Player2connected)
		{
			if(Network.isServer)
			{
				player2=(PlayerController)GameObject.Find ("Opponent(Clone)").GetComponent("PlayerController");
				player2Health=(Healthsystem)GameObject.Find("Opponent(Clone)").GetComponent("Healthsystem");
		
				player1.Opponent= GameObject.Find ("Opponent(Clone)").transform;
				player1.canControl=true;
				player1Health.controller2=player2;
				player1Health.player2=player2Health;
				player1Health.Opponentname=player2.name;
				player2.Opponent= GameObject.Find ("Player").transform;
				player2.canControl=true;
				player2.health=player2Health;
				player2Health.playernumber=2;
				player2Health.controller1=player1;
				player2Health.controller2=player2;
				player2Health.player1=player1Health;
				player2Health.player2=player2Health;
				player2Health.Opponentname=player1.name;
				changeDamage(GameObject.Find("Opponent(Clone)"));
				for(int i=0; i< player1Health.enemy.Length;i++)
					{
							player1Health.enemy[i]=player1Health.enemy[i]+ "O";
					}
				healthBar.x2=player2Health;
				camera.player2=GameObject.Find("Opponent(Clone)").transform;
				//networkView.RPC("changePlayers",RPCMode.AllBuffered,-2);
			}
			else
			{
				player1=(PlayerController)GameObject.Find ("Opponent(Clone)").GetComponent("PlayerController");
				player1Health=(Healthsystem)GameObject.Find("Opponent(Clone)").GetComponent("Healthsystem");
				
				player2.Opponent= GameObject.Find ("Opponent(Clone)").transform;
				player2.canControl=true;
				player2Health.controller1=player1;
				player2Health.player1=player1Health;
				player1.Opponent=GameObject.Find ("Player2").transform;
				player1.canControl=true;
				player1.health=player1Health;
				player1Health.playernumber=1;
				player1Health.controller1=player1;
				player1Health.controller2=player2;
				player1Health.player1=player1Health;
				player1Health.player2=player2Health;
				changeDamage(GameObject.Find ("Opponent(Clone)"));
				for(int i=0; i< player2Health.enemy.Length;i++)
					{
						player2Health.enemy[i]=player2Health.enemy[i]+ "O";
					}
				healthBar.x1=player1Health;
				camera.player1=GameObject.Find("Opponent(Clone)").transform;
				//networkView.RPC("changePlayers",RPCMode.AllBuffered,-2);
			}
			Player2connected=false;
		}
		
		/*if(playersplaying >= -2 && playersplaying<0)
		{
			if(Network.isServer)
			{
				networkView.RPC("sendPlayerInfo",RPCMode.AllBuffered,1,playername);
			}
			else
			{
				networkView.RPC("sendPlayerInfo",RPCMode.AllBuffered,2,playername);
			}
			networkView.RPC("changePlayers",RPCMode.AllBuffered,1);
		}*/
		
		if(Network.isServer && player1Health!=null && player2Health!=null && j <=1)
		{
			networkView.RPC("sendPlayerInfo",RPCMode.AllBuffered,1,playername);
			j++;
		}
		
		if(Network.isClient && player2Health!=null && player1Health!=null && j <=1)
		{
			networkView.RPC("sendPlayerInfo",RPCMode.AllBuffered,2,playername);
			j++;
		}
		
		DontDestroyOnLoad(thisGameObject);
	}
	
	void startServer()
	{
		if(serverIp!="")
		{
			MasterServer.ipAddress=serverIp;
		}
		Network.InitializeServer(ammountofPlayers, int.Parse(listenPort), !Network.HavePublicAddress());
		if(serverIp!="")
		{
			Network.natFacilitatorIP="10.0.0.12";
		}	
		MasterServer.RegisterHost(gameTypeName, gameName, servercomment);
	checking=MasterServer.ipAddress;
	}
	
	void refreshHostList()
	{
		
		if(serverIp!="")
		{
			MasterServer.ipAddress=serverIp;
			Network.natFacilitatorIP="10.0.0.12";
		}
		MasterServer.ClearHostList();
		MasterServer.RequestHostList(gameTypeName);
		refreshing = true;
		Debug.Log ("Refreshing");
	
	}
	
	
	//functions
	void OnMasterServerEvent(MasterServerEvent mse)
	{
		if(mse== MasterServerEvent.RegistrationSucceeded)
		{
			Debug.Log ("Registrated server");
		}
	}
	
	void OnServerInitialized()
	{
		Debug.Log("Server initialized");
	connectionfail=false;
	}
	
	void changeLayer(GameObject g)
	{
		foreach(Transform t in g.transform)
		{
		t.gameObject.layer=LayerMask.NameToLayer("LayerSelf");
		changeLayer(t.gameObject);
		}
	}
				
	void changeDamage(GameObject g)
	{		
		foreach(Transform t in g.transform)
		{
			for(int i=0; i<enemy.Length;i++)
			{		
				if(t.gameObject.name== enemy[i])
				{
					t.gameObject.name=t.gameObject.name+"O";
				}
			}
			changeDamage(t.gameObject);
		}
	}
	
	void OnLevelWasLoaded(int level)
	{
		if(level==0)
		{
		Network.Disconnect();
		MasterServer.UnregisterHost();
		Destroy(thisGameObject);
		}
	}
	
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if(connectionfail==false)
		Application.LoadLevel(0);
	}
	
	void OnPlayerDisconnected(NetworkPlayer player)
	{
		if(connectionfail==false)
		{
		if(Application.loadedLevel!=0)
			Application.LoadLevel(0);
		}
	}
	
	void OnFailedToConnect(NetworkConnectionError Error)
	{
		Debug.Log (Error);
		connectionfail=true;
	}
	
	void OnFailedToConnectToMasterServer(NetworkConnectionError Error)
	{
		Debug.Log (Error);
		connectionfail=true;
	}
	
	// Buttons
	void OnGUI()
	{
		if(!Network.isClient && !Network.isServer && hostData==null && fillindata==false && instructions==false && fillinjoindata==false)
		{
			
			if(GUI.Button(new Rect(Screen.width/2-btnX,btnY,btnW,btnH),"Create Game",styles[0]))
			{
				audio.PlayOneShot(clicksound);
				fillindata=true;
			}
			if(GUI.Button(new Rect(Screen.width/2-btnX,btnY+ btnH+ 32.0f, btnW ,btnH), "Join Game",styles[0]))
			{
				Debug.Log ("Refreshing hosts");
				audio.PlayOneShot(clicksound);
				fillinjoindata=true;
			}
			if(GUI.Button(new Rect(Screen.width/2-btnX,btnY+ btnH*2+ 32.0f*2, btnW ,btnH), "Instructions",styles[0]))
			{
				instructions=true;
				audio.PlayOneShot(clicksound);
			}
			if(GUI.Button(new Rect(Screen.width/2-btnX,btnY+ btnH*3+ 32.0f*3, btnW ,btnH), "Quit",styles[0]))
			{
				audio.PlayOneShot(clicksound);
				Application.Quit();
			}
			GUI.Label(new Rect(Screen.width/2-120.0f,Screen.height-btnH-12.0f,240.0f,btnH),"Made by Benjamin Van den Broeck");
		}
		
		if(instructions==true)
		{
			
			GUI.Box(new Rect(Screen.width/2, btnY-50.0f,150.0f,100.0f),"",styles[1]);
			GUI.TextArea(new Rect(Screen.width/2-200.0f, btnY-50.0f,175.0f,100.0f),"A = Punching \nD = Kicking \nS = Guard (only if you have special meter) \nW = Special attack (only if special meter is full)");
			GUI.Box(new Rect(Screen.width/2-200.0f, btnY+100.0f,150.0f,100.0f),"",styles[2]);
			GUI.TextArea(new Rect(Screen.width/2,btnY+120.0f,185.0f,60.0f), "Up Arrow = Jumping \nLeft Arrow = Walk to the left \nRight Arrow = Walk to the right"); 
			
			if(GUI.Button(new Rect(Screen.width/2-btnW/2,Screen.height-btnH-12.0f,btnW,btnH),"Back",styles[0]))
				{
					audio.PlayOneShot(clicksound);
					instructions=false;
				}	
		}
		
		if( hostData!=null && Network.connections.Length<1)
		{
			connectionfail=false;
			fillinjoindata=false;
			noGames=false;
			GUI.Label(new Rect(Screen.width/2-btnW/2, btnY-32.0f,btnW,btnH+12.0f),"",styles[6]);
			for(i=0; i<hostData.Length;i++)
			{
				if(GUI.Button(new Rect(Screen.width/2-btnW/4 ,btnY-32.0f+32.0f*i+ (btnH*i) +(btnH+42.0f), btnW/2, btnH ),hostData[i].gameName))
				{
					audio.PlayOneShot(clicksound);
					Network.Connect(hostData[i].ip,hostData[i].port);
				}
			}
			if(GUI.Button(new Rect(Screen.width/2-btnW/2,Screen.height-btnH-12.0f,btnW,btnH),"Back",styles[0]))
			{
				audio.PlayOneShot(clicksound);
				hostData=null;
			}
			if(connectionfail==true)
			{
				GUI.Box(new Rect((Screen.width/2)-(btnW/2)+20.0f,Screen.height-btnH-60.0f, btnW-40.0f,btnH), "Cannot connect to Server",styles[3]);
			}
		}
		
		if(fillinjoindata==true)
		{
			GUI.Label(new Rect(Screen.width/2-btnW/2,btnY+btnH+24.0f,btnW,btnH),"Location of Server:");
			serverIp= GUI.TextField(new Rect(Screen.width/2,btnY+btnH+24.0f,btnW/2,btnH),serverIp);
			GUI.Label(new Rect(Screen.width/2-btnW/2,btnY+btnH*2+35.0f,btnW,btnH),"Username:");
			playername= GUI.TextField(new Rect(Screen.width/2,btnY+btnH*2+35.0f,btnW/2,btnH),playername.ToString());
			if(GUI.Button(new Rect(Screen.width/2-btnW/2,btnY,btnW,btnH),"Refresh Hosts",styles[0]))
			{
				//Debug.Log ("Starting server");
				audio.PlayOneShot(clicksound);
				refreshHostList();
				if(hostData==null)
				{
					noGames=true;
				}
			}
			if(GUI.Button(new Rect(Screen.width/2-btnW/2,Screen.height-btnH-12.0f,btnW,btnH),"Back",styles[0]))
			{
				audio.PlayOneShot(clicksound);
				fillinjoindata=false;
				noGames=false;
				connectionfail=false;
			}
			
			if(noGames==true)
			{
				if(connectionfail==true)
				{
				GUI.Box(new Rect((Screen.width/2)-(btnW/2)+20.0f,Screen.height-btnH-60.0f, btnW-40.0f,btnH), "Cannot connect to server",styles[3]);
				}
				else
				{
				GUI.Box(new Rect((Screen.width/2)-(btnW/2)+20.0f,Screen.height-btnH-60.0f, btnW-40.0f,btnH), "Cannot find games",styles[3]);
				}
			}

		}
		
		if(fillindata==true)
		{
			GUI.Label(new Rect(Screen.width/2-btnW/2,btnY+btnH+24.0f,btnW,btnH),"Name of Game:");
			gameName= GUI.TextField(new Rect(Screen.width/2,btnY+btnH+24.0f,btnW/2,btnH),gameName);
			GUI.Label(new Rect(Screen.width/2-btnW/2,btnY+btnH*2+35.0f,btnW,btnH),"Location of Server:");
			serverIp= GUI.TextField(new Rect(Screen.width/2,btnY+btnH*2+35.0f,btnW/2,btnH),serverIp);
			GUI.Label(new Rect(Screen.width/2-btnW/2,btnY+btnH*3+46.0f,btnW,btnH),"Set Port:");
			listenPort= GUI.TextField(new Rect(Screen.width/2,btnY+btnH*3+46.0f,btnW/2,btnH),listenPort.ToString());
			GUI.Label(new Rect(Screen.width/2-btnW/2,btnY+btnH*4+57.0f,btnW,btnH),"Username:");
			playername= GUI.TextField(new Rect(Screen.width/2,btnY+btnH*4+57.0f,btnW/2,btnH),playername.ToString());
			
			if(GUI.Button(new Rect(Screen.width/2-btnW/2,btnY,btnW,btnH),"Start Server",styles[0]))
			{
				//Debug.Log ("Starting server");
				audio.PlayOneShot(clicksound);
				startServer();
				fillindata=false;
			}
			if(GUI.Button(new Rect(Screen.width/2-btnW/2,Screen.height-btnH-12.0f,btnW,btnH),"Back",styles[0]))
			{
				audio.PlayOneShot(clicksound);
				fillindata=false;
				connectionfail=false;
			}
		}
		
		if(Network.isServer && Network.connections.Length<1 && Application.loadedLevel==0)
		{
			GUI.Box(new Rect((Screen.width/2)-(btnW/2),(Screen.height/2)-60, btnW,btnH), "Waiting for player to connect",styles[3]);
			if(connectionfail==true)
			{
				GUI.Box(new Rect((Screen.width/2)-(btnW/2)+10.0f,Screen.height-btnH-60.0f, btnW-20.0f,btnH), "Cannot connect to Server",styles[3]);
			}
			if(GUI.Button(new Rect(Screen.width/2-btnW/2,Screen.height-btnH-12.0f,btnW,btnH),"Back",styles[0]))
			{
				audio.PlayOneShot(clicksound);
				Application.LoadLevel(0);
			}
			if(connectionfail==true)
			{
				GUI.Box(new Rect((Screen.width/2)-(btnW/2)+10.0f,Screen.height-btnH-60.0f, btnW-20.0f,btnH), "Cannot connect to Server",styles[3]);
			}
		}
		
		if(Network.connections.Length==1 && Application.loadedLevel==0)
		{
			for(int i=0;i<scenes.Length;i++)
			{
				GUI.Label(new Rect(Screen.width/2-btnW/2, btnY-32.0f,btnW,btnH+12.0f),"",styles[4]);
				
				if(GUI.Button(new Rect(Screen.width/2-btnW/4 ,btnY-32.0f+32.0f*i+ (btnH*i) +(btnH+42.0f) , btnW/2, btnH ),scenes[i]))
				{
					audio.PlayOneShot(clicksound);
					networkView.RPC("loadLevel",RPCMode.AllBuffered,scenes[i]);
	
				}
				if(GUI.Button(new Rect(Screen.width/2-btnW/2,Screen.height-btnH-12.0f,btnW,btnH),"Back",styles[0]))
				{
				audio.PlayOneShot(clicksound);
				Application.LoadLevel(0);
				}
			}
		}
		
		if(Application.loadedLevel!=0 && choosecharacter==false)
		{
			for(i=0; i<chars.Length;i++)
			{
				GUI.Label(new Rect(Screen.width/2-btnW/2, btnY-160.0f,btnW,btnH+12.0f),"",styles[5]);

				if(GUI.Button(new Rect(Screen.width/2-btnW/4 ,btnY-160.0f+32.0f*i+ (btnH*i) +(btnH+42.0f), btnW/2, btnH ),new GUIContent(chars[i].name,chars[i].name)))
				{
					audio.PlayOneShot(clicksound);
					spawnObject=GameObject.Find("Spawnpoint1").transform;
					spawnObject2=GameObject.Find("Spawnpoint2").transform;
					spawnPlayer(chars[i].prefab);
					choosecharacter=true;
				}
				
				if(GUI.tooltip==chars[i].name)
				{
				GUI.TextArea(new Rect(Screen.width/2+ btnW/3,btnY-112.0f,(3*btnW)/4, btnH*7), chars[i].text, styles[7]);
				GUI.Label(new Rect(20.0f, btnY- 160.0f, chars[i].profilepic.normal.background.width,chars[i].profilepic.normal.background.height),"",chars[i].profilepic);
				}
				
				if(GUI.Button(new Rect(Screen.width/2-btnW/2,Screen.height-btnH-12.0f,btnW,btnH),"Back",styles[0]))
				{
				audio.PlayOneShot(clicksound);
				Application.LoadLevel(0);
				}
			}
		}
	}
	
	void spawnPlayer(Object prefab)
	{
		if(Network.isServer)
		{
			player1Object= Network.Instantiate(prefab,spawnObject.position,Quaternion.identity,0);
			player1Object.name= "Player";
			GameObject.Find("Player").layer=LayerMask.NameToLayer("LayerSelf");
			changeLayer(GameObject.Find("Player"));
			player1= (PlayerController)GameObject.Find ("Player").GetComponent("PlayerController");
			player1Health=(Healthsystem)GameObject.Find("Player").GetComponent("Healthsystem");
			player1Health.playernumber=1;
			player1Health.controller1=player1;
			player1Health.player1=player1Health;
			player1.health=player1Health;
			camera=(CameraFightScene)GameObject.Find ("Main Camera").GetComponent("CameraFightScene");
			camera.player1=GameObject.Find ("Player").transform;
			healthBar=(Healthbar)GameObject.Find("Main Camera").GetComponent("Healthbar");
			healthBar.x1=player1Health;
			networkView.RPC("changePlayers", RPCMode.AllBuffered, 1);
			if(playersplaying==2)
			{
			networkView.RPC("player2connect", RPCMode.AllBuffered, true);
			}
		}
		else
		{
			if(Network.isClient)
			{
				player2Object= Network.Instantiate(prefab,spawnObject2.position,Quaternion.identity,1);
				player2Object.name= "Player2";
				GameObject.Find("Player2").layer=LayerMask.NameToLayer("LayerSelf");
				changeLayer(GameObject.Find("Player2"));
				player2= (PlayerController)GameObject.Find ("Player2").GetComponent("PlayerController");
				player2Health=(Healthsystem)GameObject.Find("Player2").GetComponent("Healthsystem");
				player2Health.playernumber=2;
				player2Health.controller2=player2;
				player2Health.player2=player2Health;
				player2.health=player2Health;
				camera=(CameraFightScene)GameObject.Find ("Main Camera").GetComponent("CameraFightScene");
				camera.player2=GameObject.Find ("Player2").transform;
				healthBar=(Healthbar)GameObject.Find("Main Camera").GetComponent("Healthbar");
				healthBar.x2=player2Health;
				networkView.RPC("changePlayers", RPCMode.AllBuffered, 1);
				if(playersplaying==2)
				{
				networkView.RPC("player2connect", RPCMode.AllBuffered, true);
				}			
			}
		}
	}
	
	[RPC]
	void loadLevel(string scene)
	{
	 Application.LoadLevel(scene);
	}
	
	[RPC]
	void player2connect(bool connect)
	{
		Player2connected=true;
	}
	
	[RPC]
	void changePlayers(int plus)
	{
		playersplaying+=plus;
	}
	
	[RPC]
	void sendPlayerInfo (int number, string name)
	{
		
		Debug.Log ("Name "+ number + " : " + name + " is sent");
		switch(number)
		{
		case 1:
			player1Health.name=name;
			break;
		case 2:
			player2Health.name=name;
			break;
		}
	}
}
