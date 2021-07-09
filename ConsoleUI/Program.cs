using System;
using System.Collections.Generic;
using PokerEngine;

namespace ConsoleUI
{
    class Program
    {
        static string CardListToString(IEnumerable<Card> cards)
        {
            string result = "|";
            foreach (Card card in cards)
            {
                result += card.ToString() + "|";
            }
            return result;
        }

        static void Main(string[] args)
        {
            Table table = new Table(5, 3000, 100);
            for (int i = 0; i < 3; i++)
            {
                table.AddPlayer("Shark" + Convert.ToString(i));
            }
            table.ShowDownEvent += (players) =>
            {
                Console.WriteLine("\n*****SHOW DOWN*****");
                foreach (var p in players)
                {
                    Console.WriteLine($"Player: {p.Name}, Hand: {p.Combination}");
                }
            };

            table.WinnersGotPotEvent += (awards) =>
            {
                Console.WriteLine("________________________");
                foreach (var award in awards)
                {
                    Player winner = award.Winner;
                    int wonChips = award.WonPotSize;
                    Console.WriteLine($"Winner {winner.Name} has {winner.Combination.Type} and gets {wonChips}");
                }
            };

            Combination comb = Combination.FromCards(new List<Card> { new Card( Rank.King, Suit.Hearts ), new Card(Rank.Queen, Suit.Spades ), new Card(Rank.Nine, Suit.Hearts),
                                                                          new Card(Rank.Four, Suit.Clubs ), new Card(Rank.Eight, Suit.Diamonds ), new Card(Rank.Two, Suit.Hearts), new Card(Rank.Three, Suit.Diamonds ) });
            /*Console.WriteLine(comb.Type);
            foreach (Card card in comb)
            {
                Console.WriteLine(card);
            }*/
            /*Enum en = (Enum)3;
            List<int> li = new List<int>();
            li.Clear();
            Console.WriteLine(en);*/
            while (true)
            {

                string choice = "1";
                do
                {
                    if (!table.IsDeal)
                    {
                        table.StartNewDeal();
                    }
                    Player player = table.GetWhoseMove();

                    Console.WriteLine("________________________");
                    Console.WriteLine(table.CurrentRound.ToString().ToUpper());
                    string boardCards = CardListToString(table.GetBoardCards());
                    Console.WriteLine($"Board: {boardCards}");
                    Console.WriteLine($"{player.Name}, It's your turn!");
                    Console.WriteLine($"Your hand: {player.Hand}");
                    Console.WriteLine($"Your combination: {player.Combination}");
                    Console.WriteLine($"Stack: {player.Stack} chips | Pot: {table.PotSize}");
                    Console.WriteLine($"Current max bet size: {table.CurrentMaxBetSize} | To bet: {table.CurrentMaxBetSize - table.GetBetSizeOf(player)}");
                    choice = Console.ReadLine();
                    
                    switch (choice)
                    {
                        case "bet":
                            Console.WriteLine("How much you wanna bet?");
                            int betSize = Convert.ToInt32(Console.ReadLine());
                            table.MakeMove(player, new Bet(betSize));
                            break;
                        case "raise":
                            Console.WriteLine("How much you wanna raise?");
                            double coef = Convert.ToDouble(Console.ReadLine());
                            table.MakeMove(player, new Raise(coef));
                            break;
                        case "fold":
                            table.MakeMove(player, new Fold());
                            break;
                        case "check":
                            table.MakeMove(player, new Check());
                            break;
                        case "call":
                            table.MakeMove(player, new Call());
                            break;
                        case "allin":
                            table.MakeMove(player, new AllIn());
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Undefined action");
                            Console.ResetColor();
                            break;
                    }
                    
                }
                while (choice != "0");
            }
        }
    }   
}
