using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CellsController : MonoBehaviour
{
        [SerializeField] private RowsContainerComponent rowsContainerComponent;
        [SerializeField] private Sprite[] cellSprites;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button toMenuButton;
        [SerializeField] private float cellMoveTime;
        [SerializeField] private float cellsPopTime;
        private readonly Dictionary<int, Cell> _cells = new();
        private readonly Dictionary<(int x, int y), Cell> _cellsCoord = new ();
        private readonly List<Cell> _selectedCells = new();
        private bool _cellsMove;
        private int[] _rowsCellsLength;
        private int _score;

        private void Start()
        {
                toMenuButton.onClick.AddListener(ReturnMenu);
              Initialization(rowsContainerComponent);
        }

        private void ReturnMenu()
        {
                SceneManager.LoadScene(0);
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
                                        6 => CellType.Sixth,
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
                                _selectedCells.Clear();
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
                var firstSelectedCell = selectedCells[0];
                var secondSelectedCell = selectedCells[1];
                var cellOneImagePosition = firstSelectedCell.Image.transform.position;
                var cellTwoImagePosition = secondSelectedCell.Image.transform.position;
                var sequence = DOTween.Sequence();
                sequence.Join(firstSelectedCell.Image.transform.DOMove(cellTwoImagePosition, cellMoveTime));
                sequence.Join(secondSelectedCell.Image.transform.DOMove(cellOneImagePosition, cellMoveTime));
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
                var firstCellList = new List<Cell> {firstSelectedCell};
                var secondCellList = new List<Cell> {secondSelectedCell};
                var invertSelectedCells = new List<Cell> {secondSelectedCell, firstSelectedCell};
                if (CheckHorizontalVertical(firstSelectedCell, CheckCellsMatches(firstCellList)) || CheckHorizontalVertical(secondSelectedCell, CheckCellsMatches(secondCellList)))
                {
                        PopCells(); 
                }
                else
                {
                        Swap(invertSelectedCells);
                        _cellsMove = false;      
                }
        }

        private async void Swap(List<Cell> selectedCells)
        {
                var firstSelectedCell = selectedCells[0];
                var secondSelectedCell = selectedCells[1];
                var cellOnePosition = firstSelectedCell.Image.transform.position;
                var cellTwoPosition = secondSelectedCell.Image.transform.position;
                var sequence = DOTween.Sequence();
                sequence.Join(firstSelectedCell.Image.transform.DOMove(cellTwoPosition, cellMoveTime));
                sequence.Join(secondSelectedCell.Image.transform.DOMove(cellOnePosition, cellMoveTime));
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
        }
        
        #endregion

        #region CheckMatches

        private List<Cell> CheckCellsMatches(List<Cell> neigh, List<Cell> cellsList = null)
        {
                var result = cellsList ?? new List<Cell> {neigh[0]};
                if (neigh.Count <= 0) return result;
                foreach (var cell in neigh)
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

        private bool CheckHorizontalVertical(Cell checkCell, List<Cell> matchCells)
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

        #endregion

        private async void PopCells()
        {
                for (int y = 0; y < _rowsCellsLength.Length; y++)
                {
                        for (int x = 0; x < _rowsCellsLength[y]; x++)
                        {
                                var checkCell = new List<Cell> {_cellsCoord[(x, y)]};
                                var matchList = new List<Cell>();
                                matchList = CheckCellsMatches(checkCell, matchList);

                                if (!CheckHorizontalVertical(_cellsCoord[(x, y)], matchList)) continue;
                                var scoreCounter = 0;
                                var scoreCell = 0;
                                switch (matchList[0].CellType)
                                {
                                        case CellType.None:
                                                break;
                                        case CellType.First:
                                                scoreCell = 1;
                                                break;
                                        case CellType.Second:
                                                scoreCell = 2;
                                                break;
                                        case CellType.Third:
                                                scoreCell = 3;
                                                break;
                                        case CellType.Fourth:
                                                scoreCell = 4;
                                                break;
                                        case CellType.Fifth:
                                                scoreCell = 5;
                                                break;
                                        case CellType.Sixth:
                                                scoreCell = 6;
                                                break;
                                        default:
                                                throw new ArgumentOutOfRangeException();
                                }
                                var minimizeCells = DOTween.Sequence();
                                foreach (var matchCell in matchList)
                                {
                                        scoreCounter += scoreCell;
                                        minimizeCells.Join(matchCell.gameObject.transform.DOScale(Vector3.zero, cellsPopTime));
                                }

                                await minimizeCells.Play().AsyncWaitForCompletion();
                                _score += scoreCounter;
                                scoreText.text = $"Score: {_score}";
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
                                                6 => CellType.Sixth,
                                                _ => cell.CellType
                                        };
                                        maximizeCells.Join(cell.transform.DOScale(Vector3.one, cellsPopTime));
                                }

                                await maximizeCells.Play().AsyncWaitForCompletion();
                                _cellsMove = false;
                        }  
                }
        }

        private void OnDestroy()
        {
                toMenuButton.onClick.RemoveListener(ReturnMenu);
                foreach (var cell in _cells.Values)
                {
                        cell.OnCellClicked -= OnCellClickedHandler;
                }
        }
}