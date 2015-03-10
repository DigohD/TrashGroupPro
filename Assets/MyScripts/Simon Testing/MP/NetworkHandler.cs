using UnityEngine;
using System.Collections;


public class NetworkHandler : MonoBehaviour {

	// Name of the server
	string server_ID = "MP_TRASH_HEAP_SERVER_TEST_X01X";
	// True if refreshing hostlist
	bool refreshing = false;
	// Time in seconds that we wait for servers
	float refreshRequestLength = 3.0f;
	// The servers fetched from the master server
	HostData[] hostData;

	// The player object prefab
	public GameObject playerClass;
	// The game controller prefab
	public GameObject gameControlClass;

	// Audio for countdown
	public AudioSource aGetReady, aC1, aC2, aC3, aGo;

	// The UI
	public UIHandler ui;
	// The Victory-Defeat condition monitor
	public EndGameScript egs;

	string userName = "";

	// The countdown/game-startup phase
	int phase = 0;

	// Has another player connected? 
	// Has the player spawned? 
	// Run in single player mode?
	bool connected, playerSpawned, singleTesting;

	// Initialize the server on the Unity master server, so that other players can find us.
	// NOTE: Port forwarding must be applied within the router for the server to work
	void StartServer(){
		Network.InitializeServer(16, 25002, false);
		MasterServer.RegisterHost(server_ID, "Trash Heap MP", "Test for server code");
	}

	// When server is up and running
	void OnServerInitialized(){
		// Another player has not yet connected
		connected = false;

		// If not single testing, we start at the "waiting for player" phase
		if(!singleTesting)
			setPhase(1);
		// else, we jump directly to the "start game" phase
		else
			setPhase(7);
	}

	// Logs that we successfully connected to the Unity master server
	void OnMasterServerEvent(MasterServerEvent masterServerEvent){
		if(masterServerEvent == MasterServerEvent.RegistrationSucceeded){
			Debug.Log("Registration Successful");
		}
	}

	// Refreshes the list of available servers
	IEnumerator RefreshHostList(){
		Debug.Log("Refreshing!");

		// Request server list from Unity master server
		MasterServer.RequestHostList(server_ID);

		// Calculate when to stop refreshing the host list
		float timeStart = Time.time;
		float timeEnd = Time.time + refreshRequestLength;

		// Continuously request servers until the specified time has passed
		while(timeStart < timeEnd){
			hostData = MasterServer.PollHostList();
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}

		// Log messages for the number of servers, if any, are found
		if(hostData == null || hostData.Length == 0){
			Debug.Log("No Active Servers Found!");
		}else{
			Debug.Log(hostData.Length + " Servers found!");
		}
		
	}

	// Checks if the game controller has already been spawned
	private bool isControllerSpawned = false;

	// instantiates the game controller if it has not already been instantiated
	void createGameController(){
		if(!isControllerSpawned){
			GameObject gameInstance = (GameObject) Instantiate(gameControlClass, new Vector3(0f, 0f, 0f), Quaternion.identity);
			isControllerSpawned = true;
		}
	}

	// If the player for the local client has not been spawned already, spawn it
	void initSpawnPlayer(int posX, int posY){
		if(!playerSpawned)
			SpawnPlayer(Network.player, userName, posX, posY);
	}

	// Spawns a player at the specified location
	void SpawnPlayer(NetworkPlayer newPlayer, string newUserName, int posX, int posY){
		playerSpawned = true;
		GameObject playerInstance = (GameObject) Network.Instantiate(playerClass, new Vector3(posX, posY, 2f), Quaternion.identity, 0);
		playerInstance.transform.rotation =  Quaternion.Euler(-90, 0, 0);

		// Set the player username
		PlayerStats ps = (PlayerStats) playerInstance.GetComponent("PlayerStats");
		ps.setID(newUserName);

		// Connect the camera to the Player
		PlayerCameraSetup(playerInstance);

		//NetworkView playerNetworkView = playerInstance.networkView;
		//playerNetworkView.RPC("PlayerSetupFunc", RPCMode.AllBuffered, newPlayer);
	}

	// Sets the tracked object of the camera to the player object
	void PlayerCameraSetup(GameObject player){
		if (player.networkView.isMine)
		{
			ObjectTrack ot;
			ot = (ObjectTrack) Camera.main.GetComponent("ObjectTrack");
			ot.tracked = player;
		}
	}

	void OnGUI(){
		// Print a small top-let corner label, so that the player knows its status
		if(Network.isClient)
			GUILayout.Label("Client");
		if(Network.isServer)
			GUILayout.Label("Server");

		// If the local client is either host or client, don't show the start GUI
		if(Network.isClient || Network.isServer)
			return;

		// Input field for player username
		userName = GUI.TextField(new Rect (25f, 145f, 150f, 25f), userName, 25);

		// Button that starts the server, waiting for another player to connect
		if(userName.Length > 4 && GUI.Button(new Rect(25f, 25f, 150f, 30f), "Start New Server")){
			singleTesting = false;
			StartServer();
		}

		// Button that fetches server list
		if(userName.Length > 4 && GUI.Button(new Rect(25f, 65f, 150f, 30f), "Refresh Server List")){
			StartCoroutine(RefreshHostList());
		}

		// Button for single player testing mode
		if(GUI.Button(new Rect(25f, 105f, 150f, 30f), "Single Player Testing")){
			singleTesting = true;
			StartServer();
		}

		// Creates one button for each available server, connects to the one pressed
		if(hostData != null){
			for(var i = 0; i < hostData.Length; i++){
				if(!userName.Equals("") && GUI.Button(new Rect(Screen.width/2, 65f * i, 300f, 30f), hostData[i].gameName)){
					Network.Connect(hostData[i]);
				}
			}
		}
	}

	// When a player disconnects, destroy everything associated with that player
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

	// When the game is closed, disconnect from the server/Unity-master-server
	void OnApplicationQuit(){
		if(Network.isServer){
			Network.Disconnect(200);
			MasterServer.UnregisterHost();
		}
		if(Network.isClient){
			Network.Disconnect(200);
		}
	}

	// When istimed is set to false, continue to the next phase in Update()
	bool isTimed = false;

	void Update () {
		// Phase 1: Wait for a player to connect, Display "waiting for player..."
		if(phase == 1){
			if(connected)
				setPhase(2);
			else
				ui.setWaiting();
		}
		// Phase 2: Display "get ready" for 2 seconds
		else if(phase == 2){
			// Display "get ready" message
			ui.setGetReady();
			// If isTimed = false, start waiting-coroutine and play announcer sound
			if(!isTimed){
				isTimed = true;
				aGetReady.Play();
				StartCoroutine(PhaseTimer(3, 2));
			}
		}
		// Phase 3: Display "3" for 1 second
		else if(phase == 3){
			ui.setCount3();
			if(!isTimed){
				isTimed = true;
				aC3.Play();
				StartCoroutine(PhaseTimer(4, 1));
			}
		}
		// Phase 4: Display "2" for 1 second
		else if(phase == 4){
			ui.setCount2();
			if(!isTimed){
				isTimed = true;
				aC2.Play();
				StartCoroutine(PhaseTimer(5, 1));
			}
		}
		// Phase 5: Display "1" for 1 second
		else if(phase == 5){
			ui.setCount1();
			if(!isTimed){
				isTimed = true;
				aC1.Play();
				StartCoroutine(PhaseTimer(6, 1));
			}
		}
		// Phase 6: Display "GO" for 1 second
		else if(phase == 6){
			ui.setGo();
			if(!isTimed){
				isTimed = true;
				aGo.Play();
				StartCoroutine(PhaseTimer(7, 1));
			}
		}
		// Phase 7: Start the game
		else if(phase == 7){
			ui.clearUI();
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

	// USed to switch to the next phase with a time delay
	IEnumerator PhaseTimer(int nextPhase, int seconds){
		// Specify waiting time
		float timeStart = Time.time;
		float timeEnd = Time.time + seconds;
		
		// Wait until specified time has passed
		while(timeStart < timeEnd){
			timeStart = Time.time;
			yield return new WaitForEndOfFrame();
		}
		
		//When waiting time is over, continue to the specified phase
		isTimed = false;
		setPhase(nextPhase);
	}

	// Synchronizes the phase on all clients
	void setPhase(int newPhase){
		if(Network.isClient)
			return;
		networkView.RPC("rpcSetPhase", RPCMode.All, newPhase);
	}

	// Used to send spawn player message over the network
	[RPC]
	void rpcSpawnPlayers(int posX, int posY){
		initSpawnPlayer(posX, posY);
		// Start the win-lose condition monitor
		egs.activate();
	}

	// Used to set the phase over the network
	[RPC]
	void rpcSetPhase(int newPhase){
		phase = newPhase;
	}


}
