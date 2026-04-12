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
    public class GameService
    {
        private readonly GideBdContext _db = DBService.Instance.Context;

        public ObservableCollection<Game> Games { get; set; } = new();

        public int Commit()=> _db.SaveChanges();
        public void Add(Game game)
        {
            var _game = new Game
            {
                Id = game.Id,
                Title = game.Title,
                AuthorId = game.AuthorId,
                Author = game.Author,
                BuildUrl = game.BuildUrl,
                Description = game.Description,
                FullProjectUrl = game.FullProjectUrl,
                Players = game.Players,
                NameExe = game.NameExe,
            };
            _db.Add(_game);
            Commit();
            Games.Add(_game);
        }

        public void GetAll()
        {
            var games = _db.Games
                           .Include(g => g.Author)
                           .Include(g => g.Players)
                           .ToList();
            Games.Clear();
            foreach (var game in games)
            {
                Games.Add(game);
            }
        }

        public GameService()
        {
            GetAll();
        }

        public void Remove(Game game)
        {
            _db.Remove(game);
            if (Commit() > 0)
                if (Games.Contains(game))
                    Games.Remove(game);
        }
    }
}
