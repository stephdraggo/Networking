using UnityEngine;
using Mirror;

namespace BattleCars.Networking
{
    public class BattleCarsNetworkManager : NetworkManager
    {
        /// <summary>
        /// reference to battle cars version of network manager
        /// </summary>
        public static BattleCarsNetworkManager Instance => singleton as BattleCarsNetworkManager;

        /// <summary>
        /// whether or not this network manager is the host
        /// </summary>
        public bool IsHost { get; private set; } = false;

        /// <summary>
        /// says this network is the host if it run the start hosting method
        /// </summary>
        public override void OnStartHost()
        {
            IsHost = true;
        }


    }
}