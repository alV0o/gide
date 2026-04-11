using gide.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gide.Service
{
    public class PlayerService
    {
        private readonly GideBdContext _db = DBService.Instance.Context;

        public ObservableCollection<Player> Players { get; set; } = new();

        public int Commit() => _db.SaveChanges();
        public void Add(Player player)
        {
            var _player = new Player
            {
                Id = player.Id,
                Username = player.Username,
                Password = player.Password,
                Games = player.Games
            };
            _db.Add(_player);
            Commit();
            Players.Add(_player);
        }

        public void GetAll()
        {
            var players = _db.Players
                             .Include(p=>p.Games)
                             .ToList();
            Players.Clear();
            foreach (var player in players)
            {
                Players.Add(player);
            }
        }

        public PlayerService()
        {
            GetAll();
        }

        public void Remove(Player player)
        {
            _db.Remove(player);
            if (Commit() > 0)
                if (Players.Contains(player))
                    Players.Remove(player);
        }
    }
}
