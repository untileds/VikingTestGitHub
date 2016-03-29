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

	public bool isGenaratable = true;
	public bool isStartGame=false;
	public bool isFinish=false;
	public bool isWinGame=false;
	public List<int> enemyList = new List<int>();
	public List<string>removeEnemyList = new List<string>();

	private List<int> generatorOrderList = new List<int>();
	private List<string>characterSelectedList = new List<string>();
	private int selectIndex =0;
	private int perfectCount=0;
	private static GameManager gameManager;
	private GameConstant gameConstant;
	private GameLevel gameLevel = GameLevel.NONE;
	private bool isGameOver=false;
	private bool isFirst =true;
	private int stageNumber=0;
	private int unlockedStage=0;
	private int slideBarWidth=0;
	private int gameScore=0;


	public void addGameScore(int value){
		gameScore+=value;
	}

	public int getGameScore(){
		return gameScore;
	}

	public void resetScore(){
		gameScore=0;
	}

	public void onSetSlideBarWidth(int value){
		slideBarWidth=value;
	}

	public int getSlideWidth(){
		return slideBarWidth;
	}

	public List<int> getGeneratorOrderList(){
		return generatorOrderList;
	}

	public void resetGeneratorOrderList(){
		generatorOrderList.Clear();
	}

	public int getSelectIndex(){
		Debug.Log("index "+selectIndex);
		return selectIndex;
	}

	public void increaseSelectIndex(){
		selectIndex++;
	}

	public void setStage(int stage){
		stageNumber=stage;	
	}

	public List<string> getCharacterSelectedList(){
		return characterSelectedList;
	}

	public void addCharacterListByName(string name){
		characterSelectedList.Add(name);
	}

	public void resetCharacterSelectedList(){
		characterSelectedList.Clear();
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
		unlockedStage++;
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

	public void resetEnemyDict(){
		enemyList.Clear();
		removeEnemyList.Clear();
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
		
	public void addEnemyList(string name,int position){
		//Debug.Log("Add  position "+position);
		enemyList.Add(position);
		removeEnemyList.Add(name);
	}

	public void onRemoveEnemy(){
		//Debug.Log("Remove enemy "+removeEnemyList[0]+" value position "+enemyList[0]);
		enemyList.RemoveAt(0);
		removeEnemyList.RemoveAt(0);

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
