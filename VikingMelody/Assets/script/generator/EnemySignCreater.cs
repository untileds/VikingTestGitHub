using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySignCreater : MonoBehaviour {
	public List<GameObject>SignList;
	public MainGame mainGame;
	private int correctInput=0;
	private int tempLoop=0;
	private int multiScore=0;
	private GameManager gameManager = GameManager.getSingleton();
	public void onCreateSign(){
		foreach(GameObject sign in SignList){
			sign.GetComponent<EnemyProperties>().setEnemySign();
		}
	}

	public void onDestroyEffect(){
		foreach(GameObject sign in SignList){
			sign.GetComponent<EnemyProperties>().destroyAura();
		}
	}

	public GameObject getAuraInChild(int index){
		return SignList[index].GetComponent<EnemyProperties>().getAuraEffect();
	}

	public void setAuraActive(int index){
		Debug.Log("aura index active "+SignList[index]+" on index "+index);
		SignList[index].GetComponent<EnemyProperties>().getAuraEffect().SetActive(true);
	}

	public void resetAuraEffect(){
		Debug.Log(gameObject+" reset effect");
		foreach(GameObject effect in SignList){
			effect.GetComponent<EnemyProperties>().getAuraEffect().SetActive(false);
		}
	}

	public string getGameObjetName(){
		return gameObject.name;
	}

	public bool onCheckSign(int value,float sliderPos){
		//Debug.Log("Check on sign "+gameObject);
		for(int i=tempLoop;i<SignList.Count;i++){
			if(SignList[i].activeInHierarchy){
				if(GameManager.getSingleton().getEnemyList()[i]==value){
					if(checkSidePosition(value,sliderPos)){
						tempLoop++;
						correctInput++;
						setAuraActive(i);
						//Debug.Log("correct input "+correctInput);
						if(correctInput==SignList.Count){
							gameObject.GetComponentInParent<EnemyObjectScript>().onActiveDestroy();
							//Debug.Log("remove usingEnemyList"+ mainGame.generatorList[gameManager.getGeneratorOrderList()[0]].getUsingEnemyList()[0]);
							mainGame.generatorList[gameManager.getGeneratorOrderList()[0]].getUsingEnemyList().RemoveAt(0);
							gameManager.getGeneratorOrderList().RemoveAt(0);
							gameManager.addGameScore(multiScore);
							mainGame.scoreLabel.GetComponent<UILabel>().text ="score  "+gameManager.getGameScore().ToString();
							tempLoop=0;
							correctInput=0;
							multiScore=0;
							resetAuraEffect();
						}
						return true;
					}
				}else{
					tempLoop=0;
					correctInput=0;
					resetAuraEffect();
					mainGame.characterList[value].GetComponent<ButtonInputProperties>().onCreateEffect(HitType.MISS);
					//Debug.Log("Miss input");
					return false;
				}
			}
		}return false;
	}

	private bool checkSidePosition(int character,float sliderPos){
		HitType hitType = HitType.NONE;
		//Debug.Log("slide position"+sliderPos);
		if(sliderPos<=((gameManager.getGameConstant().getPerfectSlide()*gameManager.getSlideWidth())/100)){
			//perfect left
			hitType=HitType.PERFECT;
			mainGame.characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			multiScore+=(gameManager.getScoreByGameLevel()*gameManager.getGameConstant().getperfectMultiplier());
			gameManager.increasePerfectCount();
			//Debug.Log("PERFECT LEFT "+multiScore);
			return true;
		}else if(sliderPos<=(gameManager.getGameConstant().getGreatSlide()*gameManager.getSlideWidth())/100){
			//great left
			hitType=HitType.GREAT;
			mainGame.characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			multiScore+=(gameManager.getScoreByGameLevel());
			//Debug.Log("GREAT LEFT "+multiScore);
			return true;
		}else if(sliderPos<(gameManager.getGameConstant().getSliceMiss()*gameManager.getSlideWidth())/100){
			//miss
			//Debug.Log("MISS");
			hitType=HitType.MISS;
			mainGame.characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			//		resetEffect();
			gameManager.setIsFirst(true);
			return false;
		}else if(sliderPos>=gameManager.getSlideWidth() - (gameManager.getGameConstant().getGreatSlide()*gameManager.getSlideWidth())/100&&
			sliderPos<gameManager.getSlideWidth() - (gameManager.getGameConstant().getPerfectSlide()*gameManager.getSlideWidth())/100){
			//great right
			hitType=HitType.GREAT;
			mainGame.characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			multiScore+=(gameManager.getScoreByGameLevel());
			//Debug.Log("GREAT RIGHT "+multiScore);
			return true;
		}else if(	sliderPos>=gameManager.getSlideWidth() - (gameManager.getGameConstant().getPerfectSlide()*gameManager.getSlideWidth())/100){
			//perfect right
			hitType=HitType.PERFECT;
			mainGame.characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			multiScore+=(gameManager.getScoreByGameLevel()*gameManager.getGameConstant().getperfectMultiplier());
			gameManager.increasePerfectCount();
			//Debug.Log("PERFECT RIGHT "+multiScore);
			return true;
		}
		return false;
	}

	public void onDestroyEnemy(){
		for(int i =0;i<SignList.Count;i++){
			//Debug.Log("remove enemy at "+gameObject.name);
			GameManager.getSingleton().onRemoveEnemy();
		}
	}

}
