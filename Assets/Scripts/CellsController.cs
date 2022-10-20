using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellsController : MonoBehaviour
{
        [SerializeField] private RowsContainerComponent rowsContainerComponent;
        [SerializeField] private Sprite[] cellSprites;
        [SerializeField] private float cellMoveTime;
        private readonly Dictionary<int, Cell> _cells = new();
        private readonly List<Cell> _selectedCells = new();
        private bool _cellsMove;

        private void Start()
        {
              Initialization(rowsContainerComponent);
        }

        private void Initialization(RowsContainerComponent rowsContainerValue)
        {
                var rowsContainer = rowsContainerValue.GetRowsContainers;
                for (int y = 0; y < rowsContainer.Length; y++)
                {
                        var rowCells = rowsContainer[y].GetRowCells;
                        for (int x = 0; x < rowCells.Length; x++)
                        {
                                var cell = rowCells[x].GetComponent<Cell>();
                                cell.XNumber = x;
                                cell.YNumber = y;
                                var id = x + y * rowCells.Length;
                                cell.SetImage = cellSprites[Random.Range(1, cellSprites.Length)];
                                cell.Id = id;
                                cell.OnCellClicked += OnCellClickedHandler;
                                _cells.Add(id, cell);
                        }
                }
        }

        private void OnCellClickedHandler(int id)
        {
                if (_cellsMove) return;
                var selectedCell = _cells[id];
                if (_selectedCells.Contains(selectedCell)) return;
                if (_selectedCells.Count > 0)
                {
                        if (_selectedCells.Count > 2) return;
                        if (ClosestCellsCheck(_selectedCells[0], selectedCell))
                        {
                                _selectedCells.Add(selectedCell);
                                _cellsMove = true;
                                ChangeCells(_selectedCells);
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

        private void ChangeCells(List<Cell> selectedCells)
        {
                var cellOnePosition = selectedCells[0].Image.transform.position;
                var cellTwoPosition = selectedCells[1].Image.transform.position;
                var sequence = DOTween.Sequence();
                sequence.Join(selectedCells[0].Image.transform.DOMove(cellTwoPosition, cellMoveTime));
                sequence.Join(selectedCells[1].Image.transform.DOMove(cellOnePosition, cellMoveTime));
                sequence.Play().OnComplete(OnCompleteMoveHandler);
        }

        private void OnCompleteMoveHandler()
        {
                var imageOne = _selectedCells[0].Image;
                var imageTwo = _selectedCells[1].Image;
                _selectedCells[0].Image = imageTwo;
                _selectedCells[1].Image = imageOne;
                imageOne.transform.SetParent(_selectedCells[1].transform);
                imageTwo.transform.SetParent(_selectedCells[0].transform);
                _selectedCells.Clear();
                _cellsMove = false;
        }

        private bool ClosestCellsCheck(Cell inListCell, Cell newCell)
        {
                var listX = inListCell.XNumber;
                var listY = inListCell.YNumber;
                return listX + 1 == newCell.XNumber || listX - 1 == newCell.XNumber || listY + 1 == newCell.YNumber || listY - 1 == newCell.YNumber;
        }
        
        private void OnDestroy()
        {
                foreach (var cell in _cells.Values)
                {
                        cell.OnCellClicked -= OnCellClickedHandler;
                }
        }
}