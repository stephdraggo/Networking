using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattleCars.Networking;
using Mirror;

namespace BattleCars.UI
{
    public class ConnectionMenu : MonoBehaviour
    {
        [SerializeField]
        private BattleCarsNetworkManager networkManager;

        [SerializeField]
        private Button hostButton, connectButton;
        [SerializeField]
        private Text ipText;
        [SerializeField]
        private DiscoveredGame gameTemplate;
        [SerializeField]
        private Transform foundGamesHolder;

        private Dictionary<IPAddress, DiscoveredGame> discoveredGames = new Dictionary<IPAddress, DiscoveredGame>();

        void Start()
        {
            hostButton.onClick.AddListener(() => networkManager.StartHost());
            connectButton.onClick.AddListener(OnClickConnect);

            networkManager.discovery.onServerFound.AddListener(OnDetectServer);
            networkManager.discovery.StartDiscovery();
        }

        private void OnClickConnect()
        {
            networkManager.networkAddress = ipText.text;
            networkManager.StartClient();
        }


        private void OnDetectServer(DiscoveryResponse _response)
        {
            //we've seen the server ad on the network
            if (!discoveredGames.ContainsKey(_response.EndPoint.Address))
            { 
                //it's a new ad!
                DiscoveredGame game = Instantiate(gameTemplate, foundGamesHolder);
                game.gameObject.SetActive(true);
                //set it up
                game.Setup(_response, networkManager);
                discoveredGames.Add(_response.EndPoint.Address, game);
            }
        }
    }
}