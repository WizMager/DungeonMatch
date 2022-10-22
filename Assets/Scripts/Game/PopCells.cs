namespace Game
{
    public class PopCells
    {
        
        
        public async void Pop()
                {
                        // for (int y = 0; y < _rowsCellsLength.Length; y++)
                        // {
                        //         for (int x = 0; x < _rowsCellsLength[y]; x++)
                        //         {
                        //                 var checkCell = new List<Cell> {_cellsCoord[(x, y)]};
                        //                 var matchList = new List<Cell>();
                        //                 matchList = CheckCellsMatches(checkCell, matchList);
                        //
                        //                 if (!CheckHorizontalVertical(_cellsCoord[(x, y)], matchList)) continue;
                        //                 var scoreCounter = 0;
                        //                 var scoreCell = matchList[0].CellType;
                        //                 var minimizeCells = DOTween.Sequence();
                        //                 foreach (var matchCell in matchList)
                        //                 {
                        //                         scoreCounter += scoreCell;
                        //                         minimizeCells.Join(matchCell.gameObject.transform.DOScale(Vector3.zero, cellsPopTime));
                        //                 }
                        //
                        //                 await minimizeCells.Play().AsyncWaitForCompletion();
                        //                 _score += scoreCounter;
                        //                 scoreText.text = $"Score: {_score}";
                        //                 var maximizeCells = DOTween.Sequence();
                        //                 foreach (var cell in matchList)
                        //                 {
                        //                         var randomType = Random.Range(1, cellSprites.Length);
                        //                         cell.ChangeSprite = cellSprites[randomType];
                        //                         cell.CellType = randomType;
                        //                         maximizeCells.Join(cell.transform.DOScale(Vector3.one, cellsPopTime));
                        //                 }
                        //
                        //                 await maximizeCells.Play().AsyncWaitForCompletion();
                        //                 _cellsMove = false;
                        //         }  
                        // }
                }
    }
}