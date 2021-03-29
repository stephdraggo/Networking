using UnityEngine;
using Mirror;
using System.Linq;
using System.Collections.Generic;

namespace BattleCars.Networking
{
    public class BattleCarsNetworkManager : NetworkManager
    {
        private Dictionary<byte, BattleCarsPlayerNet> players = new Dictionary<byte, BattleCarsPlayerNet>();

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

        public void AddPlayer(BattleCarsPlayerNet _player)
        {
            if (!players.ContainsKey(_player.playerId))
            {
                players.Add(_player.playerId, _player);
            }
        }


        /// <summary>
        /// Attempt to get player by ID.
        /// </summary>
        public BattleCarsPlayerNet GetPlayerForId(byte _playerId)
        {
            BattleCarsPlayerNet player;
            players.TryGetValue(_playerId, out player);
            return player;
        }

        /// <summary>
        /// Runs when a client connects to server.
        /// Responsible for creating player object and placing it in the scene.
        /// </summary>
        /// <param name="_connection"></param>
        public override void OnServerAddPlayer(NetworkConnection _connection)
        {
            Transform spawnPos = GetStartPosition(); //get next spawn position depending on spawnmode

            //spawn player at position if given, otherwise just spawn player
            GameObject playerObj = spawnPos != null ? Instantiate(playerPrefab, spawnPos.position, spawnPos.rotation) : Instantiate(playerPrefab);

            AssignPlayerId(playerObj); //assign ID
            NetworkServer.AddPlayerForConnection(_connection, playerObj); //add to server
        }

        /// <summary>
        /// Removes player (by ID) from dictionary.
        /// </summary>
        public void RemovePlayer(byte _id)
        {
            if (players.ContainsKey(_id))
            {
                players.Remove(_id);
            }
        }

        /// <summary>
        /// Finds next available ID and assigns it.
        /// </summary>
        /// <param name="_playerObj"></param>
        protected void AssignPlayerId(GameObject _playerObj)
        {
            byte id = 0;
            List<byte> playerIds = players.Keys.OrderBy(x => x).ToList(); //generate sorted list
            foreach (byte key in playerIds)
            {
                //if this id is not free, go to next
                if (id == key) id++;
            }

            //get component and assign
            BattleCarsPlayerNet player = _playerObj.GetComponent<BattleCarsPlayerNet>();
            player.playerId = id;

            players.Add(id, player);
        }
    }
}