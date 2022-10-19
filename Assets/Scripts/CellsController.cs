using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CellsController : MonoBehaviour
{
        [SerializeField] private RowsContainerComponent rowsContainerComponent;
        [SerializeField] private Sprite[] cellSprites;
        private readonly Dictionary<int, Cell> _cells = new();
        private readonly List<Cell> _selectedCells = new();

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
                var selectedCell = _cells[id];
                if (_selectedCells.Contains(selectedCell)) return;
                if (_selectedCells.Count > 0)
                {
                        if (_selectedCells.Count > 2) return;
                        if (ClosestCellsCheck(_selectedCells[0], selectedCell))
                        {
                                _selectedCells.Add(selectedCell);
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