using System;
using System.Collections.Generic;
using PokerEngine;
using System.Text.Json;


namespace ConsoleUI
{
    class Program
    {
        enum Enum
        {
            first = 1,
            second = 2
        }

        static string CardListToString(IEnumerable<Card> cards)
        {
            string result = "|";
            foreach (Card card in cards)
            {
                result += card.ToString() + "|";
            }
            return result;
        }

        struct CombinationInfo
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public List<CardInfo> Cards { get; set; }
            public Enum Enum { get; set; }

           
        }

        struct CardInfo
        {
            public Rank Rank { get; set; }
            public Suit Suit { get; set; }
            public CardInfo(Rank rank, Suit suit)
            {
                Rank = rank;
                Suit = suit;
            }

        }

        static void Main(string[] args)
        {
            Table table = new Table(5, 3000, 4000, 100);
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




            /*Hand hand = new Hand(new Card(Rank.Ace, Suit.Hearts), new Card(Rank.Five, Suit.Spades));
            string json = JsonSerializer.Serialize<Combination>(comb);
            Console.WriteLine(json);
            Combination restoredComb = JsonSerializer.Deserialize<Combination>(json);
            Console.WriteLine(restoredComb.Type);*/

            CombinationInfo p = new CombinationInfo();
            p.Cards = new List<CardInfo> { new CardInfo( Rank.King, Suit.Hearts ), new CardInfo(Rank.Queen, Suit.Spades ), new CardInfo(Rank.Nine, Suit.Hearts),
                                                                          new CardInfo(Rank.Four, Suit.Clubs ), new CardInfo(Rank.Eight, Suit.Diamonds ), new CardInfo(Rank.Two, Suit.Hearts), new CardInfo(Rank.Three, Suit.Diamonds )};
            string json = JsonSerializer.Serialize<CombinationInfo>(p);
            Console.WriteLine(json);
            CombinationInfo restoredP = JsonSerializer.Deserialize<CombinationInfo>(json);
            Console.WriteLine(restoredP.Cards);
            Console.WriteLine(json.Length);

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
