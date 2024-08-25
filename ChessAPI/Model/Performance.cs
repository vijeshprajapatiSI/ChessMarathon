namespace ChessAPI.Model
{
    public class Performance
    {
        public int Id {  get; set; }
        public string FullName { get; set; }
        public int TotalMatchesPlayed {  get; set; }
        public int MatchesWon {  get; set; }
        public double WinPercentage {  get; set; }

    }
}
