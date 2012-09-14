using System.Collections.Generic;
using Slugburn.Thunderstone.Lib.Messages;

namespace Slugburn.Thunderstone.Lib
{
    public interface IPlayerView
    {
        void StartTurn();
        void Log(string message);
        void BuyCard(IEnumerable<Deck> availableDecks, object message);
        void HideUseAbility();
        void SelectCards(SelectCardsMessage message);
        void UpdateDeck(object message);
        void UpdateDungeon(object message);
        void GameSetup(object message);
        void GameBoard(object gameBoard);
        void UpdateHand(object handMessage);
        void UpdateStatus(object statusMessage);
        void UseAbility(object message);
    }
}