using SignalR.Hubs;
using Slugburn.Thunderstone.Lib;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Web
{
    public class PlayerHub : Hub, IPlayerView
    {
        private Player GetPlayer()
        {
            return PlayerStore.Instance.Get(Context.ConnectionId);
        }

        public void NewPlayer()
        {
            var session = new GameSession();
            var player = new Player(Context.ConnectionId, this);
            PlayerStore.Instance.Store(player);

            session.Join(player);
            var setup = new GameSetup();
            player.Session.Setup = setup;
            player.Session.SendAll(x => x.View.GameSetup(GameSetupModel.From(setup)));
        }

        public void StartGame()
        {
            var player = GetPlayer();
            var game = new Game();
            game.Initialize(player.Session);
            game.Players.Each(p => p.View.GameBoard(GameBoardModel.From(p)));
            game.CurrentPlayer.StartTurn();
        }

        public void Village()
        {
            var player = GetPlayer();
            player.State = PlayerState.Village;
            player.UseAbilities();
        }

        public void Dungeon()
        {
            var player = GetPlayer();
            player.State = PlayerState.Dungeon;
            player.UseAbilities();
        }

        public void Prepare()
        {
            var player = GetPlayer();
            player.State = PlayerState.Prepare;
            player.Prepare();
        }

        public void Rest()
        {
            var player = GetPlayer();
            player.State = PlayerState.Rest;
            player.UseAbilities();
        }

        public void UseAbility(UseAbilityResponse message)
        {
            var player = GetPlayer();
            if (message.AbilityId == null)
            {
                player.State.ContinueWith(player);
                return;
            }
            player.UseAbility(message.AbilityId.Value);
        }

        public void BuyCard(string deckId)
        {
            var player = GetPlayer();
            var card = player.Game.BuyCard(long.Parse(deckId));
            player.AddToDiscard(card);
            player.LevelHeroes();
        }

        public void SelectCards(long[] cardIds)
        {
            var player = GetPlayer();
            player.SelectCardsCallback(cardIds);
        }

        public void SelectOption(string option)
        {
            var player = GetPlayer();
            player.SelectOptionCallback(option);
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

        void IPlayerView.SelectOption(SelectOptionMessage message)
        {
            Caller.displaySelectOption(message);
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