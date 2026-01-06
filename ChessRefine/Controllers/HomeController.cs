using ChessRefine.Models;
using ChessRefine.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace ChessRefine.Controllers
{
    public class HomeController : Controller
    {
        private readonly GameAnalysisService _analysis;

        public HomeController(GameAnalysisService analysis)
        {
            _analysis = analysis;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AnalysisRequest());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AnalyzeTest()
        {
            return Content("POST działa!");
        }


        // POST: /Home/Analyze
        [HttpPost]
        public async Task<IActionResult> Analyze(AnalysisRequest request)
        {
            // Walidacja pustego PGN
            if (string.IsNullOrWhiteSpace(request.Pgn))
            {
                TempData["Error"] = "PGN cannot be empty.";
                return RedirectToAction("Index");
            }

            // Podstawowa walidacja PGN
            var movePattern = @"[KQBNR]?[a-h]?[1-8]?x?[a-h][1-8](=[QBNR])?|O-O(-O)?";
            if (!Regex.IsMatch(request.Pgn, movePattern))
            {
                TempData["Error"] = "Invalid PGN format. Make sure it contains at least one move.";
                return RedirectToAction("Index");
            }

            // Jeśli PGN poprawny -> analiza
            var result = await _analysis.AnalyzeGame(request.Pgn, request.Depth);

            return View("Result", result);
        }
    }
}
