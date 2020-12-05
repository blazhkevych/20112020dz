namespace _20112020dz
{
    internal class Player
    {
        //------------------------------------------------------------------------
        public string Name { get; }

        public CardDeck CardDeck;

        //---------------------------------------------------------------------
        public Player(string name)
        {
            Name = name;
            CardDeck = new CardDeck();
        }
    } //------------------------------------------------------------------------
}