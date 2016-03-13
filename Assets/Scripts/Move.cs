using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public FreeParallax fp;
	public bool changeDirection;
	int direction = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (changeDirection) {
			direction *= -1;
			fp.Speed *= direction;
			changeDirection = false;
		} else {
			this.transform.Translate (new Vector3(direction * 5,0,0)* Time.deltaTime);
		}
	}
}
