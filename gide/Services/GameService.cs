using gide.DTOs;
using gide.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace gide.Service
{
    public class GameService
    {
        private const string BaseUrl = "https://localhost:7295/api/games";
        private static readonly HttpClient _client = new HttpClient();

        public ObservableCollection<Game> Games { get; set; } = new();

        public async Task AddAsync(Game game)
        {

            var dto = new CreateGameDto //маппинг из createdgamedto в game
            {
                Title = game.Title,
                NameExe = game.NameExe,
                FullProjectUrl = game.FullProjectUrl,
                Description = game.Description,
                AuthorId = game.AuthorId,
                BuildUrl = game.BuildUrl
            };


            var json = JsonSerializer.Serialize(dto);

            var content = new StringContent(json, Encoding.UTF8, "application/json");//для корректного чтения json'ом

            var response = await _client.PostAsync(BaseUrl, content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync(); //получаем полностью сгенерированную игру
            var createdGame = JsonSerializer.Deserialize<GameDto>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (createdGame == null)
                throw new Exception("Ошибка десериализации");

            Games.Add(GameDtoMapping(createdGame));
        }

        public async Task GetAllAsync()
        {
            var json = await _client.GetStringAsync(BaseUrl);

            var dtos = JsonSerializer.Deserialize<List<GameDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true //помогает подогнать класс из json (title -> Title)
            });


            if (dtos == null)
                throw new Exception("Ошибка десериализации");

            Games.Clear();
                
            foreach (var dto in dtos)
            {
                Games.Add(GameDtoMapping(dto));
            }
        }

        public GameService()
        {
        }

        public async Task DeleteAsync(Game game)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{game.Id}");

            response.EnsureSuccessStatusCode();

            var existing = Games.FirstOrDefault(g => g.Id == game.Id);
            if (existing != null)
                Games.Remove(game);
        }

        private Game GameDtoMapping(GameDto dto) //маппинг из gamedto в game
        {
            return new Game
            {
                Id = dto.Id,
                Description = dto.Description,
                BuildUrl = dto.BuildUrl,
                FullProjectUrl = dto.FullProjectUrl,
                NameExe = dto.NameExe,
                Title = dto.Title,
                AuthorName = dto.AuthorName,
            };
        }
    }
}
