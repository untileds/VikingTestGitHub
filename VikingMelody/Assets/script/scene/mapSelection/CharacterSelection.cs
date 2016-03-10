using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelection : MonoBehaviour {
	public List<GameObject> lockedCharacterList;
	// Use this for initialization
	void Start () {
		//UnC= unlocked character
		if(PlayerPrefs.GetInt("UnC")>0){
			for(int i=0;i<PlayerPrefs.GetInt("UnC");i++){
				lockedCharacterList[i].GetComponent<BoxCollider>().enabled=true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
