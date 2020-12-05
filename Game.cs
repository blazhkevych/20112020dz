using System;
using System.Collections.Generic;
using System.Linq;

namespace _20112020dz
{
    internal class Game
    {
        //------------------------------------------------------------------------
        private readonly List<Player> _players;

        private readonly CardDeck _cardDeck;

        //---------------------------------------------------------------------
        public Game(params Player[] players)
        {
            if (players.Length == 0)
                _players = new List<Player> {new Player(RandomName.Boy(true)), new Player(RandomName.Boy(true))};
            else if (players.Length == 1)
                _players = new List<Player> {players.First(), new Player(RandomName.Boy(true))};
            else
                _players = new List<Player>(players);

            _cardDeck = new CardDeck();
            _cardDeck.Full();
        }

        //---------------------------------------------------------------------
        private void DealCards()
        {
            for (var i = 0; _cardDeck.Count != 0;)
            {
                _players[i].CardDeck.Add(_cardDeck.Take());
                if (++i == _players.Count) i = 0;
            }
        }

        //---------------------------------------------------------------------
        public void Play()
        {
            // Настройка отображения
            var between = 16;
            Console.BufferHeight = 32;
            Console.BufferWidth =
                _players.Count * between + 2 < Console.WindowWidth
                    ? Console.WindowWidth
                    : _players.Count * between + 2;

            DealCards();
            while (true)
            {
                // Рандомизация очередности хода
                //_players = _players.ToArray().Mix().ToList();

                // Имена игроков
                foreach (var player in _players)
                    Console.Write((' ' + player.Name
                                       + (player.CardDeck.Count < 10 ? "[0" : "[")
                                       + player.CardDeck.Count + ']').PadRight(between));
                Console.Write("\n ");

                // Карты на стол
                var table = new Card[_players.Count];
                for (var i = 0; i < _players.Count; i++)
                    table[i] = _players[i].CardDeck.Take();

                // Если на столе только 6 и T, тогда 6 забирают T
                var omg = true;
                foreach (var card in table)
                    if (card.Value.Weight != 6 && card.Value.Weight != 14)
                    {
                        omg = false;
                        break;
                    }

                // Формируем список победителей
                var winners =
                    omg
                        ? table.ToLookup(i => i.Value.Weight != 6 ? i.Value.Weight : 15,
                            i => Array.IndexOf(table, i)
                        ).OrderBy(i => i.Key).Last()
                        : table.ToLookup(i => i.Value.Weight,
                            i => Array.IndexOf(table, i)
                        ).OrderBy(i => i.Key).Last();

                // Отрисовка игрального стола
                for (var i = 0; i < _players.Count; i++)
                    table[i].Print(between, winners.Contains(i));
                Console.SetCursorPosition(0, Console.CursorTop + 7);
                Console.WriteLine("".PadRight(between * _players.Count - 3, '\''));

                // Победители забирают каждый свою карту
                foreach (var winner in winners)
                {
                    _players[winner].CardDeck.Add(table[winner]);
                    table[winner] = null;
                }

                // Победители забирают карты со стола (по одной) в порядке розыгрыша
                RecursionAdd(0);

                //'''''''''''''''''''''''''''''''''''''''''
                void RecursionAdd(int ind)
                {
                    foreach (var winner in winners)
                    {
                        if (table[ind] != null)
                        {
                            _players[winner].CardDeck.Add(table[ind]);
                            table[ind] = null;
                        }

                        if (++ind == table.Length) break;
                    }

                    if (ind < table.Length) RecursionAdd(ind);
                }
                //.........................................

                // Проверка на наличие карт
                for (var i = 0; i < _players.Count;)
                    if (_players[i].CardDeck.Count == 0) _players.Remove(_players[i]);
                    else i++;

                // Последний игрок?
                if (_players.Count == 1)
                {
                    Console.WriteLine(_players.First().Name + " WIN!");
                    break;
                }

                // Очистка консоли если достигнут придел буфера
                Console.BufferHeight += 10;
                if (Console.CursorTop > short.MaxValue - 64)
                {
                    Console.Write(" Enter to continue..");
                    while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                    Console.Clear();
                }
            }
        }
    } //------------------------------------------------------------------------
}