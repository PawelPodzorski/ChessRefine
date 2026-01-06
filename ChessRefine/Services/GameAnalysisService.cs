using Chess;
using ChessRefine.Models;
using System.Numerics;

namespace ChessRefine.Services
{
    public class GameAnalysisProgress
    {
        public int TotalMoves { get; set; }
        public int MovesAnalyzed { get; set; }
    }

    public class GameAnalysisService
    {
        private readonly StockfishService _engine;
        public GameAnalysisService(StockfishService engine) 
        {
            _engine = engine;
        }

        public async Task<List<MoveAnalysis>> AnalyzeGame(
            string spgn,
            int depth,
            Action<GameAnalysisProgress>? progressCallback = null)
        {
            var game = ChessBoard.LoadFromPgn(spgn);
            var board = new ChessBoard();

            var result = new List<MoveAnalysis>();
            int moveNumber = 0;
            int movesMade = 0;

            // number of moves in whole game
            int totalMoves = 0;
            foreach (var m in game.ExecutedMoves)
            {
                totalMoves++;
            }

            foreach (var move in game.ExecutedMoves)
            {
                board.Move(move.San);

                double eval = await _engine.EvaluateFen(board.ToFen(), depth);
                bool iswhite = false;
                
                // If black on move
                if (movesMade % 2 == 0)
                {
                    eval = -eval;
                    moveNumber++;
                    iswhite = true;
                }

                result.Add(new MoveAnalysis
                {
                    MoveNumber = moveNumber,
                    Move = move.San,
                    Fen = board.ToFen(),
                    Evaluation = eval,
                    IsWhiteMove = iswhite
                });

                // Progress for progressBar
                movesMade++;
                progressCallback?.Invoke(new GameAnalysisProgress
                {
                    TotalMoves = totalMoves,
                    MovesAnalyzed = movesMade
                });
            }
            return result;
        }
    }
}
