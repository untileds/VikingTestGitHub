using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InitialMapSelection : MonoBehaviour {
	GameManager gameManager = GameManager.getSingleton();
	public List<GameObject> stageList;
	// Use this for initialization
	void Start () {
		if(gameManager.getUnlockedStage()!=0){
			for(int i =0;i<gameManager.getUnlockedStage();i++){
				stageList[i].GetComponent<BoxCollider>().enabled=true;
			}
		}
	}

	public void sentStageData(int stage){
		gameManager.setStage(stage);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void onSetStage(int stage){
		gameManager.setStage(stage);
	}
}
