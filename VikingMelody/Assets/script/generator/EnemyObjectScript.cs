using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyObjectScript : MonoBehaviour {
	public GameObject mainGame;
	public ParticleSystem boomEffect;
	public ComboGenerator comboGeneratorMiss;
	public List<GameObject> enemyCharacterList;

	private List<GameObject>activeEnemy;
	private GameManager gameManager=GameManager.getSingleton();


	public void initialEnemyObjectScript(){
		//Debug.Log("initial active enemy");
		if(activeEnemy==null){
			activeEnemy=new List<GameObject>();
		}
		activeEnemy.Clear();
	}

	public void onSetEnemy(){
		//UnC=unlock character 
		if(PlayerPrefs.GetInt("UnC")>0){
			foreach(GameObject obj in enemyCharacterList){
				obj.SetActive(false);
			}
			int random = Random.Range(0,PlayerPrefs.GetInt("UnC")+1);
			//Debug.Log("Active enemy  "+enemyCharacterList[random]);
			enemyCharacterList[random].SetActive(true);
			activeEnemy.Add(enemyCharacterList[random]);
			//Debug.Log("add active enemy.count "+activeEnemy.Count);
			enemyCharacterList[random].GetComponent<EnemySignCreater>().onCreateSign();
		}else{
			enemyCharacterList[0].GetComponent<EnemySignCreater>().onCreateSign();
		}
	}

	public void onCheckInputEnemy(int value,float sliderPos){
		if(activeEnemy.Count>0){
			//Debug.Log("check on enemy "+activeEnemy[0]);
			if(activeEnemy[0].GetComponent<EnemySignCreater>().onCheckSign(value,sliderPos)){
			}
		}
	}
		
	public void destroyEnemy(){
		if(gameObject.activeInHierarchy){
			foreach(GameObject effect in enemyCharacterList){
				effect.GetComponent<EnemySignCreater>().onDestroyEffect();
			}
			comboGeneratorMiss.transform.position=gameObject.transform.position;
			comboGeneratorMiss.createEffect(HitType.MISS);
			gameObject.SetActive(false);
			Debug.Log(gameObject+" MISS !!");
			activeEnemy[0].GetComponent<EnemySignCreater>().onDestroyEnemy();
			activeEnemy.RemoveAt(0);
			gameManager.setIsFirst(true);
			gameObject.GetComponentInParent<EnemyGenerator>().getUsingEnemyList().RemoveAt(0);
			gameManager.getGeneratorOrderList().RemoveAt(0);
			mainGame.GetComponent<MainGame>().HPgenerator.onDestroyHP();
		}
	}

	public void onResetEffect(){
		foreach(GameObject effect in enemyCharacterList){
			effect.GetComponent<EnemySignCreater>().onDestroyEffect();
		}
	}

	public void onActiveDestroy(){
		boomEffect.transform.position=gameObject.transform.position;
		boomEffect.Play();
		//	Debug.Log(gameObject+"    active destroy!!!!!    "+boomEffect.isPlaying);
		gameObject.SetActive(false);
		activeEnemy[0].GetComponent<EnemySignCreater>().onDestroyEnemy();
		activeEnemy.RemoveAt(0);
		//boomEffect.Play();
	}
		
	public GameObject getAuraEffectSelectionList(int index){
		return enemyCharacterList[index].GetComponent<EnemySignCreater>().getAuraInChild(index);
	}

}
