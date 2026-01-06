namespace ChessRefine.Models
{
    public class MoveAnalysis
    {
        public int MoveNumber { get; set; }
        public string Move { get; set; } = string.Empty;
        public string Fen { get; set; } = string.Empty;
        public double Evaluation { get; set; }
        public bool IsWhiteMove { get; set; }
    }
}
