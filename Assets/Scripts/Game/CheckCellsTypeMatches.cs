using System.Collections.Generic;
using UnityEngine;

namespace Game
{
        public class CheckCellsTypeMatches
        {
                private readonly int[] _rowsCells;
                private readonly Dictionary<(int x, int y), Cell> _cellsCoordinate;

                public CheckCellsTypeMatches(CellsModel cellsModel)
                {
                        _rowsCells = cellsModel.RowsCellsCount;
                        _cellsCoordinate = cellsModel.CellsCoordinates;
                }

                public List<Cell> CheckCellsMatches(List<Cell> neighborhoodCellsList, List<Cell> previousResultList = null)
                {
                        var result = previousResultList ?? new List<Cell> {neighborhoodCellsList[0]};
                        if (neighborhoodCellsList.Count <= 0) return result;
                        foreach (var cell in neighborhoodCellsList)
                        {
                                if (!result.Contains(cell))
                                {
                                        result.Add(cell);
                                }

                                CheckCellsMatches(CheckNeighborhoodMatchCells(cell, result), result);
                        }

                        return result;
                }

                private List<Cell> CheckNeighborhoodMatchCells(Cell cell, List<Cell> resultList)
                {
                        var type = cell.CellType;
                        var x = cell.XNumber;
                        var y = cell.YNumber;
                        var neighborhoodCells = new List<Cell>();
                        if (x > 0)
                        {
                                var leftCell = _cellsCoordinate[(x - 1, y)];
                                if (leftCell.CellType == type)
                                {
                                        if (!resultList.Contains(leftCell))
                                        {
                                                neighborhoodCells.Add(leftCell);       
                                        }
                                }
                        }
                        
                        if (x < _rowsCells[0] - 1)
                        {
                                var rightCell = _cellsCoordinate[(x + 1, y)];
                                if (rightCell.CellType == type)
                                {
                                        if (!resultList.Contains(rightCell))
                                        {
                                                neighborhoodCells.Add(rightCell);       
                                        }
                                }     
                        }
                        
                        if (y > 0)
                        {
                                var upCell = _cellsCoordinate[(x, y - 1)];
                                if (upCell.CellType == type)
                                {
                                        if (!resultList.Contains(upCell))
                                        {
                                                neighborhoodCells.Add(upCell);       
                                        }
                                }
                        }
                        
                        if (y < _rowsCells.Length - 1)
                        {
                                var downCell = _cellsCoordinate[(x, y + 1)];
                                if (downCell.CellType == type)
                                {
                                        if (!resultList.Contains(downCell))
                                        {
                                                neighborhoodCells.Add(downCell);      
                                        }
                                } 
                        }

                        return neighborhoodCells;
                }

                public bool CheckHorizontalVertical(Cell checkCell, List<Cell> matchCells)
                {
                        var horizontalLineCells = 0;
                        var verticalLineCells = 0;
                        var y = checkCell.YNumber;
                        var x = checkCell.XNumber;
                        foreach (var cell in matchCells)
                        {
                                if (y == cell.YNumber)
                                {
                                        horizontalLineCells++;
                                }

                                if (x == cell.XNumber)
                                {
                                        verticalLineCells++;
                                }
                        }
                        return horizontalLineCells >= 3 || verticalLineCells >= 3;
                }
        }
}