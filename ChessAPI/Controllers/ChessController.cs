using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChessAPI.Model;
using ChessAPI.DAO;

namespace ChessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChessController : ControllerBase
    {
        private readonly IChessDAO _chessDAOImpl;

        public ChessController(IChessDAO chessDAOImpl)
        {
            _chessDAOImpl = chessDAOImpl;
        }

        [HttpPost("AddNewMatch", Name = "AddMatch")]
        public async Task<ActionResult<int>> AddMatch(Match match)
        {
            if (match == null)
            {
                return BadRequest();
            }
            if(ModelState.IsValid)
            {
                var value = await _chessDAOImpl.AddMatch(match);
                return Ok(value);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetPlayersOfCountryByColumn", Name = "GetPlayersSorted")]
        public async Task<ActionResult<List<Player>>> GetPlayersByCountrySorted(string country, string sortBy)
        {
            var playerList = await _chessDAOImpl.GetPlayersByCountrySorted(country, sortBy);
            return Ok(playerList);
        }

        [HttpGet("playerperformance", Name = "PlayersPerformance")]
        public async Task<ActionResult<List<Performance>>> GetPlayersPerformance()
        {
            var playerList = await _chessDAOImpl.GetPlayersPerformance();
            return Ok(playerList);
        }

        [HttpGet("GetPlayerWinPercentageByAverageOfWins", Name = "GetPlayerWinPercentageByAverageOfWins")]
        public async Task<ActionResult<List<PlayerWinPercentage>>> GetPlayerWinPercentageByAverageOfWins()
        {
            var players = await _chessDAOImpl.GetPlayerWinPercentageByAverageOfWins();
            if (players == null)
            {
                return NotFound();
            }

            return Ok(players);
        }

    }
}
