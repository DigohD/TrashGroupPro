using UnityEngine;
using System.Collections;

public class NetworkHandler : MonoBehaviour {

	string server_ID = "MP_TRASH_HEAP_SERVER_TEST_X01X";
	bool refreshing = false;
	float refreshRequestLength = 3.0f;
	HostData[] hostData;
	public GameObject playerClass;
	public GameObject gameControlClass;
	
	void StartServer(){
		Network.InitializeServer(16, 25002, false);
		MasterServer.RegisterHost(server_ID, "Trash Heap MP", "Test for server code");
	}
	
	void OnServerInitialized(){
		Debug.Log("Server Initialized!");
		SpawnPlayer(Network.player);
		createGameController();
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

	void createGameController(){
		GameObject gameInstance = (GameObject) Network.Instantiate(gameControlClass, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
	}

	void SpawnPlayer(NetworkPlayer newPlayer){
		GameObject playerInstance = (GameObject) Network.Instantiate(playerClass, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
		playerInstance.transform.rotation =  Quaternion.Euler(-90, 0, 0);

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
	
	void OnGUI() {
		if(Network.isClient)
			GUILayout.Label("Client");
		if(Network.isServer)
			GUILayout.Label("Server");
		
		if(Network.isClient || Network.isServer)
			return;
		
		if(GUI.Button(new Rect(25f, 25f, 150f, 30f), "Start New Server")){
			StartServer();
		}
		
		if(GUI.Button(new Rect(25f, 65f, 150f, 30f), "Refresh Server List")){
			StartCoroutine(RefreshHostList());
		}
		
		if(hostData != null){
			for(var i = 0; i < hostData.Length; i++){
				if(GUI.Button(new Rect(Screen.width/2, 65f * i, 300f, 30f), hostData[i].gameName)){
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
		//SpawnPlayer(player);
	}

	void OnConnectedToServer(){
		Debug.Log("CONNECTED!!!");
		SpawnPlayer(Network.player);
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
	
	void Update () {
		
	}
}
