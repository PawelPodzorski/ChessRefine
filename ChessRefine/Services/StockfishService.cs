using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ChessRefine.Services
{
    public class StockfishService
    {
        private readonly Process _process;
        private readonly StreamWriter _input;
        private readonly StreamReader _output;
    
        public StockfishService(string path)
        {
            _process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            _process.Start();
            _input = _process.StandardInput;
            _output = _process.StandardOutput;

            Send("uci");
            WaitFor("uciok");

            Send("isready");
            WaitFor("readyok");
        }

        public async Task<double> EvaluateFen(string fen, int depth)
        {
            Send($"position fen {fen}");
            Send($"go depth {depth}");

            double lastEval = 0;

            string? line;
            while ((line = await _output.ReadLineAsync()) != null)
            {
                if (line.StartsWith("info") && line.Contains("score cp"))
                {
                    var match = Regex.Match(line, @"score cp (-?\d+)");
                    if (match.Success)
                    {
                        lastEval = int.Parse(match.Groups[1].Value) / 100.0;
                    }
                }

                if (line.StartsWith("bestmove")) break;
            }

            return lastEval;
        }


        private void Send(string cmd)
        {
            _input.WriteLine(cmd);
            _input.Flush();
        }

        private void WaitFor(string expected)
        {
            string? line;
            while (( line = _output.ReadLine()) != null)
            {
                if (line.Contains(expected)) break;
            }
        }

        private void Dispose()
        {
            Send("quit");
            _process.Dispose();
        }
    }
}
