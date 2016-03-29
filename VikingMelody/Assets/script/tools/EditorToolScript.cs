using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System;

public class EditorToolScript : MonoBehaviour {
	public AudioClip sound;

	private int level=0;
	private float enemyFrequency=0f;
	private int BPM=0;
	private int maxSigns =0;

	private List<int> existingLevelList = new List<int>();
	private Dictionary<int,int>existingLevelDic = new Dictionary<int, int>();
	private Dictionary<int,float> existingEnemyFrequencyList = new Dictionary<int, float>();
	private Dictionary<int,int> existingBpmList = new Dictionary<int, int>();
	private Dictionary<int,int> existingMaxSignsList = new Dictionary<int, int>();
	private Dictionary<int,string> existingSongNameList = new Dictionary<int, string>();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onSubmitLevel(){
		Debug.Log( UIInput.current.value);
		if(UIInput.current.value!=""){
			level =  int.Parse( UIInput.current.value);
			Debug.Log("level "+level);
		}
		//UIInput.current.value=null;
	}

	public void onSubmitEnemyFrequency(){
		Debug.Log( UIInput.current.value);
		if(UIInput.current.value!=""){
			enemyFrequency = float.Parse(UIInput.current.value);
			Debug.Log("enemyFrequency "+enemyFrequency);
			//UIInput.current.value=null;
		}
	}
	public void onSubmitBPM(){
		Debug.Log( UIInput.current.value);
		if(UIInput.current.value!=""){
			BPM =int.Parse(UIInput.current.value);
			Debug.Log("BPM "+BPM);
		//UIInput.current.value=null;
		}
	}
	public void onSubmitMaxSigns(){
		Debug.Log( UIInput.current.value);
		if(UIInput.current.value!=""){
			maxSigns =int.Parse(UIInput.current.value);
			Debug.Log("maxSigns "+maxSigns);
	//	UIInput.current.value=null;
		}
	}

	public void onGenerate(){
		string songPath = "GameLevelData/GameLevel";
		TextAsset data = Resources.Load(songPath) as TextAsset;
		if(data!=null){
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(data.text);
			for(int i=0;i<xmlDoc.SelectNodes("Stage/Level").Count;i++){
				XmlNode stageNode = xmlDoc.SelectNodes("Stage/Level")[i]; 
				existingLevelList.Add(int.Parse(stageNode.Attributes["level"].Value));
				existingEnemyFrequencyList.Add(int.Parse(stageNode.Attributes["level"].Value),float.Parse(stageNode.Attributes["enemyFrequency"].Value));
				existingBpmList.Add(int.Parse(stageNode.Attributes["level"].Value),int.Parse(stageNode.Attributes["bpm"].Value));
				existingMaxSignsList.Add(int.Parse(stageNode.Attributes["level"].Value),int.Parse(stageNode.Attributes["maxSigns"].Value));
				existingSongNameList.Add(int.Parse(stageNode.Attributes["level"].Value),stageNode.Attributes["SongName"].Value);
			}
			string xmlString ="<?xml version="+'"'+"1.0"+'"'+" encoding="+'"'+"utf-8"+'"'+"?> \n"+
				"<!-- Level mean level of stage \n" +
				"SongName mean name os song each stage\n" +
				"EnemyFrequency mean how namy enemy fall per secs\n" +
				"BPM mean song's bpm\n"+
				"MaxSigns mean max number of enemy's signs has fall each \n" +
				"-->\n"+
				"<Stage >\n";
			if(existingLevelList.Contains(level)){
				//edit existing value
				Debug.Log("Edit value at level "+level);
				existingLevelList.Remove(level);
				existingBpmList.Remove(level);
				existingEnemyFrequencyList.Remove(level);
				existingMaxSignsList.Remove(level);
				existingSongNameList.Remove(level);
				existingLevelList.Add(level);
				existingLevelDic.Add(level,level);
				existingEnemyFrequencyList.Add(level,enemyFrequency);
				existingBpmList.Add(level,BPM);
				existingMaxSignsList.Add(level,maxSigns);
				existingSongNameList.Add(level,sound.name);
				existingLevelList.Sort();
//				existingEnemyFrequencyList.Sort();
//				existingBpmList.Sort();
//				existingMaxSignsList.Sort();
//				existingSongNameList.Sort();
			}else{
				//add more
				Debug.Log("Add more value at level "+level);
				existingLevelList.Add(level);
				existingEnemyFrequencyList.Add(level,enemyFrequency);
				existingBpmList.Add(level,BPM);
				existingMaxSignsList.Add(level,maxSigns);
				existingSongNameList.Add(level,sound.name);
			}

			for(int i =0;i<existingLevelList.Count;i++){
				xmlString+="<Level level = "+'"'+existingLevelList[i]+'"'+ " enemyFrequency = "+'"'+existingEnemyFrequencyList[existingLevelList[i]]+
					'"'+" bpm = "+'"'+existingBpmList[existingLevelList[i]]+'"'+" maxSigns = "+'"'+existingMaxSignsList[existingLevelList[i]]+'"'+" SongName ="
					+'"'+existingSongNameList[existingLevelList[i]]+'"'+ "></Level>";
			}
			xmlString+="</Stage>";
		//	XmlDocument xmlDocEdit = new XmlDocument();
			xmlDoc.LoadXml(xmlString);
			xmlDoc.Save(Application.dataPath +"/Resources/GameLevelData/GameLevel.xml");


		}else if(data==null){
			//create new
			string xmlString ="<?xml version="+'"'+"1.0"+'"'+" encoding="+'"'+"utf-8"+'"'+"?> \n"+
				"<!-- Level mean level of stage \n" +
				"SongName mean name os song each stage\n" +
				"EnemyFrequency mean how namy enemy fall per secs\n" +
				"BPM mean song's bpm\n"+
				"MaxSigns mean max number of enemy's signs has fall each \n" +
				"-->\n" +
				"<Stage >\n"+
				"<Level level = "+'"'+level+'"'+ " enemyFrequency = "+'"'+enemyFrequency+'"'+" bpm = "+'"'+BPM+'"'+" maxSigns = "+'"'+maxSigns+'"'+" SongName ="
				+'"'+sound.name+'"'+ "></Level>\n</Stage>";
			XmlDocument xmldoc = new XmlDocument();
			Debug.Log(xmlString);
			xmldoc.LoadXml(xmlString);
			xmldoc.Save(Application.dataPath +"/Resources/GameLevelData/GameLevel.xml");
		}
	}
}
