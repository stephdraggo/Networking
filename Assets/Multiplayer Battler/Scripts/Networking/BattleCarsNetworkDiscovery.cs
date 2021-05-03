using System;
using System.Net;
using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.Events;

/*
	Discovery Guide: https://mirror-networking.com/docs/Guides/NetworkDiscovery.html
    Documentation: https://mirror-networking.com/docs/Components/NetworkDiscovery.html
    API Reference: https://mirror-networking.com/docs/api/Mirror.Discovery.NetworkDiscovery.html
*/

namespace BattleCars.Networking
{
    [Serializable] public class ServerFoundEvent : UnityEvent<DiscoveryResponse> { }

    /// <summary>
    /// Data to send to client
    /// </summary>
    public class DiscoveryRequest : NetworkMessage
    {
        // Add properties for whatever information you want sent by clients
        // in their broadcast messages that servers will consume.

        public string gameName; //name of game being sent
    }

    /// <summary>
    /// Recieved by client and converted
    /// </summary>
    public class DiscoveryResponse : NetworkMessage
    {

        public IPEndPoint EndPoint { get; set; }

        public Uri uri; //URI is address, more specific URL

        public long serverId;

        public string gameName; //name of game being sent
    }

    public class BattleCarsNetworkDiscovery : NetworkDiscoveryBase<DiscoveryRequest, DiscoveryResponse>
    {
        #region Server

        public long ServerId { get; set; }

        [Tooltip("Transport (package holder) sending the server ad to the network.")]
        public Transport transport;

        public string gameNameYes;

        [Tooltip("This happens when a server is found.")]
        public ServerFoundEvent onServerFound = new ServerFoundEvent();

        public override void Start()
        {
            ServerId = RandomLong();

            gameNameYes = "Game Name Here";

            if (transport == null) transport = Transport.activeTransport;

            base.Start();
        }

        /// <summary>
        /// Process the request from a client
        /// </summary>
        /// <remarks>
        /// Override if you wish to provide more information to the clients
        /// such as the name of the host player
        /// </remarks>
        /// <param name="request">Request coming from client</param>
        /// <param name="endpoint">Address of the client that sent the request</param>
        /// <returns>A message containing information about this server</returns>
        protected override DiscoveryResponse ProcessRequest(DiscoveryRequest request, IPEndPoint endpoint)
        {
            try
            {
                //this example reply
                //add game name?
                //if specific game mode, it here
                return new DiscoveryResponse()
                {
                    serverId = ServerId,
                    uri = transport.ServerUri(),
                    gameName = gameNameYes,
                };
            }
            catch(NotImplementedException _e)
            {
                Debug.LogError($"Transport {name} does not support network discovery.",gameObject);
                throw; //a hissy fit
            }
        }

        #endregion

        #region Client

        /// <summary>
        /// Create a message that will be broadcasted on the network to discover servers
        /// </summary>
        /// <remarks>
        /// Override if you wish to include additional data in the discovery message
        /// such as desired game mode, language, difficulty, etc... </remarks>
        /// <returns>An instance of ServerRequest with data to be broadcasted</returns>
        protected override DiscoveryRequest GetRequest()=> new DiscoveryRequest();

        /// <summary>
        /// Process the answer from a server
        /// </summary>
        /// <remarks>
        /// A client receives a reply from a server, this method processes the
        /// reply and raises an event
        /// </remarks>
        /// <param name="response">Response that came from the server</param>
        /// <param name="endpoint">Address of the server that replied</param>
        protected override void ProcessResponse(DiscoveryResponse _response, IPEndPoint _endpoint)
        {
            //heckin what now?
            #region hecc
            _response.EndPoint = _endpoint; //received message remote endpoint (host)

            //turn url into uri bc otherwise might not resolve
            UriBuilder realUri = new UriBuilder(_response.uri)
            { Host = _response.EndPoint.Address.ToString() };

            _response.uri = realUri.Uri; //make the response real
            #endregion

            onServerFound.Invoke(_response);
        }

        #endregion
    }
}