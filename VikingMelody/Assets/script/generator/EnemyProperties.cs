using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyProperties : MonoBehaviour {
	public List<GameObject>unlockSign;
	public List<GameObject>lockedSigh;
	public GameObject auraEffect;
	GameManager gameManager = GameManager.getSingleton();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setEnemySign(){
		//set position1
		string objectName=gameObject.GetComponentInParent<EnemySignCreater>().getGameObjetName();
		if(PlayerPrefs.GetInt("UnC")>0){
			int position = Random.Range(0,unlockSign.Count+PlayerPrefs.GetInt("UnC"));
			//Debug.Log("random create sign "+position);
			foreach(GameObject enemySing in unlockSign){
				enemySing.SetActive(false);
			}
			foreach(GameObject enemySing in lockedSigh){
				enemySing.SetActive(false);
			}
			if(position>unlockSign.Count-1){
				lockedSigh[position-unlockSign.Count].SetActive(true);
			//	Debug.Log("GameObject "+objectName);
				gameManager.addEnemyList(objectName,position);
			}else{
				unlockSign[position].SetActive(true);
				//Debug.Log("GameObject "+objectName);
				gameManager.addEnemyList(objectName,position);
			}
		}else{
			int position = Random.Range(0,unlockSign.Count);
			foreach(GameObject enemySing1OBJ in unlockSign){
				enemySing1OBJ.SetActive(false);
			}
			unlockSign[position].SetActive(true);
			//Debug.Log("GameObject "+objectName);
			gameManager.addEnemyList(objectName,position);
		}
	}

	public void destroyAura(){
		auraEffect.SetActive(false);
	}

	public GameObject getAuraEffect(){
		return auraEffect;
	}

}
