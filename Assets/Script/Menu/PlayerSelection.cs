﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using KeyboardInput;

public class PlayerSelection : MonoBehaviour {
	#region Properties
	public XboxController Xcontroller;
	public KeyboardController Kcontroller;

	public Text confirmText;

	public int numberOfPlayers;

	public string playerNumber;

	public bool isSet = false;
	public bool isKeyboard;
	public bool ready = false;
	private bool canUseAxis = true;

	private GameObject Handler;
	#endregion
	
	#region Methods
	void Start(){
		Handler = GameObject.Find ("SelectionHandler");
		gameObject.GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AblePlayers [0];
		Search("Arrow").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleArrows [0];
	}

	void Passing(){
		if (!isKeyboard && isSet && !ready) {
			if (XCI.GetAxisRaw (XboxAxis.LeftStickX, Xcontroller) == 1f && canUseAxis) {
				canUseAxis = false;
				ChangeCurrentSprite ("next");
			} else if (XCI.GetAxisRaw (XboxAxis.LeftStickX, Xcontroller) == -1f && canUseAxis) {
				canUseAxis = false;
				ChangeCurrentSprite ("previous");
			} else if (XCI.GetAxisRaw (XboxAxis.LeftStickX, Xcontroller) == 0f) {
				canUseAxis = true;
			}
		} else if (isKeyboard && isSet && !ready) {
			if (KCI.GetAxisRaw (KeyboardAxis.Horizontal, Kcontroller) == 1f && canUseAxis) {
				canUseAxis = false;
				ChangeCurrentSprite ("next");
			} else if (KCI.GetAxisRaw (KeyboardAxis.Horizontal, Kcontroller) == -1f && canUseAxis) {
				canUseAxis = false;
				ChangeCurrentSprite ("previous");
			} else if (KCI.GetAxisRaw (KeyboardAxis.Horizontal, Kcontroller) == 0f) {
				canUseAxis = true;
			}
		}
	}

	void ChangeCurrentSprite(string direction){
		for (int i = 0; i < Handler.GetComponent<SelectionController> ().AblePlayers.Count; i++) {
			if (gameObject.GetComponent<SpriteRenderer> ().sprite == Handler.GetComponent<SelectionController> ().AblePlayers [i]) {
				if (direction == "next" && i + 1 <= Handler.GetComponent<SelectionController> ().AblePlayers.Count - 1) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AblePlayers [i + 1];
					Search("Arrow").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleArrows [i + 1];
				} else if (direction == "previous" && i - 1 >= 0) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AblePlayers [i - 1];
					Search("Arrow").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleArrows [i - 1];
				}
				break;
			}
		}
		
		for (int i = 0; i < Handler.GetComponent<SelectionController> ().NotAblePlayers.Count; i++) {
			if (gameObject.GetComponent<SpriteRenderer> ().sprite == Handler.GetComponent<SelectionController> ().NotAblePlayers [i]) {
				if (direction == "checking" && i + 1 <= Handler.GetComponent<SelectionController> ().AblePlayers.Count - 1) {
					gameObject.GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AblePlayers [int.Parse(Handler.GetComponent<SelectionController> ().NotAblePlayers [i].name.Substring(7)) - 1];
					Search("Arrow").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleArrows [int.Parse(Handler.GetComponent<SelectionController> ().NotAblePlayers [i].name.Substring(7)) - 1];
				}
				break;
			}
		}
	}

	void Choosing(){
		if (!isKeyboard && isSet && !ready) {
			if (XCI.GetButtonDown (XboxButton.A, Xcontroller)) {
				GameObject.Find ("AudioHandler").GetComponent<AudioBehaviour> ().audios [9].Play ();
				ready = true;
				PlayerPrefs.SetString (playerNumber, gameObject.GetComponent<SpriteRenderer> ().sprite.name);
				GiveControll ();

				if (gameObject.GetComponent<SpriteRenderer> ().sprite.name != "Player Random") {
					int element = int.Parse (gameObject.GetComponent<SpriteRenderer> ().sprite.name.Substring (7));
					Search ("Selection").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleSelection.Find(x => x.name == "Player" + " " + element.ToString() + " " + "Selection");
					Handler.GetComponent<SelectionController> ().SpriteNotAble (gameObject.GetComponent<SpriteRenderer> ().sprite,
																				Search ("Arrow").GetComponent<SpriteRenderer> ().sprite,
																				Search ("Selection").GetComponent<SpriteRenderer> ().sprite);
				} else {
					Search ("Selection").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleSelection [Handler.GetComponent<SelectionController> ().AbleSelection.Count - 1];
					Handler.GetComponent<SelectionController>().playersAreRandom.Add(gameObject);
				}
			}
		} else if (isKeyboard && isSet && !ready) {
			if (KCI.GetButtonDown (KeyboardButton.Jump, Kcontroller)) {
				GameObject.Find ("AudioHandler").GetComponent<AudioBehaviour> ().audios [9].Play ();
				ready = true;
				PlayerPrefs.SetString (playerNumber, gameObject.GetComponent<SpriteRenderer> ().sprite.name);
				GiveControll ();

				if (gameObject.GetComponent<SpriteRenderer> ().sprite.name != "Player Random") {
					int element = int.Parse (gameObject.GetComponent<SpriteRenderer> ().sprite.name.Substring (7));
					Search ("Selection").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleSelection.Find(x => x.name == "Player" + " " + element.ToString() + " " + "Selection");
					Handler.GetComponent<SelectionController> ().SpriteNotAble (gameObject.GetComponent<SpriteRenderer> ().sprite,
																				Search ("Arrow").GetComponent<SpriteRenderer> ().sprite,
																				Search ("Selection").GetComponent<SpriteRenderer> ().sprite);
				} else {
					Search ("Selection").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleSelection [Handler.GetComponent<SelectionController> ().AbleSelection.Count - 1];
					Handler.GetComponent<SelectionController>().playersAreRandom.Add(gameObject);
				}
			}
		}
	}

	void Unchoosing(){
		if (!isKeyboard && isSet && ready) {
			if (XCI.GetButtonDown (XboxButton.B, Xcontroller)) {
				GameObject.Find ("AudioHandler").GetComponent<AudioBehaviour> ().audios [5].Play ();
				ready = false;
				PlayerPrefs.SetString (playerNumber, "none");

				if (gameObject.GetComponent<SpriteRenderer> ().sprite.name != "Player Random") {
					Handler.GetComponent<SelectionController> ().SpriteAble (gameObject.GetComponent<SpriteRenderer> ().sprite,
					                                                         Search ("Arrow").GetComponent<SpriteRenderer> ().sprite,
					                                                         Search ("Selection").GetComponent<SpriteRenderer> ().sprite);
				} else {
					Handler.GetComponent<SelectionController>().playersAreRandom.Remove(gameObject);
				}

				Search ("Selection").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleSelection [0];
			}
		} else if (isKeyboard && isSet && ready) {
			if (KCI.GetButtonDown (KeyboardButton.Action, Kcontroller)) {
				GameObject.Find ("AudioHandler").GetComponent<AudioBehaviour> ().audios [5].Play ();
				ready = false;
				PlayerPrefs.SetString (playerNumber, "none");

				if (gameObject.GetComponent<SpriteRenderer> ().sprite.name != "Player Random") {
					Handler.GetComponent<SelectionController> ().SpriteAble (gameObject.GetComponent<SpriteRenderer> ().sprite,
					                                                         Search ("Arrow").GetComponent<SpriteRenderer> ().sprite,
					                                                         Search ("Selection").GetComponent<SpriteRenderer> ().sprite);
				} else {
					Handler.GetComponent<SelectionController>().playersAreRandom.Remove(gameObject);
				}				

				Search ("Selection").GetComponent<SpriteRenderer> ().sprite = Handler.GetComponent<SelectionController> ().AbleSelection [0];
			}
		}
	}

	void GiveControll(){
		if (isKeyboard) {
			switch (Kcontroller) {
				case KeyboardController.First:
					PlayerPrefs.SetInt (playerNumber + " Controller", 1);
					PlayerPrefs.SetString(playerNumber + " isKeyboard", "true");
					break;
				case KeyboardController.Second:
					PlayerPrefs.SetInt (playerNumber + " Controller", 2);
					PlayerPrefs.SetString(playerNumber + " isKeyboard", "true");
					break;
			}
		} else {
			switch (Xcontroller) {
				case XboxController.First:
					PlayerPrefs.SetInt (playerNumber + " Controller", 1);
					PlayerPrefs.SetString(playerNumber + " isKeyboard", "false");
					break;
				case XboxController.Second:
					PlayerPrefs.SetInt (playerNumber + " Controller", 2);
					PlayerPrefs.SetString(playerNumber + " isKeyboard", "false");
					break;
				case XboxController.Third:
					PlayerPrefs.SetInt (playerNumber + " Controller", 3);
					PlayerPrefs.SetString(playerNumber + " isKeyboard", "false");
					break;
				case XboxController.Fourth:
					PlayerPrefs.SetInt (playerNumber + " Controller", 4);
					PlayerPrefs.SetString(playerNumber + " isKeyboard", "false");
					break;
			}
		}
	}

	void Verifying(){
		if(gameObject.GetComponent<SpriteRenderer>().enabled && !ready){
			string number = gameObject.GetComponent<SpriteRenderer> ().sprite.name.Substring (7);
			if (Handler.GetComponent<SelectionController> ().AbleSelection.Find (x => x.name == "Player" + " " + number + " " + "Selection") == null) {
				ChangeCurrentSprite ("checking");
			}
		}
	}

	public Transform Search(string name){
		return Search(gameObject.transform , name);
	}
	
	public Transform Search(Transform target, string name){
		if (target.name == name){
			return target;
		}
		
		for (int i = 0; i < target.childCount; ++i) {
			Transform result = Search(target.GetChild(i), name);
			
			if (result != null) {
				return result;
			}
		}
		
		return null;
	}

	void FixedUpdate(){
		Passing ();
		Choosing ();
		Verifying ();
	}

	void LateUpdate(){
		Unchoosing ();
	}
	#endregion
}