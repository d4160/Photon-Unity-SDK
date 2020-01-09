namespace d4160.UI.Chat
{
    using d4160.Networking;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ChannelSelector : MonoBehaviour, IPointerClickHandler
    {
        public string Channel;

        public void SetChannel(string channel)
        {
            this.Channel = channel;
            Text t = GetComponentInChildren<Text>();
            t.text = this.Channel;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ChatControllerBase handler = FindObjectOfType<ChatControllerBase>();
            handler.ShowChannel(this.Channel);
        }
    }
}