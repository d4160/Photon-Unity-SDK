namespace d4160.UI.Chat
{
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
    using Networking;

    public class PhotonChatUI : ChatUIBase
	{
		[Header("UI OBJECTS")]
		public RectTransform ChatPanel; // set in inspector (to enable/disable panel)
		[SerializeField] protected InputField m_inputFieldChat;
		[SerializeField] protected Text m_currentChannelText;
		public Text StateText; // set in inspector
		public Text UserIdText; // set in inspector

		[Header("PREFABS")]
		[SerializeField] protected GameObject FriendListUiItemtoInstantiate;
		public Toggle ChannelToggleToInstantiate; // set in inspector

		protected readonly Dictionary<string, Toggle> m_channelToggles = new Dictionary<string, Toggle>();
		protected readonly Dictionary<string, FriendItem> m_friendListItemLUT =  new Dictionary<string, FriendItem>();

		protected override void Awake()
		{
			base.Awake();

			if (ChannelToggleToInstantiate)
				ChannelToggleToInstantiate.gameObject.SetActive(false);

			if (FriendListUiItemtoInstantiate != null)
			{
				FriendListUiItemtoInstantiate.SetActive(false);
			}
		}

		public override void OnEnterSend()
		{
			if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
			{
				NetworkingManagerBase.Instance.Chat.SendChatMessage(m_inputFieldChat.text);
				m_inputFieldChat.text = "";
			}
		}

		public override void OnClickSend()
		{
			if (m_inputFieldChat != null)
			{
				NetworkingManagerBase.Instance.Chat.SendChatMessage(m_inputFieldChat.text);
				m_inputFieldChat.text = "";
			}
		}

		public override void OnConnected(string[] friends = null)
		{
			var chat = NetworkingManagerBase.Instance.Chat;

			if (UserIdText)
				this.UserIdText.text = $"Connected as {chat.ChatProvider.Username}";

			//if (ChatPanel)
			//	this.ChatPanel.gameObject.SetActive(true);
            DestroyFriendItems();

			if (friends!=null  && friends.Length>0)
			{
				chat.ChatProvider.AddFriends(friends);

				// add to the UI as well
				foreach(string _friend in friends)
				{
					if (this.FriendListUiItemtoInstantiate != null && _friend != chat.ChatProvider.Username)
					{
						this.InstantiateFriendButton(_friend);
					}
				}
			}

            DestroyChannelItems();
        }

        private void DestroyFriendItems()
        {
            foreach (var friendItem in m_friendListItemLUT)
            {
                Destroy(friendItem.Value.gameObject);
            }

            m_friendListItemLUT.Clear();
        }

        private void DestroyChannelItems()
        {
            foreach (var channel in m_channelToggles)
            {
                Destroy(channel.Value.gameObject);
            }

            m_channelToggles.Clear();
        }

		protected virtual void InstantiateFriendButton(string friendId)
		{
			GameObject fbtn = Instantiate(FriendListUiItemtoInstantiate);
			fbtn.gameObject.SetActive(true);
			FriendItem _friendItem =	fbtn.GetComponent<FriendItem>();

			_friendItem.FriendId = friendId;

			fbtn.transform.SetParent(FriendListUiItemtoInstantiate.transform.parent, false);

			m_friendListItemLUT[friendId] = _friendItem;
		}

		public override void OnDisconnected()
		{
		}

		public override void OnChatStateChange(string state)
		{
			// use OnConnected() and OnDisconnected()
			// this method might become more useful in the future, when more complex states are being used.
			if (StateText)
				this.StateText.text = state;
		}

		public override void OnSubscribed(string[] channels)
		{
			foreach (string channelName in channels)
			{
				if (this.ChannelToggleToInstantiate != null)
				{
						this.InstantiateChannelButton(channelName);
				}
			}
		}

		protected virtual void InstantiateChannelButton(string channelName)
		{
			if (m_channelToggles.ContainsKey(channelName))
			{
				Debug.Log("Skipping creation for an existing channel toggle.");
				return;
			}

			Toggle cbtn = (Toggle)Instantiate(this.ChannelToggleToInstantiate);
			cbtn.gameObject.SetActive(true);
			cbtn.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
			cbtn.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);

			m_channelToggles.Add(channelName, cbtn);
		}

		public override void OnUnsubscribed(string[] channels)
		{
			foreach (string channelName in channels)
			{
				if (m_channelToggles.ContainsKey(channelName))
				{
					Toggle t = m_channelToggles[channelName];
					Destroy(t.gameObject);

					m_channelToggles.Remove(channelName);

					Debug.Log("Unsubscribed from channel '" + channelName + "'.");

					var chat = NetworkingManagerBase.Instance.Chat;

					// Showing another channel if the active channel is the one we unsubscribed from before
					if (channelName == chat.SelectedChannelName && m_channelToggles.Count > 0)
					{
						IEnumerator<KeyValuePair<string, Toggle>> firstEntry = m_channelToggles.GetEnumerator();
						firstEntry.MoveNext();

						chat.ShowChannel(firstEntry.Current.Key);

						firstEntry.Current.Value.isOn = true;
					}
				}
				else
				{
					Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
				}
			}
		}

		public override void OnPrivateMessage(string sender, object message, string channelName)
		{
			// as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
			// you also get messages that you sent yourself. in that case, the channelName is determinded by the target of your msg
			InstantiateChannelButton(channelName);
		}

		public override void OnStatusUpdate(string user, int status, bool gotMessage, object message)
		{
			Debug.LogWarning("Status: " + string.Format("{0} is {1}. Msg:{2}", user, status, message));

			if (m_friendListItemLUT.ContainsKey(user))
			{
				FriendItem _friendItem = m_friendListItemLUT[user];
				if ( _friendItem!=null) _friendItem.OnFriendStatusUpdate(status,gotMessage,message);
			}
		}

		public override void OnUserSubscribed(string channel, string user)
		{
			Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
		}

		public override void OnUserUnsubscribed(string channel, string user)
		{
			Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
		}

		public override void ShowChannel(string channel, string messages)
		{
			m_currentChannelText.text = messages;

			foreach (KeyValuePair<string, Toggle> pair in m_channelToggles)
			{
				pair.Value.isOn = pair.Key == channel ? true : false;
			}
		}

		public override void ClearMessagesOfCurrentChannel()
		{
			m_currentChannelText.text = string.Empty;
		}

		public override void AppendToCurrentChannel(string text)
		{
			m_currentChannelText.text += text;
		}

		public override void SubscribeOrShowChannel(string channel)
		{
			var chat = NetworkingManagerBase.Instance.Chat;

			// If we are already subscribed to the channel we directly switch to it, otherwise we subscribe to it first and then switch to it implicitly
			if (m_channelToggles.ContainsKey(channel))
			{
				chat.ShowChannel(channel);
			}
			else
			{
				chat.ChatProvider.Subscribe(new string[] { channel });
			}
		}
	}
}