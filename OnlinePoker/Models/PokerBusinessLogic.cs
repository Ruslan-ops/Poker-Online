using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlinePoker.Models.ViewModels;
using PokerEngine;

namespace OnlinePoker.Models
{
    public class PokerBusinessLogic
    {
        public event Action<PokerEngine.Table, Player> NewPlayerWasAdded;
        private readonly Dictionary<Guid, PokerEngine.Table> _tables;

        public PokerBusinessLogic()
        {
            _tables = new Dictionary<Guid, PokerEngine.Table>();
        }

        public async Task SitPlayerAtTableAsync(string tableId, string userName, AppDbContext appDbContext)
        {
            var table = _tables[new Guid(tableId)];
            await Task.Run(() =>
            {
                table.AddPlayer(userName);
            });
            await AddSittingPlayerToDbAsync(tableId, userName, appDbContext);
            if (table.GetPlayers().Where(p => p.HasChips).Count() == 2)
            {
                StartNewDeal(table, tableId, appDbContext);
            }
        }

        private async Task AddSittingPlayerToDbAsync(string tableId, string userName, AppDbContext appDbContext)
        {
            var table = _tables[new Guid(tableId)];
            int seatNumber = table.GetSeatOf(userName).Number;
            var tableModel = await appDbContext.Tables.FindAsync(tableId);
            tableModel.CreatedAt = DateTime.Now;
            tableModel.Status = GetStatus(table);
            var seatModel = await appDbContext.Seats.SingleAsync(seat => seat.TableId == new Guid(tableId) && seat.Number == seatNumber);

            await appDbContext.AddAsync(new Player
            {
                Id = Guid.NewGuid(),
                Seat = seatModel,
                Table = tableModel,
                User = await appDbContext.Users.FindAsync(userName)
            });
        }

        private async void StartNewDeal(PokerEngine.Table table, string tableId, AppDbContext appDbContext)
        {
            table.StartNewDeal();
            await AddNewDealAtDbAsync(tableId, table, appDbContext);
        }

        private async Task AddNewDealAtDbAsync(string tableId, PokerEngine.Table table, AppDbContext appDbContext)
        {
            var tableModel = await appDbContext.Tables.FindAsync(new Guid(tableId));
            await appDbContext.AddAsync(new Deal
            {
                Id = Guid.NewGuid(),
                LastRound = RoundType.Preflop.ToString(),
                PotSize = table.CurrentMaxBetSize,
                IsFinished = false,
                Table = tableModel,
                Players = tableModel.Players.Where(p => table.GetPlayer(p.User.UserName).IsInDeal)
            });
            
        }

        private string GetStatus(PokerEngine.Table table)
        {
            if(table.IsDeal)
            {
                return "Deal";
            }
            else
            {
                return "Wait";
            }
        }

        public async Task<List<TableModel>> GetTablesAsync(AppDbContext appDbContext)
        {
            var tables = await appDbContext.Tables.ToListAsync();
            return await Task.Run(() =>
            {
                return GetTableModels(tables);
            });
        }

        private List<TableModel> GetTableModels(IEnumerable<Table> tables)
        {
            List<TableModel> tableModels = new List<TableModel>();
            foreach (var table in tables)
            {
                tableModels.Add(new TableModel
                {
                    Id = table.Id,
                    BigBlindSize = table.BigBlindSize,
                    SmallBlindSize = table.SmallBlindSize,
                    MaxStartStack = table.MaxStartStack,
                    MinStartStack = table.MinStartStack,
                    MaxPlayersAmount = table.MaxPlayersAmount,
                    PlalyersAmount = table.Players != null ? table.Players.Count() : 0
                }) ;
            }
            return tableModels;
        }

        public async Task CreateTableAsync(TableModel model, AppDbContext appDbContext)
        {
            PokerEngine.Table table = new PokerEngine.Table(model.MaxPlayersAmount, model.MinStartStack, model.MaxStartStack, model.SmallBlindSize);
            _tables.Add(model.Id, table);
            Table tableDb = new Table
            {
                BigBlindSize = model.SmallBlindSize * 2,
                SmallBlindSize = model.SmallBlindSize,
                MaxPlayersAmount = model.MaxPlayersAmount,
                Status = "Wait",
                MinStartStack = model.MinStartStack,
                MaxStartStack = model.MaxStartStack,
            };
            await appDbContext.Tables.AddAsync(tableDb);
            for (int i = 0; i < model.MaxPlayersAmount; i++)
            {
                await appDbContext.Seats.AddAsync(new Seat
                {
                    Number = i,
                    TableId = model.Id,
                    Table = tableDb
                });
            }
            await appDbContext.SaveChangesAsync();
        }
    }
}
