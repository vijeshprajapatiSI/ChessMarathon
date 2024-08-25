namespace ChessAPI.Model
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Country {  get; set; }
        public int CurrentWorldRanking {  get; set; }
        public int TotalMatchesPlayed {  get; set; }
    }
}
