using System.Collections.Generic;

namespace _20112020dz
{
    internal class CardDeck
    {
        //------------------------------------------------------------------------
        public int Count => _cardDeck.Count;

        //---------------------------------------------------------------------
        private readonly Queue<Card> _cardDeck;

        public CardDeck()
        {
            _cardDeck = new Queue<Card>(36);
        }

        //---------------------------------------------------------------------
        public void Mix()
        {
            var mixedCards = _cardDeck.ToArray().Mix();
            _cardDeck.Clear();

            foreach (var card in mixedCards)
                _cardDeck.Enqueue(card);
        }

        //---------------------------------------------------------------------
        public void Full()
        {
            _cardDeck.Clear();
            for (var suit = 0; suit < 4; suit++)
            for (var value = 6; value < 15; value++)
                _cardDeck.Enqueue(new Card(suit, value));
            Mix();
        }

        //---------------------------------------------------------------------
        public void Add(Card card)
        {
            _cardDeck.Enqueue(card);
        }

        public Card Take()
        {
            return _cardDeck?.Dequeue();
        }
    } //------------------------------------------------------------------------
}