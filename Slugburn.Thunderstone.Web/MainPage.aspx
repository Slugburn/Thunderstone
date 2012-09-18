<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Thunderstone</title>
    <link rel="stylesheet" type="text/css" href="Scripts/jquery.qtip.min.css" />
    <script type="text/javascript" src="Scripts/jquery-1.6.4.js"> </script>
    <script type="text/javascript" src="Scripts/knockout-2.1.0.js"> </script>
    <script type="text/javascript" src="Scripts/jquery.qtip.js"> </script>
    <script type="text/javascript" src="Scripts/jquery.signalR-0.5.3.js"></script>
    <script src="signalr/hubs" type="text/javascript"></script>
    <style>
        a > .minicard
        {
            cursor: pointer;
            box-shadow: 3px 3px 0px #777;
        }

        a.down > .minicard
        {
            box-shadow: 0px 0px 0px;
            background: burlywood;
            outline-offset: 2px;
        }

        .table
        {
            display: table;
        }

        .row
        {
            display: table-row;
        }

        .cell
        {
            display: table-cell;
        }

        .iblock
        {
            display: inline-block;
            position: relative;
        }

        .block
        {
            display: block;
        }


        .main
        {
            border: solid;
            border-radius: 10px;
            padding: 5px;
            display: table;
            margin: auto;
            position: relative;
            background: white;
            box-shadow: 5px 5px 5px darkslategrey;
        }

        .name
        {
            font-size: 10px;
            font-weight: bold;
            font-family: sans-serif;
        }

        .description
        {
            font-size: 12px;
            font-family: serif;
        }

        .minicard
        {
            border-radius: 10px;
            border-style: solid;
            border-width: 2px;
            height: 60px;
            width: 100px;
            padding: 3px;
            position: relative;
            display: block;
            background: blanchedalmond;
        }

            .minicard.blank
            {
                background: -webkit-linear-gradient(darkgray, white);
                background: linear-gradient(darkgray, white);
            }

            .minicard > .name
            {
                position: absolute;
                width: 104px;
                top: -1px;
                left: -1px;
                display: block;
                text-align: left;
                background-color: darkgreen;
                color: white;
                padding: 2px;
                border-radius: 10px 10px 0px 0px;
            }

            .minicard > .equipped
            {
                position: absolute;
                width: 90px;
                bottom: 2px;
                display: block;
                text-align: center;
                background: transparent;
                padding: 4px;
                font: 8px sans-serif;
                font-style: italic;
                font-weight: bold;
            }

            .minicard > .topRight
            {
                right: 3px;
                top: 1px;
            }

            .minicard > .bottomRight
            {
                right: 3px;
                top: 47px;
            }


            .minicard > .middleMiddle
            {
                left: 44px;
                top: 25px;
            }

            .minicard > .bottomLeft
            {
                left: 2px;
                top: 47px;
            }

        .setupLabel
        {
            display: table-cell;
            float: left;
            font-weight: bold;
            margin: 5px;
            padding: 3px 10px 3px 10px;
            text-align: right;
            vertical-align: central;
        }

        .ui-tooltip-card
        {
            background-color: lightgray;
            border-color: black;
            border-radius: 10px;
            border-width: 2px;
            color: black;
        }

        .coin
        {
            background-color: white;
            border: solid black 1px;
            border-radius: 16px;
            color: black;
            font-family: sans-serif;
            font-size: 12px;
            font-weight: bold;
            height: 14px;
            padding-top: 2px;
            position: absolute;
            text-align: center;
            width: 16px;
            box-shadow: 2px 2px 2px darkslategrey;
        }


            .coin.count
            {
                color: black;
                background: white;
                border-radius: 5px;
            }

            .coin.black
            {
                color: white;
                background: -webkit-radial-gradient(center, ellipse cover, gray 0%, black 70%);
                background: radial-gradient(ellipse at center, gray 0%, black 70%);
            }

            .coin.gold
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, gold 70%);
                background: radial-gradient(ellipse at center, white 0%, gold 70%);
            }

            .coin.orange
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, orange 70%);
                background: radial-gradient(ellipse at center, white 0%, orange 70%);
            }

            .coin.brown
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, tan 70%);
                background: radial-gradient(ellipse at center, white 0%, tan 70%);
            }

            .coin.light
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, lightyellow 70%);
                background: radial-gradient(ellipse at center, white 0%, lightyellow 70%);
            }

            .coin.red
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, red 70%);
                background: radial-gradient(ellipse at center, white 0%, red 70%);
            }

            .coin.blue
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, deepskyblue 70%);
                background: radial-gradient(ellipse at center, white 0%, deepskyblue 70%);
            }

            .coin.xp
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, silver 70%);
                background: radial-gradient(ellipse at center, white 0%, silver 70%);
            }

            .coin.vp
            {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, darkturquoise 70%);
                background: radial-gradient(ellipse at center, white 0%, darkturquoise 70%);
            }

        .center
        {
            position: static;
            display: table;
            margin: 0px auto;
        }

        .cardPopup
        {
        }

        .sectionTitle
        {
            font-family: sans-serif;
            font-size: 12px;
            background-color: black;
            color: white;
            margin: 2px;
            padding: 2px 4px;
        }

        .smallButton
        {
            font-size: 10px;
        }
    </style>
</head>
<body>
    <div id="gameSetup" class="main">
        <table data-bind="template: { name: 'setup-list-template', foreach: sets }"></table>
        <button id="startGame" style="display: inherit; margin: 5px auto; width: 100px;">Start Game</button>
    </div>

    <div id="gameBoard" class="main">
        <div id="dungeon" data-bind="with: Dungeon" class="block">
            <span class="block sectionTitle">The Dungeon Hall</span>
            <div class="block">
                <!-- ko foreach: Ranks -->
                <div class="iblock">
                    <div class="block sectionTitle">Rank <span data-bind="text: Number"></span></div>
                    <div data-bind="template: {name: 'minicard-template', data: $.extend(Card, {empty: Card===null}) }"></div>
                </div>
                <!-- /ko -->
                <div class="iblock">
                    <span class="block sectionTitle">Deck</span>
                    <div class="minicard" style="position: relative;">
                        <span data-bind="text: DeckCount" class="coin count topRight"></span>
                    </div>
                </div>
            </div>
        </div>
        <div data-bind="template: {name: 'deck-section-template', data: HeroDecks }" class="block"></div>
        <div data-bind="template: {name: 'deck-section-template', data: WeaponDecks }" class="block"></div>
        <div data-bind="template: {name: 'deck-section-template', data: ItemDecks }" class="block"></div>
        <div style="display: table; width: 100%">
            <div class="iblock" style="width: 50%">
                <div data-bind="template: {name: 'deck-section-template', data: SpellDecks }" class="block"></div>
            </div>
            <div class="iblock" style="width: 50%">
                <div data-bind="template: {name: 'deck-section-template', data: VillagerDecks }" class="block"></div>
            </div>
        </div>
        <div id="hand" class="block">
            <span data-bind="with: Status" class="block sectionTitle" style="width: 664px">
                <span class="cell">Hand</span>
                <span class="cell" style="width: 30px;">
                    <span data-bind="text: Gold" class="center coin gold"></span>
                </span>
                <span class="cell">/</span>

                <span class="cell" style="width: 30px;">
                    <span data-bind="text: PhysicalAttack" class="center coin red"></span>
                </span>
                <span class="cell" style="width: 30px;">
                    <span data-bind="text: MagicAttack" class="center coin blue"></span>
                </span>
                <span class="cell" style="width: 30px;">
                    <span data-bind="text: Light" class="center coin light"></span>
                </span>
            </span>
            <!-- ko foreach: HandParts -->
            <div data-bind="foreach: $data" class="block">
                <div class="iblock">
                    <div data-bind="template: {name: 'minicard-template', data: $data }"></div>
                </div>
            </div>
            <!-- /ko -->
        </div>
        <div style="position: absolute; top: 222px; left: 460px; height: 190px; width: 222px; border-radius: 12px; background-color: white; box-sizing: border-box; padding: 3px;">
            <div style="position: relative; border: black solid 2px; border-radius: 10px; height: 100%; box-sizing: inherit; padding: 4px;">
                <div data-bind="with: Status" class="table" style="margin: 0px auto">
                    <div class="row" style="margin: 0px auto">
                        <span class="cell sectionTitle">Gold</span>
                        <div class="cell" style="width: 30px;">
                            <span data-bind="text: Gold" class="center coin gold"></span>
                        </div>
                        <span class="cell sectionTitle">Xp</span>
                        <div class="cell" style="width: 30px;">
                            <span data-bind="text: Xp" class="center coin xp" style=""></span>
                        </div>
                        <span class="sectionTitle cell">Vp</span>
                        <div class="cell" style="width: 30px;">
                            <span data-bind="text: Vp" class="center coin vp" style=""></span>
                        </div>
                    </div>
                </div>
                <hr />
                <div id="playerPanel" data-bind="with: playerPanel">
                    <span data-bind="text: playerPanel.text"></span>
                    <button data-bind="click: village, visible: commandsVisible" class="center" style="width: 100px;">Village</button>
                    <button data-bind="click: dungeon, visible: commandsVisible" class="center" style="width: 100px;">Dungeon</button>
                    <button data-bind="click: prepare, visible: commandsVisible" class="center" style="width: 100px;">Prepare</button>
                    <button data-bind="click: rest, visible: commandsVisible" class="center" style="width: 100px;">Rest</button>
                </div>
            </div>
        </div>
    </div>

    <div id="useAbility" data-bind="style: {display: Visible() ? 'table' : 'none' }">
        <div class="main">
            <div data-bind="text: Phase" class="block center sectionTitle" style="width: 100%"></div>
            <div class="table">
                <div class="row">
                    <span class="cell sectionTitle" style="width: 100px;">Card</span>
                    <span class="cell sectionTitle">Use</span>
                    <span class="cell sectionTitle" style="width: 250px;">Ability</span>
                </div>
                <!-- ko foreach: Abilities -->
                <div class="row">
                    <span data-bind="text: CardName" class="cell name"></span>
                    <button data-bind="click: $parent.use" class="cell smallButton">Use</button>
                    <span data-bind="text: Description" class="cell description"></span>
                </div>
                <!-- /ko -->
            </div>
            <button data-bind="click: Done, visible: ShowDone" class="center" style="margin-top: 5px;">Done</button>
        </div>
    </div>

    <div id="selectCards" data-bind="style: {display: Visible() ? 'table' : 'none' }">
        <div class="main">
            <div data-bind="text: Caption" class="block center sectionTitle" style="width: 100%"></div>
            <div data-bind="text: Message" class="block"></div>
            <div class="table" style="margin-bottom: 5px;">

                <!-- ko foreach: Cards -->
                <div data-bind="foreach: $data" class="row">
                    <div class="cell" style="padding: 4px; position: relative">
                        <a data-bind="template: {name: 'minicard-template', data: $data }"></a>
                    </div>
                </div>
                <!-- /ko -->

            </div>
            <button data-bind="enable: Enabled, click: Done" class="center" style="margin-top: 5px;">Done</button>
        </div>
    </div>

    <div id="log" style="position: absolute; top: 0px; left: 0px; height: 800px; width: 200px; overflow: auto; font-size: 10px; font-family: sans-serif"></div>

    <script type="text/html" id="deck-section-template">
        <span data-bind="text: SectionName" class="block sectionTitle"></span>
        <div data-bind="foreach: Decks" class="block">
            <div data-bind="attr: {id: 'deck_' + Id}, template: {name: 'minicard-template', data: $.extend(TopCard, {empty: (Count === 0)}) }" class="iblock"></div>
        </div>
    </script>

    <script type="text/html" id="setup-list-template">
        <tr>
            <td class="setupLabel" data-bind='text: name'></td>
            <!-- ko foreach: set -->
            <td data-bind="template: {name: 'minicard-template', data: $data }"></td>
            <!-- /ko -->
        </tr>
    </script>

    <script type="text/html" id="minicard-template">
        <!-- ko if: !$data.empty && $data -->
        <span class="minicard">
            <span data-bind="text: Name, visible: Name" class="name"></span>
            <!-- ko if: $data.Owner === 'Player' -->
            <span data-bind="text: Gold, visible:Gold || Gold===0" class="coin gold bottomRight"></span>
            <span data-bind="text: Equipped, visible: Equipped" class="equipped"></span>
            <span class="block middleMiddle" style="position: absolute; padding: 0px; margin: 0px;">
                <span data-bind="text: PhysicalAttack, visible:PhysicalAttack" class="coin red cell" style="position: static;"></span>
                <span data-bind="text: PotentialPhysicalAttack, visible:PotentialPhysicalAttack || PotentialPhysicalAttack===0" class="coin cell" style="background-color: rgba(255, 0, 0, 0.40); position: static;"></span>
                <span data-bind="text: MagicAttack, visible:MagicAttack" class="coin blue cell" style="position: static;"></span>
                <span data-bind="text: PotentialMagicAttack, visible:PotentialMagicAttack || PotentialMagicAttack===0" class="coin cell" style="background: rgba(0,191,255,0.40); position: static;"></span>
            </span>
            <span data-bind="text: Light, visible:Light" class="coin light bottomLeft"></span>
            <!-- /ko -->
            <!-- ko if: $data.Owner === 'Village' -->
            <span data-bind="text: $parent.Count, visible:$parent.Count" class="coin count topRight"></span>
            <span data-bind="text: Cost, visible:Cost|| Cost===0" class="coin orange bottomRight"></span>
            <!-- /ko -->
            <!-- ko if: $data.Owner === 'Dungeon' -->
            <span data-bind="text: Health, visible:Health" class="coin red middleMiddle"></span>
            <span class="block bottomLeft" style="position: absolute; padding: 0px; margin: 0px;">
                <span data-bind="text: Darkness, visible:Darkness" class="coin black cell" style="position: static;"></span>
            </span>
            <span class="block bottomRight" style="position: absolute; padding: 0px; margin: 0px;">
                <span data-bind="text: Vp, visible:Vp" class="coin vp cell" style="position: static;"></span>
            </span>
            <!-- /ko -->
            <span style="display: none">
                <span class="cardPopup" data-bind="template: {name: 'card-template', data: $data }"></span>
            </span>
        </span>
        <!-- /ko -->
        <!-- ko ifnot: !$data.empty && $data -->
        <span class="minicard blank"></span>
        <!-- /ko -->
    </script>

    <script type="text/html" id="card-template">
        <div style="height: 280px; width: 200px;">
            <span style="background-color: azure; color: black; height: 42%; left: 15px; position: absolute; top: 13%; width: 100%;"></span>
            <span data-bind="text: Name" style="display: block; font-size: 14px; font-weight: bold; text-align: center;"></span>
            <span data-bind="html: Text" style="background-color: white; color: black; height: 30%; left: 0px; padding: 10px; position: absolute; top: 60%; width: 200px;"></span>
            <span data-bind="text: Tags" style="display: block; font-size: 10px; text-align: center; padding-top: 2px;"></span>
            <span data-bind="text: Health, visible:Health" class="coin red" style="left: 195px; top: 5px;"></span>
            <span data-bind="text: Gold, visible:Gold || Gold===0" class="coin gold" style="left: 5px; top: 50px;"></span>
            <span data-bind="text: Strength, visible: Strength" class="coin brown" style="left: 5px; top: 80px;"></span>
            <span data-bind="text: Light, visible:Light" class="coin light" style="left: 5px; top: 50%;"></span>
            <span data-bind="text: Cost, visible:Cost || Cost===0" class="coin orange" style="left: 100px; top: 54%;"></span>
            <span data-bind="text: Xp, visible: Xp" class="coin xp" style="left: 5px; top: 92%;"></span>
            <span data-bind="text: Vp, visible: Vp" class="coin vp" style="left: 195px; top: 92%;"></span>
        </div>
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('#gameSetup').hide();
            $('#gameBoard').hide();

            var gameBoard;

            var hub = $.connection.playerHub;

            var useAbility = new UseAbilityVm(hub);
            ko.applyBindings(useAbility, $('#useAbility').get(0));

            var selectCards = new SelectCardsVm(hub);
            ko.applyBindings(selectCards, $('#selectCards').get(0));

            gameBoard = new GameBoardVm(hub);
            ko.applyBindings(gameBoard, $('#gameBoard').get(0));

            hub.displayGameSetup = function (message) {
                var vm = new GameSetupVm(message);
                ko.applyBindings(vm, $('#gameSetup').get(0));
                createTooltips();
                $('#gameSetup').show();
            };

            hub.displayGameBoard = function (message) {
                gameBoard.update(message);
                $('#gameBoard').show();
                createTooltips();
            };

            hub.displayStartTurn = function () {
                displayMenu();
            };

            hub.displayBuyCard = function (message) {
                gameBoard.playerPanel.text('Buy a card.');
                message.AvailableDecks.forEach(function (deckId) {
                    var button = $('<button class="center smallButton buyButton" style="position:absolute; top:48px; left: 39px;">Buy</button>');
                    $('#deck_' + deckId).append(button);
                    button
                        .click(function () {
                            hub.buyCard(deckId);
                            $(".buyButton").remove();
                        });
                });
            };

            hub.displayDungeon = function (message) {
                gameBoard.Dungeon(message);
                createTooltips('#dungeon');
            };

            hub.displayDeck = function (message) {
                gameBoard.Decks[message.Id](message);
                createTooltips('#deck_' + message.Id);
            };

            hub.displayHand = function (message) {
                var hand = message.Hand;
                var handParts = [];
                while (hand.length > 0) {
                    handParts.push(hand.slice(0, 6));
                    hand = hand.slice(6);
                }
                gameBoard.HandParts(handParts);
                createTooltips('#hand');
            };

            hub.displayStatus = function (message) {
                gameBoard.Status(message);
            };

            hub.displayUseAbility = function (message) {
                if (message) {
                    useAbility.update(message);
                } else {
                    useAbility.Visible(false);
                }
            };

            hub.displaySelectCards = function (message) {
                selectCards.update(message);
                createTooltips('#selectCards');
            };

            hub.displayLog = function (message) {
                $('#log').append(message + '<br/>');
            };

            $.connection.hub.error(function (message) {
                alert(message);
            });

            $.connection.hub.start().done(function () {
                hub.newPlayer();
            });

            $('#startGame').click(function () {
                ko.applyBindings({ sets: [] }, $('#gameSetup').get(0)); // unbind the game setup
                $('#gameSetup').hide();
                hub.startGame();
            });

            function displayMenu() {
                gameBoard.playerPanel.text('');
                gameBoard.playerPanel.commandsVisible(true);
            }

            function createTooltips(context) {
                $('.minicard', context).each(function () {
                    $(this).qtip({
                        content: $('.cardPopup', this),
                        position: { viewport: $(window) },
                        style: {
                            classes: 'ui-tooltip-shadow ui-tooltip-card'
                        },
                        hide: {
                            fixed: true,
                            delay: 100
                        }
                    });
                });
            }
        });

        function Hash() {
        }

        Hash.prototype.size = function () {
            var r = 0, key;
            for (key in this) {
                if (this.hasOwnProperty(key)) r++;
            }
            return r;
        };

        Hash.prototype.keys = function () {
            var r = [], key;
            for (key in this) {
                if (this.hasOwnProperty(key)) r.push(key);
            }
            return r;
        };

        function GameSetupVm(gameSetup) {
            this.sets = [
                { name: 'Monsters', set: [gameSetup.ThunderstoneBearer].concat(gameSetup.Monsters) },
                { name: 'Heroes', set: gameSetup.Heroes },
                { name: 'Weapons', set: gameSetup.Weapons },
                { name: 'Items', set: gameSetup.Items },
                { name: 'Spells', set: gameSetup.Spells },
                { name: 'Villagers', set: gameSetup.Villagers }
            ];
        }

        function GameBoardVm(hub) {
            var self = this;
            self.Dungeon = ko.observable();
            self.HeroDecks = new VillageSectionVm();
            self.WeaponDecks = new VillageSectionVm();
            self.ItemDecks = new VillageSectionVm();
            self.SpellDecks = new VillageSectionVm();
            self.VillagerDecks = new VillageSectionVm();
            self.HandParts = ko.observable();
            self.Status = ko.observable();
            self.playerPanel = new PlayerPanelVm(hub);

            self.update = function (model) {
                var gameDecks = {};
                self.Decks = gameDecks;
                self.Dungeon(model.Dungeon);
                self.HeroDecks.update(model.HeroDecks, gameDecks);
                self.WeaponDecks.update(model.WeaponDecks, gameDecks);
                self.ItemDecks.update(model.ItemDecks, gameDecks);
                self.SpellDecks.update(model.SpellDecks, gameDecks);
                self.VillagerDecks.update(model.VillagerDecks, gameDecks);
                self.HandParts([model.Hand]);
                self.Status(model.Status);
            };
        }

        function VillageSectionVm() {
            var self = this;
            self.SectionName = ko.observable();
            self.Decks = ko.observableArray();

            self.update = function (m, gameDecks) {
                self.SectionName(m.SectionName);
                m.Decks.forEach(function (deck) {
                    var o = ko.observable(deck);
                    self.Decks.push(o);
                    gameDecks[deck.Id] = o;
                });
            };
        }

        function PlayerPanelVm(hub) {
            var self = this;
            self.hub = hub;
            self.text = ko.observable();
            self.commandsVisible = ko.observable(false);

            self.village = function () {
                self.commandsVisible(false);
                hub.village();
            };

            self.dungeon = function () {
                self.commandsVisible(false);
                hub.dungeon();
            };

            self.prepare = function () {
                self.commandsVisible(false);
                hub.prepare();
            };

            self.rest = function () {
                self.commandsVisible(false);
                hub.rest();
            };
        }

        function UseAbilityVm(hub) {
            var self = this;
            self.hub = hub;
            self.Visible = ko.observable(false);
            self.Phase = ko.observable();
            self.Abilities = ko.observableArray();
            self.ShowDone = ko.observable(true);
            var posSet = false;

            self.update = function (model) {
                self.Phase(model.Phase);
                self.Abilities(model.Abilities);
                self.Visible(model.Abilities.length > 0);
                self.ShowDone(!model.Required);
                if (self.Visible()) {
                    if (!posSet) {
                        positionPopup('#useAbility');
                        posSet = true;
                    }
                }
            };

            self.use = function (ability) {
                self.hub.useAbility({ Phase: self.Phase, AbilityId: ability.Id });
            };

            self.Done = function () {
                self.Visible(false);
                self.hub.useAbility({ Phase: self.Phase });
            };
        }

        function SelectCardsVm(hub) {
            var self = this;
            self.Visible = ko.observable(false);
            self.Caption = ko.observable();
            self.Message = ko.observable();
            self.Cards = ko.observableArray();
            self.Min = ko.observable(1);
            self.Max = ko.observable(1);
            self.Enabled = ko.observable(false);

            self.Done = function () {
                self.Visible(false);
                hub.selectCards(self.Selected.keys());
            };

            self.update = function (model) {
                self.Caption(model.Caption);
                self.Message(model.Message);
                self.Min(model.Min);
                self.Max(model.Max);
                self.Selected = new Hash();
                self.Enabled(enabled());
                self.Visible(true);

                var cards = model.Cards;
                var cardsParts = [];
                while (cards.length > 0) {
                    cardsParts.push(cards.slice(0, 4));
                    cards = cards.slice(4);
                }
                self.Cards(cardsParts);

                positionPopup('#selectCards');
                $('a', $('#selectCards')).toggle(function () {
                    $(this).addClass('down');
                    var card = ko.dataFor(this);
                    self.Selected[card.Id] = true;
                    self.Enabled(enabled());
                }, function () {
                    $(this).removeClass('down');
                    var card = ko.dataFor(this);
                    delete self.Selected[card.Id];
                    self.Enabled(enabled());
                }
                );
            };

            function enabled() {
                return (self.Selected.size() >= self.Min() && self.Selected.size() <= self.Max());
            }
        }

        function positionPopup(div) {
            var board = $('#gameBoard');
            div = $(div);
            div.css({
                position: 'absolute',
                top: board.position().top + (board.height() - div.height()) / 2,
                left: ($(window).width() - div.width()) / 2
            });
        }


    </script>
</body>
</html>
