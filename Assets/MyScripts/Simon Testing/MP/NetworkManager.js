#pragma strict

var server_ID : String = "MP_TRASH_HEAP_SERVER_TEST_X01X";
var refreshing : boolean = false;
var refreshRequestLength = 3.0f;
var hostData : HostData[];
var playerClass : GameObject;

function StartServer(){
	Network.InitializeServer(16, 25002, false);
	MasterServer.RegisterHost(server_ID, "Trash Heap MP", "Test for server code");
}

function OnServerInitialized(){
	Debug.Log("Server Initialized!");
	SpawnPlayer();
}

function OnMasterServerEvent(masterServerEvent : MasterServerEvent){
	if(masterServerEvent == MasterServerEvent.RegistrationSucceeded){
		Debug.Log("Registration Successful");
	}
}

function RefreshHostList(){
	Debug.Log("Refreshing!");
	MasterServer.RequestHostList(server_ID);
	
	var timeStart : float = Time.time;
	var timeEnd : float = Time.time + refreshRequestLength;
	
	while(timeStart < timeEnd){
		hostData = MasterServer.PollHostList();
		timeStart = Time.time;
		yield new WaitForEndOfFrame();
	}
	
	if(hostData == null || hostData.Length == 0){
		Debug.Log("No Active Servers Found!");
	}else{
		Debug.Log(hostData.Length + " Servers found!");
	}
		
}

function SpawnPlayer(){
	Debug.Log("Spawning new player!");
	Network.Instantiate(playerClass, Vector3(0f, 0f, 0f), Quaternion.identity, 0);
}

function OnGUI() {
	
	if(Network.isClient || Network.isServer)
		return;
		
	if(Network.isClient)
		GUILayout.Label("Client");
	if(Network.isServer)
		GUILayout.Label("Server");
	
	if(GUI.Button(new Rect(25f, 25f, 150f, 30f), "Start New Server")){
		StartServer();
	}
	
	if(GUI.Button(new Rect(25f, 65f, 150f, 30f), "Refresh Server List")){
		StartCoroutine("RefreshHostList");
	}
	
	if(hostData != null){
		for(var i = 0; i < hostData.Length; i++){
			if(GUI.Button(new Rect(Screen.width/2, 65f * i, 300f, 30f), hostData[i].gameName)){
				Network.Connect(hostData[i]);
			}
		}
	}
	
}

function OnPlayerDisconnected(player : NetworkPlayer){
	Network.RemoveRPCs(player);
	Network.DestroyPlayerObjects(player);
}

function OnApplicationQuit(){
	if(Network.isServer){
		Network.Disconnect(200);
		MasterServer.UnregisterHost();
	}
	if(Network.isClient){
		Network.Disconnect(200);
	}
}

function Update () {

}