using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChildList : MonoBehaviour {

	public List<GameObject> childList;

	void Start(){
		childList = new List<GameObject>();
	}

	public void addChild(GameObject go){
		childList.Add (go);
	}


	public List<GameObject> get(){
		Debug.Log ("returning childlist: " + childList.Count + " name" + childList);
		return childList;
	
	}
}
