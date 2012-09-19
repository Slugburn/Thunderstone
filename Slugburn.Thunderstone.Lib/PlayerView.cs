using System;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;

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

        public void StartTurn(StartTurnMessage message)
        {
            SendMessage("StartTurn");
        }

        public void Log(string message)
        {
            SendMessage("Log", message);
        }

        public void BuyCard(BuyCardMessage message)
        {
            SendMessage("BuyCard", message);
        }

        public void HideUseAbility()
        {
            SendMessage("UseAbility", false);
        }

        public void SelectCards(SelectCardsMessage message)
        {
            SendMessage("SelectCards", message);
        }

        public void UpdateDeck(DeckModel model)
        {
            SendMessage("UpdateDeck", model);
        }

        public void UpdateDungeon(DungeonModel model)
        {
            SendMessage("UpdateDungeon", model);
        }

        public void GameSetup(GameSetupModel model)
        {
            SendMessage("GameSetup", model);
        }

        public void GameBoard(GameBoardModel model)
        {
            SendMessage("GameBoard", model);
        }

        public void UpdateHand(UpdateHandMessage message)
        {
            SendMessage("UpdateHand", message);
        }

        public void UpdateStatus(StatusModel model)
        {
            SendMessage("UpdateStatus", model);
        }

        public void UseAbility(UseAbilityMessage message)
        {
            SendMessage("UseAbility", message);
        }
    }
}
