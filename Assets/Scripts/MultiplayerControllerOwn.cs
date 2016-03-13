using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using UnityEngine.UI;


public class MultiplayerControllerOwn : MonoBehaviour,RealTimeMultiplayerListener {
	public Text ez;
	public Text ez1;
	string lol;
	// Use this for initialization
	void Start () {
		PlayGamesPlatform.Activate ();
		Social.localUser.Authenticate((bool success) => {
			ez.text = Social.localUser.userName;
		});
	}

	public void HostGame()
	{
		const int MinOpponents = 1, MaxOpponents = 1;
		const int GameVariant = 0;
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents, GameVariant, this);
	}

	public void OnRoomSetupProgress(float progress) {
		// (процесс загрузки отображается от 0.0 до 100.0)
	}

	public void OnRoomConnected(bool success) {
		if (success) {
			// Выполняется при успешном соединении
			// …можете начинать игру…
			List<Participant> participants = PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
			Participant myself = PlayGamesPlatform.Instance.RealTime.GetSelf();
			lol = myself.ParticipantId;
			byte[] message = System.Text.Encoding.UTF8.GetBytes("Yeees!");; // сообщение в формате byte[]
			bool reliable = true;
			PlayGamesPlatform.Instance.RealTime.SendMessageToAll(reliable, message);
			Application.LoadLevel (1);
		} else {
			// Выполняется при неудачном соединении
			// …сообщение об ошибке…
		}
	}

	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data) {
		ez.text = senderId;
		ez1.text = lol;
	}

	public void OnLeftRoom() {
		// Возвращение в меню и показ сообщения об ошибке
		// не вызывайте здесь PlayGamesPlatform.Instance.RealTime.LeaveRoom()
		// вы уже вышли из комнаты
	}

	public void OnParticipantLeft(Participant participant){
	}

	public void OnPeersConnected(string[] participantIds) {
		// реакция на появление нового участника
	}

	public void OnPeersDisconnected(string[] participantIds) {
		// реакция на отсоединение участника
	}

	// Update is called once per frame
	void Update () {
	
	}
}
