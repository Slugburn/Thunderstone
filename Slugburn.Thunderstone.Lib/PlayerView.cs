using System;
using System.Collections.Generic;

namespace Slugburn.Thunderstone.Lib
{
    public class PlayerView : IPlayerView
    {
        public readonly Action<string, object> _onMessage;

        public PlayerView(Action<string, object> onMessage)
        {
            _onMessage = onMessage;
        }

        protected PlayerView()
        {
        }

        public void SendMessage(string messageId, object body=null)
        {
            _onMessage(messageId, body);
        }

        public void StartTurn()
        {
            SendMessage("StartTurn");
        }

        public void Log(string message)
        {
            SendMessage("Log", message);
        }

        public void BuyCard(IEnumerable<Deck> availableDecks, object message)
        {
            SendMessage("BuyCard", message);
        }

        public void HideUseAbility()
        {
            SendMessage("UseAbility", false);
        }

        public void SelectCards(object message)
        {
            SendMessage("SelectCards", message);
        }

        public void UpdateDeck(object message)
        {
            SendMessage("UpdateDeck", message);
        }

        public void UpdateDungeon(object message)
        {
            SendMessage("UpdateDungeon", message);
        }

        public void GameSetup(object message)
        {
            SendMessage("GameSetup", message);
        }

        public void GameBoard(object gameBoard)
        {
            SendMessage("GameBoard", gameBoard);
        }

        public void UpdateHand(object handMessage)
        {
            SendMessage("UpdateHand", handMessage);
        }

        public void UpdateStatus(object statusMessage)
        {
            SendMessage("UpdateStatus", statusMessage);
        }

        public void UseAbility(object message)
        {
            SendMessage("UseAbility", message);
        }
    }
}
