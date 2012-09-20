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
            var player = GetPlayer();
            StartGameHandler.Do(player);
        }

        private Player GetPlayer()
        {
            return PlayerStore.Instance.Get(Context.ConnectionId);
        }

        public void Village()
        {
            var player = GetPlayer();
            VillageHandler.Do(player);
        }

        public void Dungeon()
        {
            var player = GetPlayer();
            DungeonHandler.Do(player);
        }

        public void Prepare()
        {
            var player = GetPlayer();
            PrepareHandler.Do(player);
        }

        public void Rest()
        {
            var player = GetPlayer();
            RestHandler.Do(player);
        }

        public void UseAbility(UseAbilityResponse message)
        {
            var player = GetPlayer();
            UseAbilityHandler.Do(player, message);
        }

        public void BuyCard(string deckId)
        {
            var player = GetPlayer();
            BuyCardHandler.Do(player, long.Parse(deckId));
        }

        public void SelectCards(long[] cardIds)
        {
            var player = GetPlayer();
            SelectCardsHandler.Do(player, cardIds);
        }

        void IPlayerView.StartTurn(StartTurnMessage message)
        {
            Caller.displayStartTurn(message);
        }

        void IPlayerView.Log(string message)
        {
            Caller.displayLog(message);
        }

        void IPlayerView.BuyCard(BuyCardMessage message)
        {
            Caller.displayBuyCard(message);
        }

        void IPlayerView.UseAbility(UseAbilityMessage message)
        {
            Caller.displayUseAbility(message);
        }

        void IPlayerView.HideUseAbility()
        {
            Caller.displayUseAbility(false);
        }

        void IPlayerView.SelectCards(SelectCardsMessage message)
        {
            Caller.displaySelectCards(message);
        }

        void IPlayerView.UpdateDeck(DeckModel model)
        {
            Caller.displayDeck(model);
        }

        void IPlayerView.UpdateDungeon(DungeonModel model)
        {
            Caller.displayDungeon(model);
        }

        void IPlayerView.GameSetup(GameSetupModel model)
        {
            Caller.displayGameSetup(model);
        }

        void IPlayerView.GameBoard(GameBoardModel model)
        {
            Caller.displayGameBoard(model);
        }

        void IPlayerView.UpdateHand(UpdateHandMessage message)
        {
            Caller.displayHand(message);
        }

        void IPlayerView.UpdateStatus(StatusModel model)
        {
            Caller.displayStatus(model);
        }

    }
}