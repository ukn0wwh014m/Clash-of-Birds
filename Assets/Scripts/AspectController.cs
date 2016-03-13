using UnityEngine;
using System.Collections;

public class AspectController : MonoBehaviour {

	public float targetRatio = 16f/10f; //The aspect ratio you did for the game.
	void Start()
	{
		Camera cam = GetComponent<Camera>();
		cam.aspect = targetRatio;
	} 
}
