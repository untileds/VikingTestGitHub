using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResultScript : MonoBehaviour {
	public List<GameObject> starSpriteList ;
	// Use this for initialization
	void Start () {

	}
	public void setShowStar(int index){
		if(index<=starSpriteList.Count){
			for(int i=0;i<index;i++){
				starSpriteList[i].SetActive(true);
			}	
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
