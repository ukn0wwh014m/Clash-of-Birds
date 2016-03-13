using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour,MPLobbyListener {
	public Text debugText;

	 public void HideLobby ()
	{
		throw new System.NotImplementedException ();
	}

	// Use this for initialization
	void Start () {
		MultiplayerController.Instance.TrySilentSignIn();
	}
		
	public void LogOut()
	{
		MultiplayerController.Instance.SignOut();
	}

	public void SetLobbyStatusMessage(string message) {
		debugText.text = message;
	}

	public void StartMultiplayer()
	{
		MultiplayerController.Instance.SignInAndStartMPGame();
	}
	// Update is called once per frame
	void Update () {
		 
	}
}
