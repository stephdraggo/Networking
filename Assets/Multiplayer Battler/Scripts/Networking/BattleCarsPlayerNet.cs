using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using BattleCars.UI;
using BattleCars.Player;
using System.Collections;

namespace BattleCars.Networking
{
    [RequireComponent(typeof(PlayerMotor))]
    public class BattleCarsPlayerNet : NetworkBehaviour
    {
        [SyncVar]
        public byte playerId;
        [SyncVar]
        public string username = "";

        private Lobby lobby;
        private bool hasJoinedLobby = false;

        #region Commands
        [Command] //commands have to start with Cmd
        public void CmdSetUsername(string _name) => username = _name;
        [Command]
        public void CmdAssignPlayerToLobbySlot(bool _left, int _slotId, byte _playerId) => RpcAssignPlayerToLobbyList(_left, _slotId, _playerId);
        #endregion
        #region RPCs
        [ClientRpc]
        public void RpcAssignPlayerToLobbyList(bool _left, int _slotId, byte _playerId)
        {
            if (BattleCarsNetworkManager.Instance.IsHost) return;

            lobby = FindObjectOfType<Lobby>();
            StartCoroutine(AssignPlayerToLobbySlotDelayed(BattleCarsNetworkManager.Instance.GetPlayerForId(playerId), _left, _slotId));
        }
        #endregion
        #region Coroutines
        private IEnumerator AssignPlayerToLobbySlotDelayed(BattleCarsPlayerNet _player, bool _left, int _slotId)
        {
            while (lobby == null)
            {
                yield return null;
                lobby = FindObjectOfType<Lobby>();
            }

            lobby.AssignPlayerToSlot(_player, _left, _slotId);
        }
        #endregion

        private void Start()
        {
            if (isLocalPlayer)
            {
                PlayerMotor playerMotor = gameObject.GetComponent<PlayerMotor>();
                playerMotor.Setup();
                //can move in lobby eheh
            }
        }

        private void Update()
        {
            if (BattleCarsNetworkManager.Instance.IsHost)
            {
                //if no lobby yet, find lobby until found
                if (lobby == null && !hasJoinedLobby)
                {
                    lobby = FindObjectOfType<Lobby>();
                }

                //if lobby but not joined, join it
                if (lobby != null && !hasJoinedLobby)
                {
                    hasJoinedLobby = true;
                    lobby.OnPlayerConnected(this);
                }
            }
        }

        public override void OnStartLocalPlayer()
        {
            //load up the lobby scene
            SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
        }

        public override void OnStartClient()
        {
            BattleCarsNetworkManager.Instance.AddPlayer(this);
        }

        /// <summary>
        /// Sends rename command to the server if this is the local player.
        /// </summary>
        public void SetUsername(string _name)
        {
            if (isLocalPlayer) //you can only change your OWN name
            {
                CmdSetUsername(_name);
            }
        }

        /// <summary>
        /// Commanding assignment.
        /// </summary>
        public void AssignPlayerToSlot(bool _left, int _slotId, byte _playerId)
        {
            if (isLocalPlayer)
            {
                CmdAssignPlayerToLobbySlot(_left, _slotId, _playerId);
            }
        }

        /// <summary>
        /// Clean up player remains from the server.
        /// </summary>
        public override void OnStopClient()
        {
            BattleCarsNetworkManager.Instance.RemovePlayer(playerId);
        }
    }
}