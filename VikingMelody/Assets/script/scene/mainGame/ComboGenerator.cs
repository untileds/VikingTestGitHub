using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ComboGenerator : MonoBehaviour {
	public GameObject comboLabel; 
	public GameObject perfectParent;
	public GameObject greatParent;
	public GameObject missParent;
	public GameObject perfectChild;
	public GameObject greatChild;
	public GameObject missChild;

//	private GameManager gameManager;
	private List<GameObject> perfectEffectList = new List<GameObject>();
	private List<GameObject> greatEffectList = new List<GameObject>();
	private List<GameObject> missEffectList = new List<GameObject>();
	// Use this for initialization
	void Start () {
		//gameManager = GameManager.getSingleton();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void createEffect(HitType hitType){
		GameObject sfTemp;
		switch(hitType){
		case HitType.PERFECT	: sfTemp = effectPooler(perfectEffectList,perfectParent,perfectChild);break;
		case HitType.GREAT		: sfTemp =effectPooler(greatEffectList,greatParent,greatChild);break;
		case HitType.MISS			: sfTemp =effectPooler(missEffectList,missParent,missChild);break;
		}
	}

	public GameObject effectPooler(List<GameObject> effectList, GameObject parent, GameObject child){
		//showComboLabel();

		for (int i = 0; i< effectList.Count;i++){
			if(!effectList[i].activeInHierarchy){
				effectList[i].transform.position = child.transform.position;
				effectList[i].SetActive(true);
				return effectList[i];
			}
		}
		GameObject fx = NGUITools.AddChild(parent,child);
		fx.name = child.name;
		fx.SetActive(true);
		effectList.Add (fx);
		return fx;

	}

//	private void showComboLabel(){
//		if(gameManager.getComboCount()>0){
//			comboLabel.SetActive(true);
//			tweenScaleCombo.ResetToBeginning();
//			tweenScaleCombo.PlayForward();
//			comboLabel.GetComponent<UILabel>().text=gameManager.getComboCount().ToString();
//		}else{
//			comboLabel.SetActive(false);
//		}
//	}
}
