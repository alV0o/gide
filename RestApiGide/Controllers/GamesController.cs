using RestApiGide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiGide.DTOs;

namespace RestApiGide.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly GideBdContext _context;

        public GamesController(GideBdContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Game>>> Get()
        {
            var games =  await _context.Games
                .Include(g => g.Author)
                .Select(g => new GameDto
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    BuildUrl = g.BuildUrl,
                    FullProjectUrl = g.FullProjectUrl,
                    NameExe = g.NameExe,
                    AuthorName = g.Author.Username,
                })
                .ToListAsync();

            return Ok(games); //возвращает код статуса после запроса
        }

        [HttpPost]
        public async Task<ActionResult<Game>> Add(CreateGameDto dto) //dto нужны для того чтобы не было зацикливания и сохранялось только нужное
        {
            try
            {
                var game = new Game
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    BuildUrl = dto.BuildUrl,
                    FullProjectUrl = dto.FullProjectUrl,
                    NameExe = dto.NameExe,
                    AuthorId = dto.AuthorId,
                };

                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                return Ok(game);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
                return NotFound("Игра не найдена");

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return Ok("Удалено");
        }

    }
}
