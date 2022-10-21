using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellsController : MonoBehaviour
{
        [SerializeField] private RowsContainerComponent rowsContainerComponent;
        [SerializeField] private Sprite[] cellSprites;
        [SerializeField] private float cellMoveTime;
        [SerializeField] private float cellsPopTime;
        private readonly Dictionary<int, Cell> _cells = new();
        private readonly Dictionary<(int x, int y), Cell> _cellsCoord = new ();
        private readonly List<Cell> _selectedCells = new();
        private bool _cellsMove;
        private int[] _rowsCellsLength;
        private List<Cell> _popCells = new();

        private void Start()
        {
              Initialization(rowsContainerComponent);
        }

        private void Initialization(RowsContainerComponent rowsContainerValue)
        {
                var rowsContainer = rowsContainerValue.GetRowsContainers;
                _rowsCellsLength = new int[rowsContainer.Length];
                for (int y = 0; y < rowsContainer.Length; y++)
                {
                        var rowCells = rowsContainer[y].GetRowCells;
                        _rowsCellsLength[y] = rowCells.Length;
                        for (int x = 0; x < rowCells.Length; x++)
                        {
                                var cell = rowCells[x].GetComponent<Cell>();
                                cell.XNumber = x;
                                cell.YNumber = y;
                                var id = x + y * rowCells.Length;
                                var randomType = Random.Range(1, cellSprites.Length);
                                cell.SetImage = cellSprites[randomType];
                                cell.CellType = randomType switch
                                {
                                        1 => CellType.First,
                                        2 => CellType.Second,
                                        3 => CellType.Third,
                                        4 => CellType.Fourth,
                                        5 => CellType.Fifth,
                                        _ => cell.CellType
                                };
                                cell.Id = id;
                                cell.OnCellClicked += OnCellClickedHandler;
                                _cells.Add(id, cell);
                                _cellsCoord.Add((x, y), cell);
                        }
                }
                PopCells();
        }

        private void OnCellClickedHandler(int id)
        {
                if (_cellsMove) return;
                var selectedCell = _cells[id];
                if (_selectedCells.Contains(selectedCell)) return;
                if (_selectedCells.Count > 0)
                {
                        if (SwapCellsAvailableCheck(_selectedCells[0], selectedCell))
                        {
                                _selectedCells.Add(selectedCell);
                                _cellsMove = true;
                                SwapCells(_selectedCells);
                        }
                        else
                        {
                                _selectedCells.Clear();
                                _selectedCells.Add(selectedCell);
                        }
                }
                else
                {
                        _selectedCells.Add(selectedCell);
                }
        }

        #region SwapCells

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
                var cellOnePosition = selectedCells[0].Image.transform.position;
                var cellTwoPosition = selectedCells[1].Image.transform.position;
                var sequence = DOTween.Sequence();
                sequence.Join(selectedCells[0].Image.transform.DOMove(cellTwoPosition, cellMoveTime));
                sequence.Join(selectedCells[1].Image.transform.DOMove(cellOnePosition, cellMoveTime));
                await sequence.Play().AsyncWaitForCompletion();
                var imageOne = _selectedCells[0].Image;
                var imageTwo = _selectedCells[1].Image;
                var tempType = _selectedCells[0].CellType;
                _selectedCells[0].Image = imageTwo;
                _selectedCells[1].Image = imageOne;
                imageOne.transform.SetParent(_selectedCells[1].transform);
                imageTwo.transform.SetParent(_selectedCells[0].transform);
                _selectedCells[0].CellType = _selectedCells[1].CellType;
                _selectedCells[1].CellType = tempType;
                var firstCellList = new List<Cell> {_selectedCells[0]};
                var secondCellList = new List<Cell> {_selectedCells[1]};
                if (CheckCellsNeighborhoodMatches(secondCellList, new List<Cell>()).Count < 3 
                    || CheckCellsNeighborhoodMatches(firstCellList, new List<Cell>()).Count < 3)
                {
                       Swap(_selectedCells);
                       _cellsMove = false;
                }
                else
                {
                        OnCompleteSwapHandler();     
                }
        }

        private async void Swap(List<Cell> selectedCells)
        {
                var cellOnePosition = selectedCells[0].Image.transform.position;
                var cellTwoPosition = selectedCells[1].Image.transform.position;
                var sequence = DOTween.Sequence();
                sequence.Join(selectedCells[0].Image.transform.DOMove(cellTwoPosition, cellMoveTime));
                sequence.Join(selectedCells[1].Image.transform.DOMove(cellOnePosition, cellMoveTime));
                await sequence.Play().AsyncWaitForCompletion();
                var imageOne = _selectedCells[0].Image;
                var imageTwo = _selectedCells[1].Image;
                var tempType = _selectedCells[0].CellType;
                _selectedCells[0].Image = imageTwo;
                _selectedCells[1].Image = imageOne;
                imageOne.transform.SetParent(_selectedCells[1].transform);
                imageTwo.transform.SetParent(_selectedCells[0].transform);
                _selectedCells[0].CellType = _selectedCells[1].CellType;
                _selectedCells[1].CellType = tempType;    
        }

        private void OnCompleteSwapHandler()
        {
                PopCells();
                //var cellList = new List<Cell> {_selectedCells[1]};
                //_popCells = CheckCellsNeighborhoodMatches(cellList, null);
                //PopMatchCells(_popCells);
                //_cellsMove = false;
        }

        #endregion

        #region CheckMatches

        private List<Cell> CheckCellsNeighborhoodMatches(List<Cell> neigh, List<Cell> cellsList)
        {
                var result = cellsList ?? new List<Cell> {neigh[0]};
                if (neigh.Count <= 0) return result;
                foreach (var cell in neigh)
                {
                        if (!result.Contains(cell))
                        {
                           result.Add(cell);     
                        }
                        CheckCellsNeighborhoodMatches(CheckNeighborhoodCells(cell, result), result);
                }

                return result;
        }
        
        private List<Cell> CheckNeighborhoodCells(Cell cell, List<Cell> resultList)
        {
                var type = cell.CellType;
                var x = cell.XNumber;
                var y = cell.YNumber;
                var neighborhoodCells = new List<Cell>();
                if (x > 0)
                {
                        var leftCell = _cellsCoord[(x - 1, y)];
                        if (leftCell.CellType == type)
                        {
                                if (!resultList.Contains(leftCell))
                                {
                                        neighborhoodCells.Add(leftCell);       
                                }
                        }
                }

                if (x < _rowsCellsLength[0] - 1)
                {
                        var rightCell = _cellsCoord[(x + 1, y)];
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
                        var upCell = _cellsCoord[(x, y - 1)];
                        if (upCell.CellType == type)
                        {
                                if (!resultList.Contains(upCell))
                                {
                                        neighborhoodCells.Add(upCell);       
                                }
                        }
                }

                if (y < _rowsCellsLength.Length - 1)
                {
                        var downCell = _cellsCoord[(x, y + 1)];
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

        #endregion

        private async void PopCells()
        {
                for (int y = 0; y < _rowsCellsLength.Length; y++)
                {
                        for (int x = 0; x < _rowsCellsLength[y]; x++)
                        {
                                var checkCell = new List<Cell> {_cellsCoord[(x, y)]};
                                var matchList = new List<Cell>();
                                matchList = CheckCellsNeighborhoodMatches(checkCell, matchList);
                                if (matchList.Count < 3) continue;
                                //PopMatchCells(matchList);
                                var minimizeCells = DOTween.Sequence();
                                foreach (var matchCell in matchList)
                                {
                                        minimizeCells.Join(matchCell.gameObject.transform.DOScale(Vector3.zero, cellsPopTime));
                                }

                                await minimizeCells.Play().AsyncWaitForCompletion();
                                
                                var maximizeCells = DOTween.Sequence();
                                foreach (var cell in matchList)
                                {
                                        var randomType = Random.Range(1, cellSprites.Length);
                                        cell.SetImage = cellSprites[randomType];
                                        cell.CellType = randomType switch
                                        {
                                                1 => CellType.First,
                                                2 => CellType.Second,
                                                3 => CellType.Third,
                                                4 => CellType.Fourth,
                                                5 => CellType.Fifth,
                                                _ => cell.CellType
                                        };
                                        maximizeCells.Join(cell.transform.DOScale(Vector3.one, cellsPopTime));
                                }

                                await maximizeCells.Play().AsyncWaitForCompletion();
                                _cellsMove = false;
                        }  
                }
        }
        
        private async void PopMatchCells(List<Cell> matchCells)
        {
                
                ChangePoppedCells(matchCells);
        }

        private async void ChangePoppedCells(List<Cell> matchCells)
        {
                var maximizeCells = DOTween.Sequence();
                foreach (var cell in matchCells)
                {
                        var randomType = Random.Range(1, cellSprites.Length);
                        cell.SetImage = cellSprites[randomType];
                        cell.CellType = randomType switch
                        {
                                1 => CellType.First,
                                2 => CellType.Second,
                                3 => CellType.Third,
                                4 => CellType.Fourth,
                                _ => cell.CellType
                        };
                        maximizeCells.Join(cell.transform.DOScale(Vector3.one, cellsPopTime));
                }

                await maximizeCells.Play().AsyncWaitForCompletion();
                _cellsMove = false;
        }

        private void CompletePopCells()
        {
                _popCells.Clear();
                _selectedCells.Clear();
                _cellsMove = false;
        }


        private void OnDestroy()
        {
                foreach (var cell in _cells.Values)
                {
                        cell.OnCellClicked -= OnCellClickedHandler;
                }
        }
}