using TMPro;
using Utils;

namespace Game
{
    public class ScoreCounter
    {
        private readonly TMP_Text _scoreText;
        private readonly CellValue[] _cellsValues;
        private int _score;
        
        public ScoreCounter(TMP_Text scoreText, CellValue[] cellsValues)
        {
            _scoreText = scoreText;
            _cellsValues = cellsValues;
        }

        public void ChangeScore(CellType cellType, int cellsCount)
        {
            var scoreCounter = 0;
            foreach (var cellValue in _cellsValues)
            {
                if(cellType != cellValue.cellType) continue;
                scoreCounter += cellValue.score * cellsCount;
                break;
            }
            
            _score += scoreCounter;
            _scoreText.text = $"Score: {_score}";
        }
    }
}