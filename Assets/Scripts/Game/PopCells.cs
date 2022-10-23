using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game
{
        public class PopCells
        {
                public Action<bool> OnPopCells;
                private readonly CellsData _cellsData;
                private readonly CheckCells _checkCells;
                private readonly ScoreCounter _scoreCounter;
                private readonly float _cellsPopTime;
                
                public PopCells(CellsData cellsData, CheckCells checkCells, ScoreCounter scoreCounter, float cellsPopTime)
                {
                        _cellsData = cellsData;
                        _checkCells = checkCells;
                        _scoreCounter = scoreCounter;
                        _cellsPopTime = cellsPopTime;
                }

                public async void PopAll()
                {
                        OnPopCells?.Invoke(true);
                        await PopBeforeAvailable();
                        OnPopCells?.Invoke(false);
                }

                private async Task PopBeforeAvailable()
                {
                        if (!_checkCells.CheckPopAvailable()) return;

                        for (int y = 0; y < _cellsData.RowsCellsCount.Length; y++)
                        {
                                for (int x = 0; x < _cellsData.RowsCellsCount[y]; x++)
                                {
                                        await PopCellMatches(_cellsData.GetCell((x, y)));
                                }
                        }  
                        PopBeforeAvailable();
                }
                
                private async Task PopCellMatches(Cell cell)
                {
                        var matchList = _checkCells.CheckCellMatchPop(cell);
                        if (matchList == null) return;
                                        
                        var minimizeCells = DOTween.Sequence();
                        foreach (var matchCell in matchList)
                        {
                                minimizeCells.Join(
                                        matchCell.gameObject.transform.DOScale(Vector3.zero,
                                                _cellsPopTime));
                        }
                        await minimizeCells.Play().AsyncWaitForCompletion();
                                        
                        _scoreCounter.ChangeScore(cell.CellType, matchList.Count);
                        _cellsData.ChangeCellsSprite(matchList);
                                        
                        var maximizeCells = DOTween.Sequence();
                        foreach (var matchCell in matchList)
                        {
                                maximizeCells.Join(matchCell.transform.DOScale(Vector3.one, _cellsPopTime));
                        }
                        await maximizeCells.Play().AsyncWaitForCompletion();   
                }
        }
}