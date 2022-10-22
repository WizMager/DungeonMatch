using System.Collections.Generic;
using ComponentScripts;
using UnityEngine;

namespace Game
{
    public class CellsModel
    {
        private readonly Dictionary<(int x, int y), Cell> _cellsCoordinate = new ();

        public Dictionary<(int x, int y), Cell> CellsCoordinates { get; } = new();
        public Dictionary<int, Cell> IdCells { get; } = new ();
        public int[] RowsCellsCount { get; private set; }

        public CellsModel(RowsContainerComponent rowsContainerComponent, Sprite[] sprites)
        {
            InitializeCells(rowsContainerComponent.GetRowsContainers, sprites);
        }

        private void InitializeCells(RowCellContainerComponent[] rows, Sprite[] sprites)
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
                    var randomType = Random.Range(0, sprites.Length);
                    cell.ChangeSprite = sprites[randomType];
                    cell.CellType = randomType;
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
    }
}