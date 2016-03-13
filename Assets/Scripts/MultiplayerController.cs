using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;

public class MultiplayerController : RealTimeMultiplayerListener {
	private static MultiplayerController _instance = null;
	private byte _protocolVersion = 1;
	// Byte + Byte + 2 floats for position + 2 floats for velcocity + 1 float for rotZ
	private int _updateMessageLength = 22;
	public MPUpdateListener updateListener;	
	private List<byte> _updateMessage;
	//Matchmaking options
	private uint minimumOpponents = 1;
	private uint maximumOpponents = 1;
	private uint gameVariation = 0;
	//end of
	public MPLobbyListener lobbyListener;

	private MultiplayerController() {
		_updateMessage = new List<byte>(_updateMessageLength);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate ();
	}

	public static MultiplayerController Instance {
		get {
			if (_instance == null) {
				_instance = new MultiplayerController();
			}
			return _instance;
		}
	}

	public void SignInAndStartMPGame() {
		if (! PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.localUser.Authenticate((bool success) => {
				if (success) {
					Debug.Log ("We're signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
					StartMatchMaking();
				} else {
					Debug.Log ("Oh... we're not signed in.");
				}
			});
		} else {
			Debug.Log ("You're already signed in.");
			StartMatchMaking();
		}
	}

	public void OnRoomSetupProgress (float percent)
	{
		ShowMPStatus ("We are " + percent + "% done with setup");
	}

	public void OnRoomConnected (bool success)
	{
		if (success) {
			ShowMPStatus ("We are connected to the room! I would probably start our game now.");
			Application.LoadLevel("MainGame");
		} else {
			ShowMPStatus ("Uh-oh. Encountered some error connecting to the room.");
		}
	}

	public void OnLeftRoom ()
	{
		ShowMPStatus ("We have left the room. We should probably perform some clean-up tasks.");
	}

	public void OnParticipantLeft (Participant participant)
	{
		throw new System.NotImplementedException ();
	}

	public void OnPeersConnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has joined.");
		}
	}

	public void OnPeersDisconnected (string[] participantIds)
	{
		foreach (string participantID in participantIds) {
			ShowMPStatus ("Player " + participantID + " has left.");
		}
	}

	public void OnRealTimeMessageReceived (bool isReliable, string senderId, byte[] data)
	{
		// We'll be doing more with this later...
		byte messageVersion = (byte)data[0];
		// Let's figure out what type of message this is.
		char messageType = (char)data[1];
		if (messageType == 'U' && data.Length == _updateMessageLength) { 
			float posX = System.BitConverter.ToSingle(data, 2);
			float posY = System.BitConverter.ToSingle(data, 6);
			float velX = System.BitConverter.ToSingle(data, 10);
			float velY = System.BitConverter.ToSingle(data, 14);
			float rotZ = System.BitConverter.ToSingle(data, 18);
			Debug.Log ("Player " + senderId + " is at (" + posX + ", " + posY + ") traveling (" + velX + ", " + velY + ") rotation " + rotZ);
			// We'd better tell our GameController about this.
			if (updateListener != null) {
				updateListener.UpdateReceived(senderId, posX, posY, velX, velY, rotZ);
			}
		}
		ShowMPStatus ("We have received some gameplay messages from participant ID:" + senderId);
	}

	public void ShowMPStatus(string message) {
		Debug.Log(message);
		if (lobbyListener != null) {
			lobbyListener.SetLobbyStatusMessage(message);
		}   
	}

	public List<Participant> GetAllPlayers() {
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants ();
	}

	public string GetMyParticipantId() {
		return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
	}

	public void TrySilentSignIn() {
		if (! PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.Authenticate ((bool success) => {
				if (success) {
					Debug.Log ("Silently signed in! Welcome " + PlayGamesPlatform.Instance.localUser.userName);
				} else {
					Debug.Log ("Oh... we're not signed in.");
				}
			}, true);
		} else {
			Debug.Log("We're already signed in");
		}
	}

	public void SignOut() {
		PlayGamesPlatform.Instance.SignOut ();
	}

	public bool IsAuthenticated() {
		return PlayGamesPlatform.Instance.localUser.authenticated;
	}

	private void StartMatchMaking() {
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame (minimumOpponents, maximumOpponents, gameVariation, this);
	}

	public void SendMyUpdate(float posX, float posY, Vector2 velocity, float rotZ) {
		_updateMessage.Clear ();
		_updateMessage.Add (_protocolVersion);
		_updateMessage.Add ((byte)'U');
		_updateMessage.AddRange (System.BitConverter.GetBytes (posX));  
		_updateMessage.AddRange (System.BitConverter.GetBytes (posY));  
		_updateMessage.AddRange (System.BitConverter.GetBytes (velocity.x));
		_updateMessage.AddRange (System.BitConverter.GetBytes (velocity.y));
		_updateMessage.AddRange (System.BitConverter.GetBytes (rotZ));
		byte[] messageToSend = _updateMessage.ToArray(); 
		Debug.Log ("Sending my update message  " + messageToSend + " to all players in the room");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, messageToSend);
	}

}
