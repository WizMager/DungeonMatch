using System.Collections.Generic;
using ComponentScripts;
using UnityEngine;
using Utils;

namespace Game
{
    public class CellsData
    {
        private readonly CellValue[] _cellsValues;
        
        public Dictionary<(int x, int y), Cell> CellsCoordinates { get; } = new();
        public Dictionary<int, Cell> IdCells { get; } = new ();
        public int[] RowsCellsCount { get; private set; }

        public CellsData(RowsContainerComponent rowsContainerComponent, CellValue[] cellsValues)
        {
            _cellsValues = cellsValues;
            InitializeCells(rowsContainerComponent.GetRowsContainers);
        }

        private void InitializeCells(RowCellContainerComponent[] rows)
        {
            RowsCellsCount = new int[rows.Length];
            for (int y = 0; y < rows.Length; y++)
            {
                var rowCells = rows[y].GetRowCells;
                RowsCellsCount[y] = rowCells.Length;
                for (int x = 0; x < rowCells.Length; x++)
                {
                    var cell = rowCells[x].GetComponent<Cell>();
                    cell.XNumber = x;
                    cell.YNumber = y;
                    var id = x + y * rowCells.Length;
                    var randomType = Random.Range(0, _cellsValues.Length);
                    cell.ChangeSprite = _cellsValues[randomType].sprite;
                    cell.CellType = _cellsValues[randomType].cellType;
                    cell.Id = id;
                    IdCells.Add(id, cell);
                    CellsCoordinates.Add((x, y), cell);
                }
            } 
        }

        public Cell GetCell(int id)
        {
            return IdCells[id];
        }

        public Cell GetCell((int x, int y) coordinates)
        {
            return CellsCoordinates[coordinates];
        }

        public void ChangeCellsSprite(List<Cell> cells)
        {
            foreach (var cell in cells)
            {
                var randomType = Random.Range(0, _cellsValues.Length);
                cell.ChangeSprite = _cellsValues[randomType].sprite;
                cell.CellType = _cellsValues[randomType].cellType;
            }
        }
    }
}