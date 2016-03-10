using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum GameLevel{
	NONE=0,
	EASY,
	NORMAL,
	HARD
}
public enum HitType{
	NONE=0,
	GREAT,
	PERFECT,
	MISS

}

public class GameManager{
	public bool isStartGame=false;
	public bool isFinish=false;
	public List<int> enemyList = new List<int>();

	private int perfectCount=0;
	private static GameManager gameManager;
	private GameConstant gameConstant;
	private GameLevel gameLevel = GameLevel.NONE;
	private bool isGameOver=false;
	private bool isFirst =true;
	private int stageNumber=PlayerPrefs.GetInt("Sn");
	private int unlockedStage=PlayerPrefs.GetInt("UnS");
	public void setStage(int stage){
		stageNumber=stage;	
	}
	public int getCurrentStage(){
		Debug.Log("current stage "+stageNumber);
		return stageNumber; 
	}
	public int getUnlockedStage(){
		Debug.Log("unlocked stage "+stageNumber);
		return unlockedStage; 
	}

	public void unlockNextStage(){
		//UnS unlocked stage
		unlockedStage+=1;
		Debug.Log("unlock stage "+unlockedStage);
		PlayerPrefs.SetInt("UnS",unlockedStage);
		PlayerPrefs.Save();
	}
	public void increasePerfectCount(){
		perfectCount++;
	}

	public void resetStatus(){
		perfectCount=0;
	}

	public int getPerfectCount(){
		return perfectCount;
	}

	public void setIsGameOver(bool status){
		isGameOver = status;
	}

	public bool getGameOver(){
		return isGameOver;
	}

	public bool getIsFirst(){
		return isFirst;
	}

	public void setIsFirst(bool status){
		isFirst = status;
	}

	public void setGameLevel(GameLevel level){
		gameLevel = level;
	}

	public GameLevel getGameLevel(){
		return gameLevel;
	}

	public void resetEnemyList(){
		enemyList.Clear();
	}

	public List<int> getEnemyList(){
		return enemyList;
	}

	public static GameManager getSingleton(){
		if(gameManager==null){
			gameManager= new GameManager();
		}
		return gameManager;
	}

	public GameConstant getGameConstant(){
		if(gameConstant==null){
			gameConstant=new GameConstant();
		}
		return gameConstant;
	}

	public void setIsStartGame(bool status){
		isStartGame=status;
	}

	public void addEnemyList(int position){
		//Debug.Log("Add  position "+position);
		enemyList.Add(position);
	}

	public int getScoreByGameLevel(){
		gameConstant = getGameConstant();
		switch(gameLevel){
		case GameLevel.EASY : return gameConstant.getEasyScore();
		case GameLevel.NORMAL : return gameConstant.getNormalScore();
		case GameLevel.HARD : return gameConstant.getHardScore();
		default :return 0;
		}
	}

}
