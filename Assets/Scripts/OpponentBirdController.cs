using UnityEngine;
using System.Collections;

public class OpponentBirdController : MonoBehaviour {
	public Sprite[] birdSprites;
	 
 
	 

	public void SetBirdNumber (int birdNum) {
		GetComponent<SpriteRenderer>().sprite = birdSprites[birdNum-1];
	}

	public void SetBirdInformation(float posX, float posY, float velX, float velY, float rotZ) {
		transform.position = new Vector3 (posX, posY, 0);
		transform.rotation = Quaternion.Euler (0, 0, rotZ);
	}

	 
}
