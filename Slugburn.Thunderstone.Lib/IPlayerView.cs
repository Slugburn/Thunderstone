using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;

namespace Slugburn.Thunderstone.Lib
{
    public interface IPlayerView
    {
        void StartTurn(StartTurnMessage message);
        void Log(string message);
        void BuyCard(BuyCardMessage message);
        void HideUseAbility();
        void SelectCards(SelectCardsMessage message);
        void UpdateDeck(DeckModel model);
        void UpdateDungeon(DungeonModel model);
        void GameSetup(GameSetupModel model);
        void GameBoard(GameBoardModel model);
        void UpdateHand(UpdateHandMessage message);
        void UpdateStatus(StatusModel model);
        void UseAbility(UseAbilityMessage message);
    }
}