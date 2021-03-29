using UnityEngine;
using UnityEngine.UI;
using BattleCars.Networking;

namespace BattleCars.UI
{
    public class LobbyPlayerSlot : MonoBehaviour
    {
        public bool IsTaken => player != null;
        public bool IsLeft { get; private set; }
        public BattleCarsPlayerNet Player => player;

        //[SerializeField]
        private Text nameDisplay;

        //[SerializeField]
        private Button playerButton;

        private BattleCarsPlayerNet player = null;

        void Start()
        {
            playerButton = gameObject.GetComponent<Button>();

            nameDisplay = playerButton.GetComponentInChildren<Text>();
        }

        //set player
        public void AssignPlayer(BattleCarsPlayerNet _player) => player = _player;



        public bool SetSide(bool _left) => IsLeft = _left;

        void Update()
        {
            //if slot is taken, button is interactable
            playerButton.interactable = IsTaken;

            //if player is set, display name, otherwise display "Awaiting Player..."
            nameDisplay.text = IsTaken ? GetPlayerName() : "Awaiting Player...";
        }

        private string GetPlayerName()
        {
            return string.IsNullOrEmpty(player.username) ? $"Player {player.playerId + 1}" : player.username;
        }
    }
}