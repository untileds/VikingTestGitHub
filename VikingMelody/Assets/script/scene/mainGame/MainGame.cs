using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainGame : MonoBehaviour {
	public AudioSource soundBG;
	public GameObject gameDurationLabel;
	public GameObject slideBarOBJ;
	public TweenPosition sliderBar;
	public ParticleSystem testParticle;
	public HitPointGenerator HPgenerator;
	public List<EnemyGenerator> generatorList;
	public GameObject startGameLabel;
	public GameObject countDownLabel;
	public GameObject scoreLabel;
	public GameObject pauseDialog;
	public GameObject resultDialog;
	public GameObject pauseButton;
	public List<GameObject> characterList;

	private int enemyCount;
	private int countDown;
	private float gameTime=0f;
	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		initial();
		InvokeRepeating("callGenerator",0f,5f);
		sliderBar.duration=60/140f;
	}
	// Update is called once per frame
	void Update () {
		if(gameManager.getGameOver()&&!gameManager.isStartGame){
			//lose game
			gameManager.isFinish=false;
			onPauseEndGame();
			resultDialog.GetComponentInChildren<UILabel>().text=gameManager.getGameScore().ToString();
			//setCharacter(false);
		}else if(!gameManager.getGameOver()&&gameManager.isStartGame){
			//game is playing
			//	resultDialog.SetActive(false);
			//setCharacter(true);
			gameTime+=Time.deltaTime;
			gameDurationLabel.GetComponent<UILabel>().text="TIME: 60s :: "+gameTime.ToString("F1")+" s";
			if(gameTime>=gameManager.getGameConstant().getGameDuration()){
				//endgame then shows result 
				if(!gameManager.isFinish){
					Debug.Log("Finish game show result");
					gameManager.isFinish=true;
					onPauseEndGame();
				}
			}else if(gameTime>=(gameManager.getGameConstant().getGameDuration()-gameManager.getGameConstant().getDelayGameDuration())){
				//set to doesn't create enemy
				if(gameManager.isGenaratable){
					Debug.Log("End generators");
					gameManager.isGenaratable=false;
				}
			}
		}

//		if(!gameManager.isStartGame){
//			pauseButton.SetActive(false);
//			foreach(GameObject character in characterList){
//				character.GetComponent<BoxCollider>().enabled=false;
//			}
//		}
	}

	private void initial(){
		resultDialog.SetActive(false);
		gameManager=GameManager.getSingleton();
		gameManager.onSetSlideBarWidth(slideBarOBJ.GetComponent<UISprite>().width);
		gameManager.resetScore();
		gameManager.setIsFirst(true);
		gameManager.setIsGameOver(false);
		gameManager.isGenaratable=true;
		gameManager.resetStatus();
		setGameLevel();
		gameTime=0;
		countDown=3;
		enemyCount=0;
		setCharacter(false);
		gameManager.resetEnemyDict();
		foreach(EnemyGenerator temp in generatorList){
			temp.initialGenerator();
		}
		HPgenerator.HPinitial();
		HPgenerator.createHP();

		//character initialization
		if(gameManager.getCharacterSelectedList().Count>0){
			float defaultXpos =0f;
			List<string>nameList=gameManager.getCharacterSelectedList();
			//reset character
			foreach(GameObject character in characterList){
				character.SetActive(false);
			}

			//set sprite name
			for(int i=0;i<nameList.Count;i++){
				characterList[i].transform.FindChild("characterSprite").GetComponent<UISprite>().spriteName=nameList[i];
			}
			//set position
			if(nameList.Count==3){
				//3pl position
				defaultXpos =-240f;
				for(int i=0;i<nameList.Count;i++){
					characterList[i].transform.localPosition=new Vector2(defaultXpos,0);
					characterList[i].SetActive(true);
					defaultXpos+=260f;
				}
			}else if(nameList.Count==4){
				//4pl position
				defaultXpos =-300f;
				for(int i=0;i<nameList.Count;i++){
					characterList[i].transform.localPosition=new Vector2(defaultXpos,0);
					characterList[i].SetActive(true);
					defaultXpos+=200f;
				}
			}else if(nameList.Count==2){
				//2pl position
				defaultXpos =-120f;
				for(int i=0;i<nameList.Count;i++){
					characterList[i].transform.localPosition=new Vector2(defaultXpos,0);
					characterList[i].SetActive(true);
					defaultXpos+=260f;
				}
			}
		}
	}

	private void setCharacter(bool status){
			foreach(GameObject character in characterList){
			character.GetComponentInChildren<TweenScale>().ResetToBeginning();
				character.GetComponentInChildren<TweenScale>().enabled=status;
			character.GetComponentInChildren<TweenScale>().duration=60/140f;
			if(character.GetComponentInChildren<TweenScale>().enabled){
				character.GetComponentInChildren<TweenScale>().PlayForward();
			}
		}
	}

	public void onRestartGame(){
		gameDurationLabel.GetComponent<UILabel>().text="TIME: ";
		gameManager.resetGeneratorOrderList();
		//Debug.Log("generatorOrder is "+generatorOrderList.Count);
		setCharacter(true);
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
		sliderBar.ResetToBeginning();
	}

	private void setGameLevel(){
		gameManager.setGameLevel(GameLevel.EASY);
	}

	private void callGenerator(){
		if(gameManager.isStartGame&&gameManager.isGenaratable){
			int random = Random.Range(0,generatorList.Count);
			//Debug.Log("random create generator "+random);
			generatorList[random].creatEnemy();
			enemyCount++;
			gameManager.getGeneratorOrderList().Add(random);
			//Debug.Log(gameManager.getGeneratorOrderList().Count);
		}
	}

	public void onCheckInput(int value){
		float slideTemp = sliderBar.transform.localPosition.x;
		//Debug.Log("go on generator"+generatorList[gameManager.getGeneratorOrderList()[0]]);
		if(gameManager.getGeneratorOrderList().Count>0){
		//	Debug.Log("onCheckIput in mainGame and generator.count"+gameManager.getGeneratorOrderList().Count);
			generatorList[gameManager.getGeneratorOrderList()[0]].onCheckEnemyInput(value,slideTemp);
		}
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
//			PlayerPrefs.SetInt("WIN",gameManager.getCurrentStage());
//			PlayerPrefs.Save();
			gameManager.unlockNextStage();
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
		onFinishGame();
		sliderBar.enabled=false;
		soundBG.Pause();
		setCharacter(false);
		if(!gameManager.isStartGame){
			foreach(EnemyGenerator enemy in generatorList){
				enemy.pauseEnemy();
			}
		}
	}

	public void onPauseButton(){
		if(gameManager.isStartGame){
			soundBG.Pause();
			sliderBar.enabled=false;
			gameManager.isStartGame=false;
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
			sliderBar.enabled=true;
			gameManager.isStartGame=true;
			foreach(EnemyGenerator enemy in generatorList){
				enemy.resumeEnemy();
			}
			setDialogPlay();
			setCharacter(true);
		}
	}

	private void setDialogPlay(){
		pauseDialog.SetActive(false);
		pauseButton.SetActive(true);
		foreach(GameObject character in characterList){
			character.GetComponent<BoxCollider>().enabled=true;
		}
	}

	private void setDialogPause(){
		pauseButton.SetActive(false);
		foreach(GameObject character in characterList){
			character.GetComponent<BoxCollider>().enabled=false;
		}
	}

	public void onCountDownEvent(){
		countDown--;
		if(countDown==0){
			gameManager.isStartGame=true;
			setCharacter(true);
			Debug.Log("Time now"+Time.time);
			soundBG.Stop();
			if(!sliderBar.isActiveAndEnabled){
				sliderBar.enabled=true;
			}
			sliderBar.PlayForward();
			soundBG.Play();
			countDownLabel.SetActive(false);
			setDialogPlay();
		}
		countDownLabel.GetComponent<UILabel>().text=countDown.ToString();
		countDownLabel.GetComponent<TweenScale>().ResetToBeginning();
		countDownLabel.GetComponent<TweenScale>().PlayForward();
	}
}