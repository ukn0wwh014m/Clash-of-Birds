using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;

public class GameController : MonoBehaviour,MPUpdateListener{
	public GameObject myBird;
	public GameObject opponentPrefab;
	private float _timePlayed;
	private float _nextBroadcastTime = 0;
	private bool _multiplayerReady;
	private string _myParticipantId;
	 
	private Dictionary<string, OpponentBirdController> _opponentScripts;
	public bool _paused;
	private float _timeLeft;
	private int _lapsRemaining;
	 
	private Dictionary<string, float> _finishTimes;

	void Start()
	{
		SetupMultiplayerGame ();
	}

	public void UpdateReceived(string senderId, float posX, float posY, float velX, float velY, float rotZ) {
		if (_multiplayerReady) {
			OpponentBirdController opponent = _opponentScripts[senderId];
			if (opponent != null) {
				opponent.SetBirdInformation (posX, posY, velX, velY, rotZ);
			}
		}
	}

	void DoMultiplayerUpdate() {
		 
			MultiplayerController.Instance.SendMyUpdate (myBird.transform.position.x, 
				myBird.transform.position.y,
				myBird.GetComponent<Rigidbody2D> ().velocity, 
				myBird.transform.rotation.eulerAngles.z);
		 
	}

	void SetupMultiplayerGame()
	{
		MultiplayerController.Instance.updateListener = this;
		// 1
		_myParticipantId = MultiplayerController.Instance.GetMyParticipantId();
		// 2
		List<Participant> allPlayers = MultiplayerController.Instance.GetAllPlayers();
		Debug.Log ("AllPlayers"+ " " +MultiplayerController.Instance.GetAllPlayers ());
		_opponentScripts = new Dictionary<string, OpponentBirdController>(allPlayers.Count - 1); 
		_finishTimes = new Dictionary<string, float>(allPlayers.Count);
		for (int i =0; i < allPlayers.Count; i++) {
			string nextParticipantId = allPlayers[i].ParticipantId;
			 
			Debug.Log (nextParticipantId);
			Debug.Log("Setting up bird for " + nextParticipantId);
			if (nextParticipantId == _myParticipantId) {
				// 4
				myBird.GetComponent<BirdController> ().SetBirdChoice(i + 1, true);
			
			} else {
				// 5
				Debug.Log("Spawning Enemy");
				GameObject opponentBird = (Instantiate(opponentPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject);
				OpponentBirdController opponentScript = opponentBird.GetComponent<OpponentBirdController>();
				opponentScript.SetBirdNumber(i+1);
				// 6
				_opponentScripts[nextParticipantId] = opponentScript;
			}
		}
		// 7
		_multiplayerReady = true;
	}

	 

	void PauseGame() {
		_paused = true;
		myBird.GetComponent<BirdController>().SetPaused(true);
	}

	 

	void Update()
	{
		if (_paused) {
			return;
		}
		 
			// 1
			DoMultiplayerUpdate ();
			
			// 2
			MultiplayerController.Instance.SendMyUpdate(myBird.transform.position.x,
				myBird.transform.position.y,
				new Vector2(0,0),
				myBird.transform.rotation.eulerAngles.z);
			// 3
		 
			 
		 
	}
}
