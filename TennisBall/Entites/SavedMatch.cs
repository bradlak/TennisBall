using SQLite;

namespace TennisBall.Entites
{
    [Table("SavedMatches")]
    public class SavedMatch : BaseEntity
    {
        public string Player1Sets { get; set; }

        public string Player2Sets { get; set; }

        public string Player1Total { get; set; }

        public string Player2Total { get; set; }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }

        public string WinnerName { get; set; }
    }
}