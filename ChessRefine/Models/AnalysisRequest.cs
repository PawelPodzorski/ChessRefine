namespace ChessRefine.Models
{
    public class AnalysisRequest
    {
        public string Pgn { get; set; } = string.Empty;
        public int Depth { get; set; } = 18;
    }
}
