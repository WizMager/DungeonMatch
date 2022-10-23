using System.Collections.Generic;

namespace Game
{
        public class CheckCells
        {
                private readonly CellsData _cellsData;

                public CheckCells(CellsData cellsData)
                {
                        _cellsData = cellsData;
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
                                var leftCell = _cellsData.CellsCoordinates[(x - 1, y)];
                                if (leftCell.CellType == type)
                                {
                                        if (!resultList.Contains(leftCell))
                                        {
                                                neighborhoodCells.Add(leftCell);       
                                        }
                                }
                        }
                        
                        if (x < _cellsData.RowsCellsCount[0] - 1)
                        {
                                var rightCell = _cellsData.CellsCoordinates[(x + 1, y)];
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
                                var upCell = _cellsData.CellsCoordinates[(x, y - 1)];
                                if (upCell.CellType == type)
                                {
                                        if (!resultList.Contains(upCell))
                                        {
                                                neighborhoodCells.Add(upCell);       
                                        }
                                }
                        }
                        
                        if (y < _cellsData.RowsCellsCount.Length - 1)
                        {
                                var downCell = _cellsData.CellsCoordinates[(x, y + 1)];
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

                public bool CheckPopAvailable()
                {
                        for (int y = 0; y < _cellsData.RowsCellsCount.Length; y++)
                        {
                                for (int x = 0; x < _cellsData.RowsCellsCount[y]; x++)
                                {
                                        if (CheckCellMatchPop(_cellsData.GetCell((x, y))) != null)
                                        {
                                                return true;
                                        }
                                }
                        }

                        return false;
                }

                public List<Cell> CheckCellMatchPop(Cell cell)
                {
                        var checkCell = new List<Cell> {cell};
                        var matchList = new List<Cell>();
                        matchList = CheckCellsMatches(checkCell, matchList);

                        return CheckHorizontalVertical(cell, matchList) ? matchList : null;
                }
        }
}