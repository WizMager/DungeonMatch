using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;

namespace Game
{
    public class CellsSwapper
    {
        public Action<List<Cell>> OnSuccessSwap;
        private readonly List<Cell> _selectedCells = new();
        private readonly float _swapTime;
        private readonly CheckCells _checkCells;
        private bool _cellsSwap;

        public CellsSwapper(float swapTime, CheckCells checkCells)
        {
            _swapTime = swapTime;
            _checkCells = checkCells;
        }
        
        public void Swap(Cell newCell)
        {
            if (_cellsSwap) return;
            if (_selectedCells.Contains(newCell)) return;
            if (_selectedCells.Count > 0)
            {
                    if (SwapCellsAvailableCheck(_selectedCells[0], newCell))
                    {
                        _selectedCells.Add(newCell);
                            SwapCells(_selectedCells);
                            _selectedCells.Clear();
                    }
                    else
                    {
                            _selectedCells.Clear();
                            _selectedCells.Add(newCell);
                    }
            }
            else
            {
                    _selectedCells.Add(newCell);
            }
        }

        private bool SwapCellsAvailableCheck(Cell inListCell, Cell newCell)
        {
            var listX = inListCell.XNumber;
            var listY = inListCell.YNumber;
            var newX = newCell.XNumber;
            var newY = newCell.YNumber;
            if (listX == newX)
            {
                if (listY - 1 == newY || listY + 1 == newY)
                {
                    return true;
                }   
            }

            if (listY == newY)
            {
                if (listX - 1 == newX || listX + 1 == newX)
                {
                    return true;
                } 
            }
                
            return false;
        }

        private async void SwapCells(List<Cell> selectedCells)
        {
            var firstSelectedCell = selectedCells[0];
            var secondSelectedCell = selectedCells[1];
            await SwapCellsValues(firstSelectedCell, secondSelectedCell);
            var firstCellList = new List<Cell> {firstSelectedCell};
            var secondCellList = new List<Cell> {secondSelectedCell};
            var matchFirstCellTypeList = _checkCells.CheckCellsMatches(firstCellList);
            var matchSecondCellTypeList = _checkCells.CheckCellsMatches(secondCellList);
            var checkFirstMatchList =
                _checkCells.CheckHorizontalVertical(firstSelectedCell, matchFirstCellTypeList);
            var checkSecondMatchList =
                _checkCells.CheckHorizontalVertical(secondSelectedCell, matchSecondCellTypeList);
            if (checkFirstMatchList || checkSecondMatchList)
            {
                if (checkFirstMatchList && secondSelectedCell)
                {
                    var summMatch = new List<Cell>();
                    summMatch.AddRange(matchFirstCellTypeList);
                    summMatch.AddRange(matchSecondCellTypeList);
                    OnSuccessSwap?.Invoke(summMatch);
                }
                else
                {
                    OnSuccessSwap?.Invoke(checkFirstMatchList ? matchFirstCellTypeList : matchSecondCellTypeList);
                }
            }
            else
            {
                await SwapCellsValues(secondSelectedCell, firstSelectedCell);
            }
        }
        
        private async Task SwapCellsValues(Cell firstSelectedCell, Cell secondSelectedCell)
        {
            _cellsSwap = true;
            var cellOnePosition = firstSelectedCell.Image.transform.position;
            var cellTwoPosition = secondSelectedCell.Image.transform.position;
            var sequence = DOTween.Sequence();
            sequence.Join(firstSelectedCell.Image.transform.DOMove(cellTwoPosition, _swapTime));
            sequence.Join(secondSelectedCell.Image.transform.DOMove(cellOnePosition, _swapTime));
            await sequence.Play().AsyncWaitForCompletion();
            var imageOne = firstSelectedCell.Image;
            var imageTwo = secondSelectedCell.Image;
            var tempType = firstSelectedCell.CellType;
            firstSelectedCell.Image = imageTwo;
            secondSelectedCell.Image = imageOne;
            imageOne.transform.SetParent(secondSelectedCell.transform);
            imageTwo.transform.SetParent(firstSelectedCell.transform);
            firstSelectedCell.CellType = secondSelectedCell.CellType;
            secondSelectedCell.CellType = tempType;
            _cellsSwap = false;
        }
    }
}