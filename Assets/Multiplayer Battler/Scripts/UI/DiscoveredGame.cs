using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleCars.Networking;

namespace BattleCars.UI
{
    [RequireComponent(typeof(Button))]
    public class DiscoveredGame : MonoBehaviour
    {
        

        [SerializeField]
        private Text textDisplay;

        private string ip;

        private BattleCarsNetworkManager networkManager;

        /// <summary>
        /// Set up the heccin join game button
        /// </summary>
        public void Setup(DiscoveryResponse _response,BattleCarsNetworkManager _manager)
        {
            ip = _response.EndPoint.Address.ToString();
            textDisplay.text = _response.gameName.ToString()+"\n"+ip;
            networkManager = _manager;

            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(JoinGame);
        }

        /// <summary>
        /// When click on button, connect to displayed server
        /// </summary>
        private void JoinGame()
        {
            networkManager.networkAddress = ip;
            networkManager.StartClient();
        }
    }
}