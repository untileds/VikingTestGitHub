using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
//	public UILabel textLabel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void onTestUnlock2(UILabel textLabel){
		PlayerPrefs.SetInt("UnC",1);
		PlayerPrefs.Save();
		textLabel.text="Unlock complete";
		Debug.Log(PlayerPrefs.GetInt("UnC"));
	}

	public void onResetGameState(UILabel textLabel){
		PlayerPrefs.SetInt("UnC",0);
		PlayerPrefs.Save();
		textLabel.text="reset complete";
		Debug.Log(PlayerPrefs.GetInt("UnC"));
	}

}
