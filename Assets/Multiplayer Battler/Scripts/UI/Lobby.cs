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

        public void OnPlayerConnected(BattleCarsPlayerNet _player)
        {
            bool assigned = false; //this be break in foreach

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
                    assigned = true;
                }
            });

            assignToLeft = !assignToLeft; //flip team
        }

        void Start()
        {
            //fill lists with player slots
            leftTeamSlots.AddRange(leftTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());
            rightTeamSlots.AddRange(rightTeamHolder.GetComponentsInChildren<LobbyPlayerSlot>());


        }

        void Update()
        {

        }
    }
}