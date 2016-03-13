using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerGoal : MonoBehaviour {
	public Text score;
	int levelScore = 0;
	void Start()
	{
		if (!PlayerPrefs.HasKey ("score")) {
			PlayerPrefs.SetInt ("score", 0);
		} else {
			levelScore = PlayerPrefs.GetInt ("score");
		}
		score.text = levelScore.ToString ();
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "enemy") {
			levelScore++;
			score.text = levelScore.ToString ();
			PlayerPrefs.SetInt ("score", levelScore);
			Application.LoadLevel (Application.loadedLevel);
		}
	}
}
