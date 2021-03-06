﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Thunderstone.Lib.Events;
using Slugburn.Thunderstone.Lib.Messages;
using Slugburn.Thunderstone.Lib.Models;
using Slugburn.Thunderstone.Lib.Selectors.Sources;

namespace Slugburn.Thunderstone.Lib.Selectors
{
    public class SelectionContext : ISelectionContext, ISelectSource, IDefineSelection, ISelectionCallback 
    {
        public SelectionContext(Player player, Ability triggeredBy)
        {
            Player = player;
            TriggeredBy = triggeredBy;
            Min = 1;
            Max = 1;
            Callbacks = new List<Action<ISelectionContext>>();
        }

        public Game Game { get { return Player.Game; } }
        public Player Player { get; private set; }

        ISelectionContext ISelectSource.SelectionContext
        {
            get { return this; }
        }

        public ICardSource Source { get; set; }
        public IList<Card> Selected { get; set; }
        public Ability TriggeredBy { get; private set; }

        public List<Action<ISelectionContext>> Callbacks { get; private set; }
        public string Caption { get; set; }
        public string Message { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public Func<Card, bool> Filter { get; set; }

        public void SendRequest(Action<SelectionContext> continuation)
        {
            var selectFrom = GetSourceCards();
            var ev = new SelectingCards(TriggeredBy, selectFrom);
            Player.PublishEvent(ev);
            selectFrom = ev.Selection;
            Action<IEnumerable<long>> callback = selectedIds =>
            {
                Selected = selectFrom.Where(card => selectedIds.Contains(card.Id)).ToArray();
                Callbacks.Each(action => action(this));
                continuation(this);
            };
            if (selectFrom.Count <= Min)
            {
                var selectedIds = selectFrom.Select(card => card.Id);
                callback(selectedIds);
            }
            else
            {
                SelectCard(selectFrom, callback);
            }
        }

        public List<Card> GetSourceCards()
        {
            return (Filter == null ? Source.GetCards() : Source.GetCards().Where(card => Filter(card))).ToList();
        }

        private void SelectCard(IEnumerable<Card> cards, Action<IEnumerable<long>> callback)
        {
            var message = new SelectCardsMessage { Caption = Caption, Message = Message, Cards = CardModel.From(cards), Min = Min, Max = Max };
            Player.SelectCardsCallback = callback;
            Player.View.SelectCards(message);
        }
    }
}