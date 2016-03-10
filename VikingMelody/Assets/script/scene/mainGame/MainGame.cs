using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainGame : MonoBehaviour {
	public AudioSource soundBG;
	public GameObject gameDurationLabel;
	public GameObject slideBarOBJ;
	public TweenPosition slideBar;
	public ParticleSystem testParticle;
	public HitPointGenerator HPgenerator;
	public List<EnemyGenerator> generatorList;
	public GameObject startGameLabel;
	public GameObject countDownLabel;
	public GameObject scoreLabel;
	public GameObject pauseDialog;
	public GameObject resultDialog;
	public GameObject pauseButton;
	public ComboGenerator comboGenerator;
	public List<GameObject> characterList;

	private int enemyCount;
	private int slideBarWidth;
	private int tempScoreMulti;
	private int gameScore;
	private int countDown;
	private float gameTime=0f;
	private List<int> generatorOrderList = new List<int>();
	private GameManager gameManager ;
	// Use this for initialization
	void Start () {
		initial();
		InvokeRepeating("callGenerator",0,2);
		slideBar.duration=60/140f;
	}
	// Update is called once per frame
	void Update () {
		if(gameManager.getGameOver()&&!gameManager.isStartGame){
			//lose game
			onPauseEndGame();
			resultDialog.GetComponentInChildren<UILabel>().text=gameScore.ToString();
			onFinishGame();
			setCharacter(false);
		}else if(!gameManager.getGameOver()&&gameManager.isStartGame){
			//game is playing
			resultDialog.SetActive(false);
			setCharacter(true);
			gameTime+=Time.deltaTime;
			gameDurationLabel.GetComponent<UILabel>().text="TIME: 60s :: "+gameTime.ToString("F1")+"s";
			if(gameTime>=gameManager.getGameConstant().getGameDuration()){
				//endgame then shows result 
				onPauseEndGame();
				gameManager.isFinish=true;
				onFinishGame();
			}
		}

		if(!gameManager.isStartGame){
			pauseButton.SetActive(false);
			foreach(GameObject character in HPgenerator.getCharacterList()){
				character.GetComponent<BoxCollider>().enabled=false;
			}
		}
	}

	private void initial(){
		slideBarWidth=slideBarOBJ.GetComponent<UISprite>().width;
		resultDialog.SetActive(false);
		gameManager=GameManager.getSingleton();
		gameManager.setIsFirst(true);
		gameManager.setIsGameOver(false);
		gameManager.resetStatus();
		setGameLevel();
		tempScoreMulti=0;
		gameTime=0;
		gameScore =0;
		countDown=3;
		enemyCount=0;
		gameManager.resetEnemyList();
		foreach(EnemyGenerator temp in generatorList){
			temp.initialGenerator();
		}
		HPgenerator.HPinitial();
		HPgenerator.createHP();
	}

private void setCharacter(bool status){
		foreach(GameObject character in HPgenerator.getCharacterList()){
			character.GetComponentInChildren<TweenScale>().enabled=status;
			character.GetComponentInChildren<TweenScale>().duration=60/140f;
		}
}

	public void onRestartGame(){
		gameDurationLabel.GetComponent<UILabel>().text="TIME: ";
		generatorOrderList.Clear();
		Debug.Log("generatorOrder is "+generatorOrderList.Count);
		pauseDialog.SetActive(false);
		pauseButton.SetActive(true);
		scoreLabel.GetComponent<UILabel>().text="score";
		foreach(EnemyGenerator temp in generatorList){
			temp.restartGenerator();
		}
		HPgenerator.resetHP();
		initial();
		countDownLabel.GetComponent<UILabel>().text="3";
		countDownLabel.SetActive(true);
		slideBar.ResetToBeginning();
	}

	public List<int> getGenaratorOrderList(){
		return generatorOrderList;
	}

	private void setGameLevel(){
		gameManager.setGameLevel(GameLevel.EASY);
	}

	private void callGenerator(){
		if(gameManager.isStartGame){
			int random = Random.Range(0,generatorList.Count);
			generatorList[random].creatEnemy();
			enemyCount++;
			generatorOrderList.Add(random);
		}
	}

	private bool checkSidePosition(int character){
		HitType hitType = HitType.NONE;
		float slideTemp = slideBar.transform.localPosition.x;
		Debug.Log("slide position"+slideTemp);
		if(slideTemp<=((gameManager.getGameConstant().getPerfectSlide()*slideBarWidth)/100)){
			//perfect left
			hitType=HitType.PERFECT;
			characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			tempScoreMulti+=(gameManager.getScoreByGameLevel()*gameManager.getGameConstant().getperfectMultiplier());
			gameManager.increasePerfectCount();
			Debug.Log("PERFECT LEFT "+tempScoreMulti);
			return true;
		}else if(slideTemp<=(gameManager.getGameConstant().getGreatSlide()*slideBarWidth)/100){
			//great left
			hitType=HitType.GREAT;
			characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			tempScoreMulti+=(gameManager.getScoreByGameLevel());
			Debug.Log("GREAT LEFT "+tempScoreMulti);
			return true;
		}else if(slideTemp<(gameManager.getGameConstant().getSliceMiss()*slideBarWidth)/100){
			//miss
			Debug.Log("MISS");
			hitType=HitType.MISS;
			characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			resetEffect();
			gameManager.setIsFirst(true);
			return false;
		}else if(slideTemp>=slideBarWidth - (gameManager.getGameConstant().getGreatSlide()*slideBarWidth)/100&&
					slideTemp<slideBarWidth - (gameManager.getGameConstant().getPerfectSlide()*slideBarWidth)/100){
			//great right
			hitType=HitType.GREAT;
			characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			tempScoreMulti+=(gameManager.getScoreByGameLevel());
			Debug.Log("GREAT RIGHT "+tempScoreMulti);
			return true;
		}else if(	slideTemp>=slideBarWidth - (gameManager.getGameConstant().getPerfectSlide()*slideBarWidth)/100){
			//perfect right
			hitType=HitType.PERFECT;
			characterList[character].GetComponent<ButtonInputProperties>().onCreateEffect(hitType);
			tempScoreMulti+=(gameManager.getScoreByGameLevel()*gameManager.getGameConstant().getperfectMultiplier());
			gameManager.increasePerfectCount();
			Debug.Log("PERFECT RIGHT "+tempScoreMulti);
			return true;
		}
		return false;
	}

	public void onInputClick(int value){
		if(gameManager.getEnemyList().Count>=1){
			//correct first
			if(value==gameManager.getEnemyList()[0]&&gameManager.getIsFirst()){
				if(checkSidePosition(value)){
					generatorList[generatorOrderList[0]].onSetSelectionEffect(0);
					gameManager.setIsFirst(false);
				}
			}else if(value==gameManager.getEnemyList()[1]&&!gameManager.getIsFirst()){
				//correct second
				if(checkSidePosition(value)){
					generatorList[generatorOrderList[0]].onSetSelectionEffect(1);
					generatorList[generatorOrderList[0]].destroyActiveEnemy();
					generatorList[generatorOrderList[0]].onResetEffect();
					generatorOrderList.RemoveAt(0);
					gameManager.setIsFirst(true);
					gameScore+=tempScoreMulti;
					scoreLabel.GetComponent<UILabel>().text ="score  "+gameScore.ToString();
					Debug.Log("Score "+gameScore+"  temp is "+tempScoreMulti);
					tempScoreMulti=0;
				}
			}else{
				//miss
				characterList[value].GetComponent<ButtonInputProperties>().onCreateEffect(HitType.MISS);
				resetEffect();
				gameManager.setIsFirst(true);
			}
		}
	}

	public void resetEffect(){
		generatorList[generatorOrderList[0]].onResetEffect();
	}

	public void onCheckSomthing(){
		testParticle.Play();
	}

	public void onStartGameClick(){
		startGameLabel.SetActive(false);
		countDownLabel.SetActive(true);
		setDialogPlay();
	}

	public void onFinishGame(){
		gameManager.setIsGameOver(true);
		if(gameManager.isFinish){
			// win game
			PlayerPrefs.SetInt("WIN",gameManager.getCurrentStage());
			PlayerPrefs.Save();
			float temp=((float)gameManager.getPerfectCount()/enemyCount)*100;

			if(temp>0f&&temp<50f){
				resultDialog.GetComponent<ResultScript>().setShowStar(1);
			}else if(temp>50f&&temp<60f){
				resultDialog.GetComponent<ResultScript>().setShowStar(2);
			}else if(temp>60f){
				resultDialog.GetComponent<ResultScript>().setShowStar(3);
			}
			resultDialog.transform.FindChild("Status").GetComponent<UILabel>().text="DEBUGGING STATUS Perfect ----- "+gameManager.getPerfectCount()+
				"  All enemy sign----- "+enemyCount*2;
			resultDialog.transform.FindChild("WinLabel").GetComponent<UILabel>().text="YOU WON :D";
		}else{
			resultDialog.transform.FindChild("WinLabel").GetComponent<UILabel>().text="YOU LOSE :(";
			resultDialog.transform.FindChild("Status").GetComponent<UILabel>().text="DEBUGGING STATUS Perfect ----- "+gameManager.getPerfectCount()+
				"  All enemy sign----- "+enemyCount*2;
		}
		resultDialog.SetActive(true);
	}

	public void onPauseEndGame(){
		gameManager.setIsStartGame(false);
		slideBar.enabled=false;
		soundBG.Pause();
		if(!gameManager.isStartGame){
			foreach(EnemyGenerator enemy in generatorList){
				enemy.pauseEnemy();
			}
		}
	}

	public void onPauseButton(){
		if(gameManager.isStartGame){
			soundBG.Pause();
			slideBar.enabled=false;
			gameManager.setIsStartGame(false);
			foreach(EnemyGenerator enemy in generatorList){
				enemy.pauseEnemy();
			}
			setCharacter(false);
			pauseDialog.SetActive(true);
			pauseButton.SetActive(false);
		}
	}

	public void onResume(){
		if(!gameManager.isStartGame){
			soundBG.Play();
			slideBar.enabled=true;
			gameManager.setIsStartGame(true);
			foreach(EnemyGenerator enemy in generatorList){
				enemy.resumeEnemy();
			}
			setDialogPlay();
		//	setCharacter(true);
		}
	}

	private void setDialogPlay(){
		pauseDialog.SetActive(false);
		pauseButton.SetActive(true);
		foreach(GameObject character in HPgenerator.getCharacterList()){
			character.GetComponent<BoxCollider>().enabled=true;
		}
	}

	public void onCountDownEvent(){
		countDown--;
		if(countDown==0){
			gameManager.setIsStartGame(true);
			soundBG.Stop();
			if(!slideBar.isActiveAndEnabled){
				slideBar.enabled=true;
			}
			slideBar.PlayForward();
			soundBG.Play();
			countDownLabel.SetActive(false);
			setDialogPlay();
		}
		countDownLabel.GetComponent<UILabel>().text=countDown.ToString();
		countDownLabel.GetComponent<TweenScale>().ResetToBeginning();
		countDownLabel.GetComponent<TweenScale>().PlayForward();
	}
}