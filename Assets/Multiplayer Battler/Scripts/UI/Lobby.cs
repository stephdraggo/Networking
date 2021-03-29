using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCars.Networking;

namespace BattleCars.UI
{

    public class Lobby : MonoBehaviour
    {
        private List<LobbyPlayerSlot> leftTeamSlots = new List<LobbyPlayerSlot>();
        private List<LobbyPlayerSlot> rightTeamSlots = new List<LobbyPlayerSlot>();

        [SerializeField]
        private GameObject leftTeamHolder, rightTeamHolder;

        //flipping bool to determine which team to assign each player to
        private bool assignToLeft = true;

        private BattleCarsPlayerNet localPlayer;

        public void OnPlayerConnected(BattleCarsPlayerNet _player)
        {
            bool assigned = false; //this be break in foreach

            if (_player.isLocalPlayer) localPlayer = _player;

            List<LobbyPlayerSlot> slots = assignToLeft ? leftTeamSlots : rightTeamSlots;

            slots.ForEach(slot =>
            {
                if (assigned)
                {
                    return;
                }
                else if (!slot.IsTaken)
                {
                    slot.AssignPlayer(_player);
                    slot.SetSide(assignToLeft);
                    assigned = true;
                }
            });

            for (int i = 0; i < leftTeamSlots.Count; i++)
            {
                LobbyPlayerSlot slot = leftTeamSlots[i];
                if (slot.IsTaken)
                    localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId);
            }
            for (int i = 0; i < rightTeamSlots.Count; i++)
            {
                LobbyPlayerSlot slot = rightTeamSlots[i];
                if (slot.IsTaken)
                    localPlayer.AssignPlayerToSlot(slot.IsLeft, i, slot.Player.playerId);
            }

            assignToLeft = !assignToLeft; //flip team
        }

        void Start()
        {
            //fill lists with player slots
            leftTeamSlots.AddRange(leftTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
            rightTeamSlots.AddRange(rightTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());


        }

        /// <summary>
        /// Assigns player to team and slot.
        /// </summary>
        public void AssignPlayerToSlot(BattleCarsPlayerNet _player, bool _left, int _slotId)
        {
            List<LobbyPlayerSlot> slots = _left ? leftTeamSlots : rightTeamSlots; //assign team
            slots[_slotId].AssignPlayer(_player); //assign slot
        }

        void Update()
        {

        }
    }
}