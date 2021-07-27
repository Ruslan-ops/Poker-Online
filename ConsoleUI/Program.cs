using System;
using System.Collections.Generic;
using PokerEngine;
using System.Text.Json;


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
            Table table = new Table(5, 3000, 4000, 100);
            for (int i = 0; i < 3; i++)
            {
                table.AddPlayer("Player" + Convert.ToString(i));
            }
            table.ShowDownEvent += (players) =>
            {
                Console.WriteLine("\n*****SHOW DOWN*****");
                foreach (var p in players)
                {
                    Console.WriteLine($"Player: {p.Name}, Hand: {p.Combination}");
                }
            };

            table.DealFinishedEvent += () =>
            {
                Console.WriteLine("Deal finished");
            };
            table.DealStartedEvent += () =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("***NEW DEAL STARTED***");
                Console.ResetColor();
            };

            table.WinnersGotPotEvent += (awards) =>
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("________________________");
                foreach (var award in awards)
                {
                    Player winner = award.Winner;
                    int wonChips = award.WonPotSize;
                    Console.WriteLine($"Winner {winner.Name} has {winner.Combination.Type} and gets {wonChips}");
                }
                Console.ResetColor();
            };            
            while (true)
            {
                try
                {
                    int choice;
                    do
                    {
                        if (!table.IsDeal)
                        {
                            MakeRebuys(table.GetPlayers());
                            table.StartNewDeal();
                        }
                        Player player = table.GetWhoseMove();
                        List<MoveAlias> allowedMoves = table.GetAllowedMovesFor(player);
                        PrintRound(table, player);
                        PrintAllowedMoves(allowedMoves);
                        choice = Convert.ToInt32(Console.ReadLine());
                        if(choice > 0 && choice < 5)
                        {

                            Move move = MakeMove(allowedMoves[choice - 1], table, player);
                            PrintMoveMessage(move, player);
                        }
                        else
                        {
                            throw new Exception("Undefined action");
                        }
                    }
                    while (true);
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }


        }

        private static void MakeRebuys(IEnumerable<Player> players)
        {
            foreach(var player in players)
            {
                if(!player.HasChips)
                {
                    player.AddChips(3000);
                }
            }
        }

        private static void PrintMoveMessage(Move move, Player player)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{player.Name} made {move.Alias.ToString().ToLower()}");
            string betSizeMessage = string.Empty;
            if(move is BettingMove bettingMove)
            {
                betSizeMessage = $" {bettingMove.BetSize} chips";
            }
            Console.WriteLine(betSizeMessage);
            Console.ResetColor();
        }

        private static void PrintAllowedMoves(List<MoveAlias> allowedMoves)
        {
            Console.WriteLine("Choose move");
            for(int i = 1; i <= allowedMoves.Count; i++)
            {
                Console.WriteLine($"{i}) {allowedMoves[i-1].ToString()}");
            }
            Console.Write("->");
        }

        static void PrintRound(Table table, Player player)
        {
            Console.WriteLine("________________________");
            Console.WriteLine(table.CurrentRound.ToString().ToUpper());
            string boardCards = CardListToString(table.GetBoardCards());
            Console.WriteLine($"Board: {boardCards}");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{player.Name}");
            Console.ResetColor();
            Console.WriteLine(", It's your turn!");
            Console.WriteLine($"Your hand: {player.Hand}");
            Console.WriteLine($"Your combination: {player.Combination}");
            Console.WriteLine($"Stack: {player.Stack} chips | Pot: {table.PotSize}");
            Console.WriteLine($"Current max bet size: {table.CurrentMaxBetSize} | To bet: {table.CurrentMaxBetSize - table.GetBetSizeOf(player)}");
        }
        static Move MakeMove(MoveAlias moveAlias, Table table, Player player)
        {
            Move move;
            switch (moveAlias)
            {
                case MoveAlias.Bet:
                    Console.WriteLine("How much you wanna bet?");
                    int betSize = Convert.ToInt32(Console.ReadLine());
                    move = new Bet(betSize);
                    break;
                case MoveAlias.Raise:
                    Console.WriteLine("How many times do you want to raise current bet?");
                    double coef = Convert.ToDouble(Console.ReadLine());
                    move = new Raise(coef);
                    break;
                case MoveAlias.Fold:
                    move = new Fold();
                    break;
                case MoveAlias.Check:
                    move = new Check();
                    break;
                case MoveAlias.Call:
                    move = new Call();
                    break;
                case MoveAlias.AllIn:
                    move = new AllIn();
                    break;
                default:
                    throw new Exception("Undefined action");
            }
            table.MakeMove(player, move);
            return move;
        }
    }   

}
