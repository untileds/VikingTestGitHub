using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour {
	public GameObject enemyLine;
	private List<GameObject> enemyList =new List<GameObject>();
	private List<GameObject> usingEnemyList = new List<GameObject>();
	private List<GameObject> pauseEnemyList = new List<GameObject>();
	int objNumber;

	public GameObject getEnemyLine(){
		return enemyLine;
	}

	public void initialGenerator(){
		enemyList.Clear();
		usingEnemyList.Clear();
		pauseEnemyList.Clear();
		objNumber=0;
	}

	public void restartGenerator(){
		objNumber=0;
		foreach(GameObject enemy in enemyList){
			Destroy(enemy);
		}
		enemyList.Clear();
		usingEnemyList.Clear();
		pauseEnemyList.Clear();
	//	Debug.Log("restart usingEnemyList "+usingEnemyList.Count+"  pauseEnemyList"+pauseEnemyList.Count);
	}

	public void creatEnemy(){
		//Debug.Log (gameObject.name+"position"+gameObject.transform.position);
		GameObject enemyTemp = enemyPooler(gameObject.transform.position);

		enemyTemp.GetComponent<EnemyObjectScript>().onSetEnemy();
	}

	private GameObject enemyPooler(Vector3 position){
		Debug.Log(gameObject+" generator position "+position);
		for (int i = 0; i< enemyList.Count;i++){
			if(!enemyList[i].activeInHierarchy){
				enemyList[i].transform.position = position;
				enemyList[i].GetComponent<TweenScale>().ResetToBeginning();
				enemyList[i].GetComponent<TweenScale>().PlayForward();
				enemyList[i].SetActive(true);
				usingEnemyList.Add(enemyList[i]);
				//Debug.Log("usingEnenmyList  is  "+usingEnemyList.Count);
				return enemyList[i];
			}
			objNumber=i+1;
			//Debug.Log (gameObject+"  i is "+i+" objNum is "+objNumber);
		}
		GameObject obj =  NGUITools.AddChild(gameObject,enemyLine);
		obj.GetComponent<EnemyObjectScript>().initialEnemyObjectScript();
		//obj.transform.localScale = new Vector3(0.003472f,0.003472f,1f);
		obj.name = enemyLine.name+"_Pooler   "+objNumber  ;
		obj.SetActive(true);
		enemyList.Add (obj);
		Debug.Log("enemy pooler body pos"+ obj.GetComponent<Animator>().deltaPosition);
		//Debug.Log("usingEnenmyList(new)  is  "+usingEnemyList.Count);
		usingEnemyList.Add(obj);
		return obj;

	}

	public List<GameObject> getUsingEnemyList(){
		return usingEnemyList;
	}

	public void pauseEnemy(){
		foreach(GameObject obj in enemyList){
			if(obj.activeInHierarchy){
				obj.GetComponent<Animator>().speed=0;
				pauseEnemyList.Add(obj);
			}
		}
	}

	public void resumeEnemy(){
		foreach(GameObject obj in pauseEnemyList){
			obj.GetComponent<Animator>().speed=1;
		}
		pauseEnemyList.Clear();
	}
		
	public void onCheckEnemyInput(int value,float sliderPos){
		//Debug.Log("check input "+gameObject+" using enemyList "+usingEnemyList[0]);
		usingEnemyList[0].GetComponent<EnemyObjectScript>().onCheckInputEnemy(value,sliderPos);
			//onSetSelectionEffect(value);
			//usingEnemyList.RemoveAt(0);

	}

}