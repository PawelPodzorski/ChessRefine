function updateEvalBar(evalValue) {
    const bar = document.getElementById("evalBar");
    const topLabel = document.getElementById("evalTopLabel");
    const bottomLabel = document.getElementById("evalBottomLabel");
    const containerHeight = document.getElementById("evalContainer").clientHeight;

    const maxEval = 5;
    let clampedEval = Math.max(-maxEval, Math.min(maxEval, evalValue));

    const halfHeight = containerHeight / 2;

    const spacing = 4;

    if (clampedEval >= 0) {
        // White adventage
        const barHeight = (clampedEval / maxEval) * halfHeight;
        bar.style.background = "green";
        bar.style.bottom = halfHeight + "px";
        bar.style.height = barHeight + "px";

        // Bottom eval text
        bottomLabel.style.display = "block";
        bottomLabel.textContent = clampedEval.toFixed(2);
        bottomLabel.style.bottom = (halfHeight - 16 - spacing) + "px";

        topLabel.style.display = "none";
    } else {
        // Black adventage
        const barHeight = (-clampedEval / maxEval) * halfHeight;
        bar.style.background = "red";
        bar.style.bottom = halfHeight + (clampedEval / maxEval) * halfHeight + "px";
        bar.style.height = barHeight + "px";

        // Top eval text
        topLabel.style.display = "block";
        topLabel.textContent = clampedEval.toFixed(2);
        topLabel.style.bottom = (halfHeight + spacing) + "px";

        bottomLabel.style.display = "none";
    }
}



document.addEventListener("DOMContentLoaded", function () {
    if (typeof moves === "undefined") return;

    let index = 0;

    const board = Chessboard('board', {
        position: moves[0].Fen,
        pieceTheme: '/img/chesspieces/wikipedia/{piece}.png'
    });

    const whiteMoves = document.getElementById("whiteMoves");
    const blackMoves = document.getElementById("blackMoves");

    // Generate scoresheet
    moves.forEach((m, i) => {
        const li = document.createElement("li");
        li.textContent = m.Move;
        li.dataset.index = i;
        li.style.cursor = "pointer";
        li.style.padding = "2px 5px";

        li.addEventListener("click", () => {
            index = parseInt(li.dataset.index);
            updateBoard();
        });

        if (m.IsWhiteMove) {
            whiteMoves.appendChild(li);
        } else {
            blackMoves.appendChild(li);
        }
    });

    function updateBoard() {
        board.position(moves[index].Fen);

        // Lighten move on scoresheet
        [whiteMoves, blackMoves].forEach(ul => {
            Array.from(ul.children).forEach(li => {
                li.style.backgroundColor = (parseInt(li.dataset.index) === index) ? "#d0f0ff" : "transparent";
            });
        });

        updateEvalBar(moves[index].Evaluation);
    }

    // scrolling moves function
    window.nextMove = function () {
        if (index < moves.length - 1) {
            index++;
            updateBoard();
        }
    };
    window.prevMove = function () {
        if (index > 0) {
            index--;
            updateBoard();
        }
    };
    window.firstMove = function () {
        index = 0;
        updateBoard();
    };
    window.lastMove = function () {
        index = moves.length - 1;
        updateBoard();
    };

    // keyboardtroller scrolling
    document.addEventListener("keydown", function (e) {
        if (e.key === "ArrowRight") {
            nextMove();
        } else if (e.key === "ArrowLeft") {
            prevMove();
        }
    });

    updateBoard();
});
