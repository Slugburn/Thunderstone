using SignalR.Hubs;
using Slugburn.Thunderstone.Lib;
using Slugburn.Thunderstone.Lib.MessageHandlers;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Web
{
    public class PlayerHub : Hub, IPlayerView
    {
        public void NewPlayer()
        {
            var session = new GameSession();
            var player = new Player(Context.ConnectionId, this);
            PlayerStore.Instance.Store(player);

            session.Join(player);
            GameSetupHandler.Do(player);
        }

        public void StartGame()
        {
            var player = PlayerStore.Instance.Get(Context.ConnectionId);
            StartGameHandler.Do(player);
        }

        public void StartTurn()
        {
            Caller.displayStartTurn();
        }

        public void Log(string message)
        {
            Caller.displayLog(message);
        }

        public void BuyCard(BuyCardMessage message)
        {
            Caller.displayBuyCard(message);
        }

        public void UseAbility(UseAbilityMessage message)
        {
            Caller.displayUseAbility(message);
        }

        public void HideUseAbility()
        {
            Caller.displayUseAbility(false);
        }

        public void SelectCards(SelectCardsMessage message)
        {
            Caller.displaySelectCards(message);
        }

        public void UpdateDeck(DeckModel model)
        {
            Caller.displayDeck(model);
        }

        public void UpdateDungeon(DungeonModel model)
        {
            Caller.displayDungeon(model);
        }

        public void GameSetup(GameSetupModel model)
        {
            Caller.displayGameSetup(model);
        }

        public void GameBoard(GameBoardModel model)
        {
            Caller.displayGameBoard(model);
        }

        public void UpdateHand(UpdateHandMessage message)
        {
            Caller.displayHand(message);
        }

        public void UpdateStatus(StatusModel model)
        {
            Caller.displayStatus(model);
        }

    }
}