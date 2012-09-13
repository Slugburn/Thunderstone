using System.Linq;

namespace Slugburn.Thunderstone.Lib.MessageHandlers
{
    public class StartGameHandler : MessageHandlerBase
    {
        public StartGameHandler() : base("StartGame")
        {
        }

        public override void Handle(Message message)
        {
            var game = new Game();
            game.Initialize(message.Player.Session);
            game.Players.Each(
                player=>player.SendMessage("GameBoard", CreateGameBoardMessage(player)));
            game.CurrentPlayer.StartTurn();
        }

        private static dynamic CreateGameBoardMessage(Player player)
        {
            var game = player.Game;
            var ranks = Enumerable.Range(1, 4).Select(x => new {Number = x, Card = (object) null, Penalty = -x});
            var heroDecks = CreateVillageSection(game,"Guilds and Taverns", CardType.Hero);
            var weaponDecks = CreateVillageSection(game, "Armory", CardType.Weapon);
            var itemDecks = CreateVillageSection(game, "Marketplace", CardType.Item);
            var spellDecks = CreateVillageSection(game, "Wizard's College", CardType.Spell);
            var villagerDecks = CreateVillageSection(game, "Town Square", CardType.Villager);
            var hand = player.Hand;
            var status = player.CreateStatusMessage();
            return new
            {
                Dungeon = new { Ranks = ranks, DeckCount = 31 },
                HeroDecks = heroDecks,
                WeaponDecks = weaponDecks,
                ItemDecks = itemDecks,
                SpellDecks = spellDecks,
                VillagerDecks = villagerDecks,
                Hand = hand.CreateMessage(),
                Status = status,
            };
        }

        private static dynamic CreateVillageSection(Game game, string sectionName, CardType type)
        {
            return new
            {
                SectionName = sectionName,
                Decks = game.Village[type].Select(x => x.CreateMessage())
            };
        }
    }
}
