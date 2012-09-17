﻿<!DOCTYPE html>
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

            a > .minicard {
                cursor: pointer;
                box-shadow: 3px 3px 0px #777;
            }

            a.down > .minicard {
                box-shadow: -3px -3px 0px #777;
                background: burlywood;
                outline-offset: 2px;
            }

            .table {
                display: table;
            }

            .row {
                display: table-row;
            }

            .cell {
                display: table-cell;
            }

            .iblock {
                display: inline-block;
                position: relative;
            }
            .block {
                display: block;
            }


            .main {
                border: solid;
                border-radius: 10px;
                padding: 5px; 
                display: table;
                margin: auto;
                position: relative;
                background: white;
                box-shadow: 5px 5px 5px darkslategrey;
            }

            .name {
                font-size: 10px;
                font-weight: bold;
                font-family: sans-serif;
            }

            .description {
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
            }

            .minicard.blank {
                background: -webkit-linear-gradient(darkgray, white); 
                background: linear-gradient(darkgray, white);
            }

            .minicard {
                background: blanchedalmond; 
            }


            .minicard > .name {
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

            .minicard > .equipped {
                position: absolute;
                width: 90px;
                top: 25px;
                display: block;
                text-align: center;
                background: white;
                border: solid 1px sandybrown;
                padding: 4px;
                border-radius: 16px;
                font: 8px sans-serif;
                font-style: italic;
            }

            .setupLabel {
                display: table-cell;
                float: left;
                font-weight: bold;
                margin: 5px;
                padding: 3px 10px 3px 10px;
                text-align: right;
                vertical-align: central;
            }

            .ui-tooltip-card {
                background-color: lightgray;
                border-color: black;
                border-radius: 10px;
                border-width: 2px;
                color: black;
            }

            .coin {
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

            .coin.black {
                color: white;
                background: -webkit-radial-gradient(center, ellipse cover, gray 0%, black 70%);
                background: radial-gradient(ellipse at center, gray 0%, black 70%);
            }

            .coin.gold {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, gold 70%);
                background: radial-gradient(ellipse at center, white 0%, gold 70%);
            }

            .coin.orange {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, orange 70%);
                background: radial-gradient(ellipse at center, white 0%, orange 70%);
            }

            .coin.brown {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, tan 70%);
                background: radial-gradient(ellipse at center, white 0%, tan 70%);
            }

            .coin.light {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, lightyellow 70%);
                background: radial-gradient(ellipse at center, white 0%, lightyellow 70%);
            }

            .coin.red {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, red 70%);
                background: radial-gradient(ellipse at center, white 0%, red 70%);
            }

            .coin.blue {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, deepskyblue 70%);
                background: radial-gradient(ellipse at center, white 0%, deepskyblue 70%);
            }

            .coin.xp {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, silver 70%);
                background: radial-gradient(ellipse at center, white 0%, silver 70%);
            }

            .coin.vp {
                background: -webkit-radial-gradient(center, ellipse cover, white 0%, darkturquoise 70%);
                background: radial-gradient(ellipse at center, white 0%, darkturquoise 70%);
            }

            .center {
                position: static;
                display: table;
                margin: 0px auto;
            }

            .cardPopup { }

            .sectionTitle { font-family: sans-serif;font-size: 12px;background-color: black;color: white;margin: 2px;padding: 2px 4px; }

            .smallButton {font-size: 10px; }
        </style>
    </head>
    <body>
        <div id="gameSetup" class="main">
            <table data-bind="template: { name: 'setup-list-template', foreach: sets }"></table>
            <button id="startGame" style="display: inherit; margin: 5px auto; width: 100px;">Start Game</button>
        </div>

        <div id="gameBoard" class="main" >
            <div id="dungeon" data-bind="with: Dungeon" class="block">
                <span class="block sectionTitle">The Dungeon Hall</span>
                <div  class="block">
                    <!-- ko foreach: Ranks -->
                    <div class="iblock">
                        <div class="block sectionTitle">Rank <span data-bind="text: Number"></span></div>
                        <div data-bind="template: {name: 'minicard-template', data: $.extend(Card, {empty: Card===null}) }"></div>
                    </div>
                    <!-- /ko -->
                    <div class="iblock">
                        <span class="block sectionTitle">Deck</span>
                        <div class="minicard" style="position: relative; ">
                            <span data-bind="text: DeckCount" class="coin" style="left:80%; top:47px;"></span>
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
                    <div data-bind="template: {name: 'deck-section-template', data: VillagerDecks }" class="block" ></div>
                </div>
            </div>
            <div id="hand" class="block" >
                <span data-bind="with: Status" class="block sectionTitle" style="width: 664px">
                    <span class="cell">Hand</span>
                    <span class="cell" style="width: 30px;">
                        <span data-bind="text: Gold" class="center coin gold" ></span>
                    </span>
                    <span class="cell">/</span>
                    
                    <span class="cell" style="width: 30px;">
                        <span data-bind="text: PhysicalAttack" class="center coin red" ></span>
                    </span>
                    <span class="cell" style="width: 30px;">
                        <span data-bind="text: MagicAttack" class="center coin blue" ></span>
                    </span>
                    <span class="cell" style="width: 30px;">
                        <span data-bind="text: Light" class="center coin light" ></span>
                    </span>
                </span>
                <!-- ko foreach: HandParts -->
                <div data-bind="foreach: $data" class="block">
                    <div class="iblock">
                        <div data-bind="template: {name: 'minicard-template', data: $data }" ></div>
                    </div>
                </div>
                <!-- /ko -->
            </div>
            <div style="position: absolute; top:222px;left:460px;height: 190px;width: 222px;border-radius: 12px;background-color: white; box-sizing: border-box;padding: 3px;">
                <div style="position: relative; border:black solid 2px;border-radius: 10px; height: 100%; box-sizing: inherit; padding: 4px; ">
                    <div data-bind="with: Status" class="table" style="margin: 0px auto">
                        <div class="row" style="margin: 0px auto">
                            <span class="cell sectionTitle">Gold</span>
                            <div class="cell" style="width: 30px;">
                                <span data-bind="text: Gold" class="center coin gold" ></span>
                            </div>
                            <span class="cell sectionTitle">Xp</span>
                            <div class="cell" style="width: 30px;">
                                <span data-bind="text: Xp" class="center coin xp" style=""></span>
                            </div>
                            <span class="sectionTitle cell">Vp</span>
                            <div class="cell" style="width: 30px; ">
                                <span data-bind="text: Vp" class="center coin vp" style=""></span>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div id="playerPanel">
                    </div>
                </div>
            </div>

        </div>

        <div id="useAbility" data-bind="style: {display: Visible() ? 'table' : 'none' }" >
            <div class="main">
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

        <div id="selectCards" data-bind="style: {display: Visible() ? 'table' : 'none' }" >
            <div class="main">
                <div data-bind="text: Caption" class="block center sectionTitle" style="width: 100%"></div>
                <div data-bind="text: Message" class="block" ></div>
                <div class="table" style="margin-bottom: 5px;">
                    
                <!-- ko foreach: Cards -->
                <div data-bind="foreach: $data" class="row">
                    <div class="cell" style="padding: 4px; position: relative">
                        <a data-bind="template: {name: 'minicard-template', data: $data }" ></a>
                    </div>
                </div>
                <!-- /ko -->

<!--                    <div data-bind="foreach: Cards" class="row" style="margin-bottom: 5px;">
                        <div class="cell" style="padding: 4px; position: relative">
                            <a data-bind="template: {name: 'minicard-template', data: $data }" ></a>
                        </div>
                    </div>-->
                </div>
                <button data-bind="enable: Enabled, click: Done" class="center" style="margin-top: 5px;">Done</button>
            </div>
        </div>
        
        <div id="log" style="position: absolute;top:0px; left:0px;height: 800px; width: 200px; overflow: auto;font-size: 10px; font-family: sans-serif "></div>
        
        <script type="text/html" id="deck-section-template">
            <span data-bind="text: SectionName" class="block sectionTitle"></span>
            <div data-bind="foreach: Decks" class="block">
                <div data-bind="attr: {id: 'deck_' + Id}, template: {name: 'minicard-template', data: $.extend(TopCard, {empty: (Count === 0)}) }" class="iblock" ></div>
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
            <span class="minicard" >
                <span data-bind="text: Name, visible: Name" class="name"></span>
                <!-- ko if: $data.Owner === 'Player' -->
                <span data-bind="text: Gold, visible:Gold || Gold===0" class="coin gold" style="left: 44px; top:25px; "></span>
                <span data-bind="text: Equipped, visible: Equipped" class="equipped"></span>
                <span class="block" style="left: 2px; top:47px; position: absolute;padding: 0px;margin: 0px; ">
                    <span data-bind="text: PhysicalAttack, visible:PhysicalAttack" class="coin red cell" style="position: static;"></span>
                    <span data-bind="text: PotentialPhysicalAttack, visible:PotentialPhysicalAttack || PotentialPhysicalAttack===0" class="coin cell" style="background-color: rgba(255, 0, 0, 0.40); position: static;"></span>
                    <span data-bind="text: MagicAttack, visible:MagicAttack" class="coin blue cell" style="position: static;"></span>
                    <span data-bind="text: PotentialMagicAttack, visible:PotentialMagicAttack || PotentialMagicAttack===0" class="coin cell" style="background: rgba(0,191,255,0.40) ; position: static;"></span>
                    <span data-bind="text: Light, visible:Light" class="coin cell light" style="background-color: lightyellow; position: static;"></span>
                </span>
                <!-- /ko -->
                <!-- ko if: $data.Owner === 'Village' -->
                <span data-bind="text: Cost, visible:Cost|| Cost===0" class="coin orange" style="left: 44px; top:25px; "></span>
                <span data-bind="text: $parent.Count, visible:$parent.Count" class="coin" style="left:80%; top:47px;"></span>
                <!-- /ko -->
                <!-- ko if: $data.Owner === 'Dungeon' -->
                <span data-bind="text: Health, visible:Health" class="coin red" style="left: 44px; top:25px; "></span>
                <span class="block" style="left: 2px; top:47px; position: absolute;padding: 0px;margin: 0px; ">
                    <span data-bind="text: Darkness, visible:Darkness" class="coin black cell" style="position: static;"></span>
                </span>
                <!-- /ko -->
                <span style="display: none">
                    <span class="cardPopup" data-bind="template: {name: 'card-template', data: $data }"></span>
                </span>
            </span>
            <!-- /ko -->
            <!-- ko ifnot: !$data.empty && $data -->
            <span class="minicard blank" >
            </span>
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
                <span data-bind="text: Vp, visible: Vp" class="coin vp" style="left: 195px; top: 92%; "></span>
            </div>
        </script>

        <script type="text/javascript">
            $(document).ready(function () {
                $('#gameSetup').hide();
                $('#gameBoard').hide();

                var gameBoard;
                var useAbility = new UseAbilityVm(send);
                ko.applyBindings(useAbility, $('#useAbility').get(0));

                var selectCards = new SelectCardsVm(send);
                ko.applyBindings(selectCards, $('#selectCards').get(0));

                var hub = $.connection.playerHub;
                
                hub.displayGameSetup = function (message) {
                    var vm = new GameSetupVm(message);
                    ko.applyBindings(vm, $('#gameSetup').get(0));
                    createTooltips();
                    $('#gameSetup').show();
                };

                hub.displayGameBoard = function(message) {
                    gameBoard = new GameBoardVm(message);
                    ko.applyBindings(gameBoard, $('#gameBoard').get(0));
                    $('#gameBoard').show();
                    createTooltips();
                };

                hub.displayStartTurn = function() {
                    displayMenu();
                };

                $.connection.hub.start().done(function() {
                    hub.newPlayer();
                });

//                var ws = new WebSocket('ws://localhost:8181');
//                ws.onopen = function () {
//                    send('GameSetup');
//                };

//                ws.onmessage = function(e) {
//                    var parsed = JSON.parse(e.data);
//                    var id = parsed.Id;
//                    var body = parsed.Body;
//                    switch (id) {
//                    case 'GameSetup':
//                        var vm = new GameSetupVm(body);
//                        ko.applyBindings(vm, $('#gameSetup').get(0));
//                        createTooltips();
//                        $('#gameSetup').show();
//                        break;
//                    case 'GameBoard':
//                        gameBoard = new GameBoardVm(body);
//                        ko.applyBindings(gameBoard, $('#gameBoard').get(0));
//                        $('#gameBoard').show();
//                        createTooltips();
//                        break;
//                    case 'StartTurn':
//                        displayMenu();
//                        break;
//                    case 'BuyCard':
//                        $('#playerPanel').empty().html('Buy a card.');
//                        body.AvailableDecks.forEach(function(deckId) {
//                            var button = $('<button class="center smallButton buyButton" style="position:absolute; top:48px; left: 39px;">Buy</button>');
//                            $('#deck_' + deckId).append(button);
//                            button
//                                .click(function() {
//                                    send('BuyCard', deckId);
//                                    $(".buyButton").remove();
//                                });
//                        });
//                        break;
//                    case 'UpdateDungeon':
//                        gameBoard.Dungeon(body);
//                        createTooltips('#dungeon');
//                        break;
//                    case 'UpdateDeck':
//                        gameBoard.Decks[body.Id](body);
//                        createTooltips('#deck_' + body.Id);
//                        break;
//                    case 'UpdateHand':
//                        var hand = body.Hand;
//                        var handParts = [];
//                        while(hand.length>0) {
//                            handParts.push(hand.slice(0, 6));
//                            hand = hand.slice(6);
//                        }
//                        gameBoard.HandParts(handParts);
//                        createTooltips('#hand');
//                        break;
//                    case 'UpdateStatus':
//                        gameBoard.Status(body);
//                        break;
//                    case 'UseAbility':
//                        if (body) {
//                            useAbility.update(body);
//                        } else {
//                            useAbility.Visible(false);
//                        }
//                        break;
//                    case 'SelectCards':
//                        selectCards.update(body);
//                        createTooltips('#selectCards');
//                        break;
//                    case 'Log':
//                        $('#log').append(body + '<br/>');
//                        break;
//                    default:
//                        alert("Unknown message ID " + id);
//                    }
//                };

                $('#startGame').click(function() {
                    ko.applyBindings({ sets: [] }, $('#gameSetup').get(0)); // unbind the game setup
                    $('#gameSetup').hide();
                    hub.startGame();
                    send("StartGame");
                });

                function send(id, body) {
                    ws.send(JSON.stringify({ id: id, body: body }));
                }
                
                function displayMenu(){
                    var panel = $('#playerPanel');
                    panel.empty();
                    $.each(['Village', 'Dungeon', 'Prepare', 'Rest'], function () {
                        var command = this;
                        var button = $('<button class="center" style="width:100px;">' + command + '</Button>')
                            .click(function () {
                                send(command);
                                $('#playerPanel').empty();
                            });
                        panel.append(button);
                    });
                }
                
                function createTooltips(context) {
                    $('.minicard',context).each(function () {
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
            
            function GameBoardVm(model) {
                var self = this;
                self.Decks = {};
                self.Dungeon = ko.observable(model.Dungeon);
                self.HeroDecks = createVillageSection(model.HeroDecks);
                self.WeaponDecks = createVillageSection(model.WeaponDecks);
                self.ItemDecks = createVillageSection(model.ItemDecks);
                self.SpellDecks = createVillageSection(model.SpellDecks);
                self.VillagerDecks = createVillageSection(model.VillagerDecks);
                self.HandParts = ko.observable([model.Hand]);
                self.Status = ko.observable(model.Status);

                function createVillageSection(section) {
                    var vm = {
                        SectionName: section.SectionName,
                        Decks: []
                    };
                    section.Decks.forEach(function (deck) {
                        var o = ko.observable(deck);
                        vm.Decks.push(o);
                        self.Decks[deck.Id] = o;
                    });
                    return vm;
                }
            }
            
            function UseAbilityVm(send) {
                var self = this;
                self.send = send;
                self.Visible = ko.observable(false);
                self.Abilities = ko.observableArray();
                self.ShowDone = ko.observable(true);
                var posSet = false;

                self.update = function (model) {
                    self.Phase = model.Phase;
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

                self.use = function(ability) {
                    self.send("UseAbility", { Phase: self.Phase, AbilityId: ability.Id });
                };

                self.Done = function () {
                    self.Visible(false);
                    self.send("UseAbility", { Phase: self.Phase });
                };
            }
            
            function SelectCardsVm(send) {
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
                    send("SelectCards", self.Selected.keys());
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
                    $('a', $('#selectCards')).toggle(function() {
                        $(this).addClass('down');
                        var card = ko.dataFor(this);
                        self.Selected[card.Id] = true;
                        self.Enabled(enabled());
                    }, function() {
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