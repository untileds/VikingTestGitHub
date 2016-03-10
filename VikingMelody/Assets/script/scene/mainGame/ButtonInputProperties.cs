using UnityEngine;
using System.Collections;

public class ButtonInputProperties : MonoBehaviour {
	public int value=0;
	public ComboGenerator comboGenerator;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void onCreateEffect(HitType hitType){
		comboGenerator.createEffect(hitType);
	}
	public int getValue(){
		return value;
	}
}
