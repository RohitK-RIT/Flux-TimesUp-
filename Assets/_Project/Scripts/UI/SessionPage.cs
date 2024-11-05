using _Project.Scripts.Core.Backend;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class SessionPage : UIPage
    {
        [SerializeField] private TMP_Text lobbyCodeText;
        [SerializeField] private TMP_InputField lobbyCodeInputField;
        [SerializeField] private TMP_Text statusText;

        public async void OnCreateButtonClicked()
        {
            // Start the game in host mode
            var partyCode = await NetworkSystem.Instance.CreateSession();
            lobbyCodeText.text = partyCode;
        }

        public async void OnJoinButtonClicked()
        {
            if (string.IsNullOrEmpty(lobbyCodeInputField.text) || lobbyCodeInputField.text.Length <= 4)
            {
                statusText.text = "Invalid lobby code";
                statusText.color = Color.red;
                
                return;
            }
            // Start the game in client mode
            var status = await NetworkSystem.Instance.JoinSession(lobbyCodeInputField.text);
            if (status.Item1)
            {
                statusText.text = status.Item2;
                statusText.color = Color.green;
            }
            else
            {
                statusText.text = status.Item2;
                statusText.color = Color.red;
            }
        }
    }
}