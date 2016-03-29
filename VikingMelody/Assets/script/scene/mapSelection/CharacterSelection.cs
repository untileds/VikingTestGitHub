using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {
	Dictionary<string,int> selectedCharacter = new Dictionary<string, int>();
	GameManager gameManager;
	public List<GameObject> lockedCharacterList;

	private int positionIndex=0;

	// Use this for initialization
	void Start () {
		Debug.Log(PlayerPrefs.GetInt("UnC"));
		gameManager = GameManager.getSingleton();
		gameManager.resetCharacterSelectedList();
		initialCharacter();
		if(PlayerPrefs.GetInt("UnC")>0){
			Debug.Log(PlayerPrefs.GetInt("UnC"));
		}
	}
	private void initialCharacter(){
		//UnC= unlocked character
//		if(PlayerPrefs.GetInt("UnC")>0){
//			for(int i=0;i<PlayerPrefs.GetInt("UnC");i++){
//				lockedCharacterList[i].GetComponent<BoxCollider>().enabled=true;
//			}
//		}
	}
	public void sendCharacterData(){
		foreach(KeyValuePair<string,int> pair in selectedCharacter){
			gameManager.addCharacterListByName(pair.Key);
		}
		SceneManager.LoadScene("GamePlay");

	}

	public void selectCharacter(string name , List<GameObject> posList){
		//gameManager.addCharacterListByName(name);
		if(selectedCharacter.ContainsKey(name)){
			foreach(GameObject obj in posList){
				obj.SetActive(false);
			}
		//	Debug.Log("indexlist = "+selectedChar[name]+"  index "+positionIndex);
			if(selectedCharacter[name]<positionIndex){
				positionIndex=selectedCharacter[name];
			}
		//	Debug.Log("position index last= "+positionIndex);
			selectedCharacter.Remove(name);
		}else{
			//Debug.Log("spirte name "+name);
			posList[positionIndex].SetActive(true);
			selectedCharacter.Add(name,positionIndex);
			positionIndex++;
			//Debug.Log("position first= "+positionIndex);
		}
	}

	public void onTestSmth(){
		foreach(KeyValuePair<string,int>pair in selectedCharacter){
			Debug.Log("key ="+pair.Key+", value ="+pair.Value);
		}
	}

	public void inPlayButton(){
		foreach(KeyValuePair<string,int> pair in selectedCharacter){
			gameManager.addCharacterListByName(pair.Key);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
