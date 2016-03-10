using UnityEngine;
using System.Collections;

public class GameConstant {
	// percen(%) of length each stage,devide by slideBarWidth
	private const float slidePerfect=20.83f;
	private const float slideGreat=41.66f;
	private const float slideMiss =58.33f;
	private const int perfectMultiplier=2;
	private const float gameDuration =60f;

	private const int easyScore = 200;
	private const int normalScore = 300;
	private const int hardScore = 500;
	private const int HPIndexBegin = 3;

	public float getGameDuration(){
		return gameDuration;
	}

	public int getperfectMultiplier(){
		return perfectMultiplier;
	}

	public float getSliceMiss(){
		return slideMiss;
	}
		
	public float getPerfectSlide(){
		return slidePerfect;
	}

	public float getGreatSlide(){
		return slideGreat;
	}

	public int getHPIndex(){
		return HPIndexBegin;
	}

	public int getEasyScore(){
		return easyScore;
	}

	public int getNormalScore(){
		return normalScore;
	}

	public int getHardScore(){
		return hardScore;
	}

}
