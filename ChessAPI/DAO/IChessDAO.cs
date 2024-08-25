using ChessAPI.Model;
namespace ChessAPI.DAO

{
    public interface IChessDAO
    {
        Task<int> AddMatch(Match match);
        Task<List<Player>> GetPlayersByCountrySorted(string country, string sortBy);

        Task<List<Performance>> GetPlayersPerformance();

        Task<List<PlayerWinPercentage>> GetPlayerWinPercentageByAverageOfWins();
    }
}
