using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BirdController: MonoBehaviour
{
	Vector3 startPos;
	float startTime;
	public float maxSpeed;
	public Sprite[] birdSprites;
	private float _birdSpeed;
	private bool _paused;
	private bool _stopped = false;
	void FixedUpdate()
	{
		if (this.GetComponent<Rigidbody2D> ().velocity.magnitude > 25)
			this.GetComponent<Rigidbody2D> ().velocity = this.GetComponent<Rigidbody2D> ().velocity.normalized * maxSpeed;
	}

	public void SetBirdChoice (int carNum, bool isMultiplayer) {
		GetComponent<SpriteRenderer> ().sprite = birdSprites [carNum - 1];
	}

	public void SetPaused(bool isPaused)
	{
		_paused = isPaused;
	}

	public void Stop()
	{
		_stopped = true;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

	void Update () 
	{
		
		if (Input.GetMouseButtonDown(0))
		{
			//Store initial values
			startPos = Input.mousePosition;
			startTime = Time.time;
		}

		if (Input.GetMouseButtonUp(0))
		{
			//Get end values
			Vector3 endPos = Input.mousePosition;
			float endTime = Time.time;

			//Mouse positions distance from camera. Might be a better idea to use the cameras near plane
			startPos.z = 0.1f;
			endPos.z = 0.1f;

				//Makes the input pixel density independent
			startPos = Camera.main.ScreenToWorldPoint(startPos);
			endPos = Camera.main.ScreenToWorldPoint(endPos);

			//The duration of the swipe
			float duration = endTime - startTime;

			//The direction of the swipe
			Vector3 dir = endPos - startPos;

			//The distance of the swipe
			float distance = dir.magnitude;

			//Faster or longer swipes give higher power
			float power = distance / duration;

			//expected values are what power you get when you try 
			//desired values are what you want
			//you might want these as public values so they can be set from the inspector
			const float expectedMin = 50;
			const float expectedMax = 60;
			const float desiredMin = 15;
			const float desiredMax = 20;

			//Measure expected power here
			Debug.Log(power);

			//change power from the range 50...60 to 0...1
			power -= expectedMin;
			power /= expectedMax - expectedMin;

			//clamp value to between 0 and 1
			power = Mathf.Clamp01(power);

			//change power to the range 15...20
			power *= desiredMax - desiredMin;
			power += desiredMin;

			//take the direction from the swipe. length of the vector is the power
			Vector3 velocity = (transform.rotation * dir).normalized * power;
			this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-dir.x, -dir.y).normalized * power*45);
		}		
	}
}
