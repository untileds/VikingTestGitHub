using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitPointGenerator : MonoBehaviour {
	private List<GameObject> HitPointList= new List<GameObject>();
	private List<GameObject> HitPointIndexList= new List<GameObject>();
	private GameManager gameManager;

	public GameObject HitpointLabel;
	public GameObject HpObjectParent;
	public GameObject HpObjectChild;
	private int hpTemp;
	private float posX= -40f;
	// Use this for initialization
	void Start () {
//		HPinitial();
//		//HpObjectParent.SetActive(true);
//		createHP();
	}

	public void resetHP(){
		if(HitPointList!=null){
			foreach(GameObject hp in HitPointList){
				Destroy(hp);
			}
			HitPointList.Clear();
			HitPointIndexList.Clear();

		}
	}

	public void HPinitial(){
		gameManager=GameManager.getSingleton();
		hpTemp=1;
//		HitPointList.Clear();
//		HitPointIndexList.Clear();
	}

	public void createHP(){
		posX= -40f;
		HitpointLabel.SetActive(true);

		for(int i =0;i<gameManager.getCharacterSelectedList().Count;i++){
		GameObject obj =  NGUITools.AddChild(gameObject,HpObjectParent);
		obj.name = HpObjectParent.name+"_BeatPooler   "+i  ;
			//obj.transform.localPosition= new Vector2(0,posY);
		obj.SetActive(true);
		createHPIndex(obj,HpObjectChild);
		HitPointList.Add (obj);
		//	posY +=(-20);
		}
	}
		
	public void createHPIndex(GameObject parent, GameObject child){
		for(int i =0;i < gameManager.getGameConstant().getHPIndex();i++){
			GameObject obj =  NGUITools.AddChild(parent,child);
			//HpObjectChild.transform.localPosition= new Vector2(posX,0);
			//obj.transform.localScale = new Vector3(0.003472f,0.003472f,1f);
			obj.name = HpObjectChild.name+"_BeatPooler   "+i  ;
			obj.transform.localPosition= new Vector2(posX,0);
			obj.SetActive(true);
			HitPointIndexList.Add (obj);
			//Debug.Log("add "+obj.name);
			posX+=40f;
		}
	}

	public void onDestroyHP(){
		//Debug.Log("HitpointIndexList.Count "+HitPointIndexList.Count);
		if(HitPointIndexList.Count-hpTemp>=0){
		//	Debug.Log("decrease HP");
			HitPointIndexList[HitPointIndexList.Count-hpTemp].SetActive(false);
			if(HitPointIndexList.Count-hpTemp==0){
				Debug.Log("GAME OVER");
				gameManager.isStartGame=false;
				gameManager.setIsGameOver(true);

			}
			hpTemp++;
		}
	}



	// Update is called once per frame
	void Update () {
	
	}
}
