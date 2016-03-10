using UnityEngine;
using System.Collections;

public class HitEffectObject : MonoBehaviour {
	TweenPosition tweenPosition;
	TweenScale tweenScale;
	TweenAlpha tweenAlpha;
	// Use this for initialization

	public void OnEnable(){
		if(GetComponent<TweenAlpha>()!= null){
			GetComponent<TweenAlpha>().ResetToBeginning();
			GetComponent<TweenAlpha>().PlayForward();
		}
		if(GetComponent<TweenPosition>()!= null){
			GetComponent<TweenPosition>().ResetToBeginning();
			GetComponent<TweenPosition>().PlayForward();
		}
		if(GetComponent<TweenScale>()!= null){
			GetComponent<TweenScale>().ResetToBeginning();
			GetComponent<TweenScale>().PlayForward();
		}

	}

	public void onDoneAnimate(){
		gameObject.SetActive(false);


	}
}
