using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public Canvas serverUI;
	public RawImage count1, count2, count3, waiting, go, getReady;

	// Use this for initialization
	void Start (){
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = false;

		//count1.rectTransform.Translate(new Vector2(-110000, -110000));
		//count2.rectTransform.Translate(new Vector2(-110000, -110000));
		//count3.rectTransform.Translate(new Vector2(-110000, -110000));
		//go.rectTransform.Translate(new Vector2(-110000, -110000));
	}

	public void setWaiting(){
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = true;
		go.enabled = false;
		getReady.enabled = false;
	}

	public void setCount3(){
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = true;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = false;
	}

	public void setCount2(){
		count1.enabled = false;
		count2.enabled = true;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = false;
	}

	public void setCount1(){
		count1.enabled = true;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = false;
	}

	public void setGo(){
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = true;
		getReady.enabled = false;
	}

	public void setGetReady(){
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = true;
	}

	public void clearUI(){
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
