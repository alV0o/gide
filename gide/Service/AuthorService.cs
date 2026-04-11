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
    public class AuthorService
    {
        private readonly GideBdContext _db = DBService.Instance.Context;

        public ObservableCollection<Author> Authors { get; set; } = new();

        public int Commit() => _db.SaveChanges();
        public void Add(Author author)
        {
            var _author = new Author
            {
                Id = author.Id,
                Username = author.Username,
                Password = author.Password,
                Games = author.Games,
            };
            _db.Add(_author);
            Commit();
            Authors.Add(_author);
        }
        public void GetAll()
        {
            var authors = _db.Authors
                             .Include(a => a.Games)
                             .ToList();
            Authors.Clear();
            foreach (var author in authors)
            {
                Authors.Add(author);
            }
        }

        public AuthorService()
        {
            GetAll();
        }

        public void Remove(Author author)
        {
            _db.Remove(author);
            if (Commit() > 0)
                if (Authors.Contains(author))
                    Authors.Remove(author);
        }
    }
}
