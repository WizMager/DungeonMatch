using System.Collections.Generic;
using UnityEngine;

public class CellsController : MonoBehaviour
{
        [SerializeField] private RowsContainerComponent rowsContainerComponent;
        private readonly Dictionary<int, Cell> _cells = new();
        private List<Cell> _selectedCells = new();

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
                                cell.Id = id;
                                cell.OnCellClicked += OnCellClickedHandler;
                                _cells.Add(id, cell);
                        }
                }
        }

        private void OnCellClickedHandler(int id)
        {
                var selectedCell = _cells[id];
                if (!_selectedCells.Contains(selectedCell))
                {
                        if (_selectedCells.Count > 0)
                        {
                                
                        }
                        else
                        {
                                _selectedCells.Add(selectedCell);
                        }
                }
        }

        private void OnDestroy()
        {
                foreach (var cell in _cells.Values)
                {
                        cell.OnCellClicked -= OnCellClickedHandler;
                }
        }
}