using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public Canvas serverUI;
	public RawImage count1, count2, count3, waiting, go, getReady, victory, defeat;

	// Use this for initialization
	void Start (){
		defeat.enabled = false;
		victory.enabled = false;
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = false;
	}

	public void setWaiting(){
		clearAll ();
		waiting.enabled = true;
	}

	public void setCount3(){
		clearAll ();
		count3.enabled = true;
	}

	public void setCount2(){
		clearAll ();
		count2.enabled = true;
	}

	public void setCount1(){
		clearAll ();
		count1.enabled = true;
	}

	public void setGo(){
		clearAll ();
		go.enabled = true;
	}

	public void setGetReady(){
		clearAll ();
		getReady.enabled = true;
	}

	public void setVictory(){
		clearAll ();
		victory.enabled = true;
	}

	public void setDefeat(){
		clearAll ();
		defeat.enabled = true;
	}

	public void clearUI(){
		clearAll();
	}

	private void clearAll(){
		defeat.enabled = false;
		victory.enabled = false;
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = false;
	}

	// Update is called once per frame
	void Update (){
	
	}
}
