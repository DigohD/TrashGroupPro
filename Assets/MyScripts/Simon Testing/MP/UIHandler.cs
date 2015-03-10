using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {

	public Canvas serverUI;
	// Images on the canvas
	public RawImage count1, count2, count3, waiting, go, getReady, victory, defeat;

	// Text object for displaying combos
	public Text comboText;
	// Is a combo currently being displayed?
	private bool isComboActive;
	// For how long has the combo been displayed?
	private int comboTimer;

	// textSize -1 means no text, sizemultiplier is the real size of combo text
	private float textSize = -1, sizeMultiplier = 1;

	// Combo announcer audio files
	public AudioSource c1, c2, c3, c4;

	public Text powerText;
	private bool isPowerUpActive;
	private int powerTimer;
	private float textSize2 = -1, sizeMultiplier2 = 1;

	public AudioSource TrashCon;

	// Use this for initialization
	void Start (){
		// Set all images to not displayed
		defeat.enabled = false;
		victory.enabled = false;
		count1.enabled = false;
		count2.enabled = false;
		count3.enabled = false;
		waiting.enabled = false;
		go.enabled = false;
		getReady.enabled = false;

		// Hide the combo display text
		comboText.text = "none";
		textSize = -1;
		comboText.fontSize = (int) textSize;

		powerText.text = "none";
		textSize2 = -1;
		powerText.fontSize = (int) textSize2;
	}

	/* Updates the combo and powerup message. (they work the same)
	 * 
	 *  comboTimer < 12: 				The combo text comes in fast 
	 *  comboTimer between 12 and 60: 	The combo text lingers in place
	 *  comboTimer > 60:				The combo text slowly diminishes
	 * 	comboTimer > 140:				The combo is set to inactive and is no longer shown
	 */
	public void Update(){
		// If a combo is currently being displayed
		if(isComboActive){
			// increase the displayed time of the combo
			comboTimer++;
			// if the combo has been displayed for less than 12 ticks, increase the size
			if(comboTimer < 12){
				textSize += 3.5f * sizeMultiplier;
				comboText.fontSize = (int) textSize;
			}
			// if the combo has been displayed for more than 60 ticks, decrease the size
			if(comboTimer > 60){
				textSize -= 1.2f * sizeMultiplier;
				comboText.fontSize = (int) textSize;
			}
			// if the combo has been displayed for more than 140 ticks, set it to inactive
			if(comboTimer > 140){
				comboText.text = "";
				textSize = 0;
				sizeMultiplier = 1;
				isComboActive = false;
			}
		}
		
		if(isPowerUpActive){
			// increase the displayed time of the combo
			powerTimer++;
			// if the combo has been displayed for less than 12 ticks, increase the size
			if(powerTimer < 12){
				textSize2 += 3.5f * sizeMultiplier2;
				powerText.fontSize = (int) textSize2;
			}
			// if the combo has been displayed for more than 60 ticks, decrease the size
			if(powerTimer > 60){
				textSize2 -= 1.2f * sizeMultiplier2;
				powerText.fontSize = (int) textSize2;
			}
			// if the combo has been displayed for more than 140 ticks, set it to inactive
			if(powerTimer > 140){
				powerText.text = "";
				textSize2 = 0;
				sizeMultiplier2 = 1;
				isPowerUpActive = false;
			}
		}
	}

	// Initiate a combo display text
	public void combo(int comboCount){
		// comboType determines what grade of combo was achieved
		string comboType = null;

		// if more than 3 pieces of trash were destroyed, a combo occured
		if(comboCount >= 3){
			// Set default combo text values
			comboText.color = new Color(0, 255, 0);
			comboType = "Combo! x";
			sizeMultiplier = 1;
			textSize = 0;
			// For each combo grade, make the combo text bigger and more red, also give new name
			if(comboCount >= 5){
				comboText.color = new Color(100, 200, 0);
				comboType = "Destruction! x";
				sizeMultiplier = 1.2f;
			}if(comboCount >= 7){
				comboText.color = new Color(200, 100, 0);
				comboType = "Carnage! x";
				sizeMultiplier = 1.5f;
			}if(comboCount >= 9){
				comboText.color = new Color(255, 0, 0);
				comboType = "Annihilation! x";
				sizeMultiplier = 2f;
			}

			// Set the final combo text string and start displaying it in Update()
			comboText.text = comboType + comboCount;
			isComboActive = true;
			comboTimer = 0;

			// Play announcer voice according to combo grade
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

	public void powerUp(float speed, float weight, bool turret){
		powerText.color = new Color(255, 175, 0);
		powerText.text = "";

		TrashCon.pitch = Random.Range(0.5f, 1.1f);
		TrashCon.Play();

		if(turret)
			powerText.text = powerText.text + "Weapon Added!" + '\n';
		if(speed > 0)
			powerText.text = powerText.text + "Speed Up: " + speed + '\n';
		if(weight > 0)
			powerText.text = powerText.text + "Weight Up: " + weight + '\n';
		sizeMultiplier2 = 1f;
		isPowerUpActive = true;
		powerTimer = 0;
		textSize2 = 0;
	}

	// The following functions simply displays an image

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

	// Clear the UI from all images
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
