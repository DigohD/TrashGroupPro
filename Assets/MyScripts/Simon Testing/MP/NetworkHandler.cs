using UnityEngine;
using System.Collections;


public class NetworkHandler : MonoBehaviour {

	string server_ID = "MP_TRASH_HEAP_SERVER_TEST_X01X";
	bool refreshing = false;
	float refreshRequestLength = 3.0f;
	HostData[] hostData;
	public GameObject playerClass;
	public GameObject gameControlClass;

	public AudioSource aGetReady, aC1, aC2, aC3, aGo;

	public UIHandler ui;
	public EndGameScript egs;

	public GameObject whaleClass;

	string userName = "";
	int phase = 0;

	bool connected, playerSpawned, singleTesting;

	void StartServer(){
		Network.InitializeServer(16, 25002, false);
		MasterServer.RegisterHost(server_ID, "Trash Heap MP", "Test for server code");
	}
	
	void OnServerInitialized(){
		print("Server Initialized!");
		connected = false;
		if(!singleTesting)
			setPhase(1);
		else
			setPhase(7);
	}

	void OnMasterServerEvent(MasterServerEvent masterServerEvent){
		if(masterServerEvent == MasterServerEvent.RegistrationSucceeded){
			Debug.Log("Registration Successful");
		}
	}
	
	IEnumerator RefreshHostList(){
		Debug.Log("Refreshing!");
		MasterServer.RequestHostList(server_ID);
		
		float timeStart = Time.time;
		float timeEnd = Time.time + refreshRequestLength;
		
		while(timeStart < timeEnd){
			hostData = MasterServer.PollHostList();
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}
		
		if(hostData == null || hostData.Length == 0){
			Debug.Log("No Active Servers Found!");
		}else{
			Debug.Log(hostData.Length + " Servers found!");
		}
		
	}
	
	/*void SpawnPlayer(NetworkPlayer newPlayer){
		GameObject playerInstance = (GameObject) Network.Instantiate(playerClass, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
		
		if(newPlayer == Network.player){
			PlayerSetupServer(playerInstance);
			return;
		}
		
		NetworkView playerNetworkView = playerInstance.networkView;
		playerNetworkView.RPC("PlayerSetupFunc", RPCMode.AllBuffered, newPlayer);
	}*/

	private bool isControllerSpawned = false;

	void createGameController(){
		if(!isControllerSpawned){
			GameObject gameInstance = (GameObject) Instantiate(gameControlClass, new Vector3(0f, 0f, 0f), Quaternion.identity);
			isControllerSpawned = true;
		}
	}

	void initSpawnPlayer(int posX, int posY){
		if(!playerSpawned)
			SpawnPlayer(Network.player, userName, posX, posY);
	}

	void SpawnPlayer(NetworkPlayer newPlayer, string newUserName, int posX, int posY){
		playerSpawned = true;
		GameObject playerInstance = (GameObject) Network.Instantiate(playerClass, new Vector3(posX, posY, 2f), Quaternion.identity, 0);
		playerInstance.transform.rotation =  Quaternion.Euler(-90, 0, 0);

		PlayerStats ps = (PlayerStats) playerInstance.GetComponent("PlayerStats");
		ps.setID(newUserName);

		PlayerSetupServer(playerInstance);

		//NetworkView playerNetworkView = playerInstance.networkView;
		//playerNetworkView.RPC("PlayerSetupFunc", RPCMode.AllBuffered, newPlayer);
	}

	void PlayerSetupServer(GameObject player){
		if (player.networkView.isMine)
		{
			ObjectTrack ot;
			ot = (ObjectTrack) Camera.main.GetComponent("ObjectTrack");
			ot.tracked = player;
		}
	}



	void OnGUI(){
		if(Network.isClient)
			GUILayout.Label("Client");
		if(Network.isServer)
			GUILayout.Label("Server");
		
		if(Network.isClient || Network.isServer)
			return;


		userName = GUI.TextField(new Rect (25f, 145f, 150f, 25f), userName, 25);

		if(userName.Length > 4 && GUI.Button(new Rect(25f, 25f, 150f, 30f), "Start New Server")){
			singleTesting = false;
			StartServer();
		}
		
		if(userName.Length > 4 && GUI.Button(new Rect(25f, 65f, 150f, 30f), "Refresh Server List")){
			StartCoroutine(RefreshHostList());
		}

		if(GUI.Button(new Rect(25f, 105f, 150f, 30f), "Single Player Testing")){
			singleTesting = true;
			StartServer();
		}
		
		if(hostData != null){
			for(var i = 0; i < hostData.Length; i++){
				if(!userName.Equals("") && GUI.Button(new Rect(Screen.width/2, 65f * i, 300f, 30f), hostData[i].gameName)){
					Network.Connect(hostData[i]);
				}
			}
		}
	}
	
	void OnPlayerDisconnected(NetworkPlayer player){
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	
	void OnPlayerConnected(NetworkPlayer player){
		connected = true;
	}

	void OnConnectedToServer(){
		connected = true;
	}
	
	void OnApplicationQuit(){
		if(Network.isServer){
			Network.Disconnect(200);
			MasterServer.UnregisterHost();
		}
		if(Network.isClient){
			Network.Disconnect(200);
		}
	}

	bool isTimed = false;

	void Update () {
		if(phase == 1){
			if(connected)
				setPhase(2);
			else
				ui.setWaiting();
		}else if(phase == 2){
			ui.setGetReady();
			if(!isTimed){
				isTimed = true;
				aGetReady.Play();
				StartCoroutine(setCount3());
			}
		}else if(phase == 3){
			ui.setCount3();
			if(!isTimed){
				isTimed = true;
				aC3.Play();
				StartCoroutine(setCount2());
			}
		}else if(phase == 4){
			ui.setCount2();
			if(!isTimed){
				isTimed = true;
				aC2.Play();
				StartCoroutine(setCount1());
			}
		}else if(phase == 5){
			ui.setCount1();
			if(!isTimed){
				isTimed = true;
				aC1.Play();
				StartCoroutine(setGo());
			}
		}else if(phase == 6){
			ui.setGo();
			if(!isTimed){
				isTimed = true;
				aGo.Play();
				StartCoroutine(deleteGo());
			}
		}else if(phase == 7){
			if(Network.isServer){
				// Player 2 coords X: 96 Y: 14
				networkView.RPC("rpcSpawnPlayers", RPCMode.Others, 96, 14);
				initSpawnPlayer(6, 3);
				createGameController();
				egs.activate();
			}
			phase = 8;
		}
	}

	IEnumerator setCount3(){
		float timeStart = Time.time;
		float timeEnd = Time.time + 2;
		
		while(timeStart < timeEnd){
			hostData = MasterServer.PollHostList();
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}
		
		setPhase(3);
		isTimed = false;
	}

	IEnumerator setCount2(){
		float timeStart = Time.time;
		float timeEnd = Time.time + 1;
		
		while(timeStart < timeEnd){
			hostData = MasterServer.PollHostList();
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}
		
		setPhase(4);
		isTimed = false;
	}

	IEnumerator setCount1(){
		float timeStart = Time.time;
		float timeEnd = Time.time + 1;
		
		while(timeStart < timeEnd){
			hostData = MasterServer.PollHostList();
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}
		
		setPhase(5);
		isTimed = false;
	}

	IEnumerator setGo(){
		float timeStart = Time.time;
		float timeEnd = Time.time + 1;
		
		while(timeStart < timeEnd){
			hostData = MasterServer.PollHostList();
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}
		
		setPhase(6);
		isTimed = false;
	}

	IEnumerator deleteGo(){
		float timeStart = Time.time;
		float timeEnd = Time.time + 1;
		
		while(timeStart < timeEnd){
			hostData = MasterServer.PollHostList();
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}
		
		ui.clearUI();
		setPhase(7);
		isTimed = false;
	}

	void setPhase(int newPhase){
		phase = newPhase;
		networkView.RPC("rpcSetPhase", RPCMode.Others, newPhase);
	}

	[RPC]
	void rpcSpawnPlayers(int posX, int posY){
		initSpawnPlayer(posX, posY);
		egs.activate();
	}

	[RPC]
	void rpcSetPhase(int newPhase){
		phase = newPhase;
	}


}
