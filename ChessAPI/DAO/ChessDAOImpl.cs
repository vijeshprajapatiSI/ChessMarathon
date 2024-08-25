using Npgsql;
using ChessAPI.Model;
using System.Data;
using System.Data.Common;
namespace ChessAPI.DAO
{
    public class ChessDAOImpl : IChessDAO
    {
        private readonly NpgsqlConnection _connection;
        public ChessDAOImpl(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> AddMatch(Match match)
        {
            int rowsInserted = 0;
            string message;

            string query = @$"INSERT INTO chess.matches (player1_id, player2_id, match_date, match_level, winner_id) VALUES (
                              {match.Player1Id}, {match.Player2Id}, '{match.MatchDate}', '{match.MatchLevel}', {match.WinnerId})";

            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand insertCommand = new NpgsqlCommand(query, _connection);
                    insertCommand.CommandType = CommandType.Text;
                    rowsInserted = await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch(NpgsqlException e)
            {
                message = e.Message;
                Console.WriteLine("-------------Exception Occured-----------" + message);
            }
            return rowsInserted;
        }

        public async Task<List<Player>> GetPlayersByCountrySorted(string country, string sortBy)
        {
            List<Player> players = new List<Player>();
            string query = @$"SELECT * FROM chess.players WHERE country = '{country}' ORDER BY {sortBy} ASC";
            try
            {
                Player player = null;
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            player = new Player();
                            player.Id = reader.GetInt32(0);
                            player.FirstName = reader["first_name"].ToString();
                            player.LastName = reader["last_name"].ToString();
                            player.Country = reader["country"].ToString();
                            player.CurrentWorldRanking = reader.GetInt32(4);
                            player.TotalMatchesPlayed = reader.GetInt32(5);
                            players.Add(player);
                        }
                    }
                    reader?.Close();
                }
            }
            catch(NpgsqlException e)
            {
                Console.WriteLine("------------Exception Occured------------" + e.Message);
            }
            return players;
        }

        public async Task<List<Performance>> GetPlayersPerformance()
        {
            List<Performance> players = new List<Performance>();
            string query = "SELECT p.player_id, CONCAT(p.first_name, ' ', p.last_name) AS full_name, " +
                "p.total_matches_played, COUNT(winner_id) AS matches_won " +
                "FROM chess.players p LEFT JOIN chess.matches m ON p.player_id = m.winner_id " +
                "GROUP BY p.player_id ORDER BY matches_won DESC;";

            try
            {
                Performance p = null;
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            p = new Performance();
                            p.Id = reader.GetInt32(0);
                            p.FullName = reader["full_name"].ToString();
                            p.TotalMatchesPlayed = reader.GetInt32(2);
                            p.MatchesWon = reader.GetInt32(3);
                            p.WinPercentage = (p.MatchesWon / p.TotalMatchesPlayed) * 100;
                            players.Add(p);
                        }
                    }
                    reader?.Close();
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine("------------Exception Occured------------" + e.Message);
            }
            return players;
        }

        public async Task<List<PlayerWinPercentage>> GetPlayerWinPercentageByAverageOfWins()
        {
            string query = $"WITH PlayerWins AS (SELECT winner_id AS player_id,COUNT(*) AS wins FROM chess.Matches WHERE winner_id IS NOT NULL GROUP BY winner_id),AverageWins AS (SELECT AVG(wins) AS avg_wins FROM PlayerWins),PlayerStats AS (SELECT p.player_id,p.first_name || ' ' || p.last_name AS full_name,COALESCE(pw.wins, 0) AS wins,(COALESCE(pw.wins, 0) * 100.0 / p.total_matches_played) AS win_percentage FROM chess.Players p LEFT JOIN PlayerWins pw ON p.player_id = pw.player_id)SELECT ps.full_name as full_name,ps.wins as total_wins,ps.win_percentage as win_percentage FROM PlayerStats ps, AverageWins aw WHERE ps.wins > aw.avg_wins;";
            List<PlayerWinPercentage> playerList = new List<PlayerWinPercentage>();
            PlayerWinPercentage? p = null;

            try
            {
                using (_connection)
                {
                    await _connection.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            p = new PlayerWinPercentage();
                            p.FullName = reader.GetString(0);
                            p.TotalMatchesWon = reader.GetInt32(1);
                            p.WinPercentage = reader.GetDecimal(2);

                            playerList.Add(p);
                        }
                    }

                    reader?.Close();
                }
            }
            catch (NpgsqlException e)
            {
                Console.WriteLine(e.Message);
            }

            return playerList;
        }
    }
}
