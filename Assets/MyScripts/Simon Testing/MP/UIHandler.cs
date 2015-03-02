using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public Canvas serverUI;
	public RawImage count1, count2, count3, waiting, go, getReady, victory, defeat;

	public Text comboText;
	private bool isComboActive;
	private int comboTimer;
	private float textSize = -1, sizeMultiplier = 1;

	public AudioSource c1, c2, c3, c4;

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
		
		comboText.text = "asdasd";
		textSize = -1;
		comboText.fontSize = (int) textSize;
	}

	public void Update(){
		if(isComboActive){
			comboTimer++;
			if(comboTimer < 12){
				textSize += 3.5f * sizeMultiplier;
				comboText.fontSize = (int) textSize;
			}
			if(comboTimer > 60){
				textSize -= 1.2f * sizeMultiplier;
				comboText.fontSize = (int) textSize;
			}
			if(comboTimer > 140){
				comboText.text = "";
				textSize = 0;
				sizeMultiplier = 1;
				isComboActive = false;
			}
		}
	}

	public void combo(int comboCount){
		string comboType = null;
		if(comboCount >= 3){
			comboText.color = new Color(0, 255, 0);
			comboType = "Combo! x";
			sizeMultiplier = 1;
			if(comboCount >= 6){
				comboText.color = new Color(100, 200, 0);
				comboType = "Destruction! x";
				sizeMultiplier = 1.2f;
			}if(comboCount >= 9){
				comboText.color = new Color(200, 100, 0);
				comboType = "Carnage! x";
				sizeMultiplier = 1.5f;
			}if(comboCount >= 9){
				comboText.color = new Color(255, 0, 0);
				comboType = "Annihilation! x";
				sizeMultiplier = 2f;
			}

			comboText.text = comboType + comboCount;
			isComboActive = true;
			comboTimer = 0;

			if(comboType.Equals("Combo! x"))
				c1.Play();
			if(comboType.Equals("Destruction! x"))
				c2.Play();
			if(comboType.Equals("Carnage! x"))
				c3.Play();
			if(comboType.Equals("Annihilation! x"))
				c4.Play();
		}
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
}
