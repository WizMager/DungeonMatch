using System.Collections.Generic;
using ComponentScripts;
using UnityEngine;

namespace Game
{
    public class CellsModel
    {
        private readonly Dictionary<(int x, int y), Cell> _cellsCoordinate = new ();
        private int[] _rowsCount;

        public Dictionary<int, Cell> IdCells { get; } = new ();

        public CellsModel(RowsContainerComponent rowsContainerComponent, Sprite[] sprites)
        {
            var rowsContainer = rowsContainerComponent.GetRowsContainers;
            _rowsCount = new int[rowsContainer.Length];
            for (int y = 0; y < rowsContainer.Length; y++)
            {
                var rowCells = rowsContainer[y].GetRowCells;
                _rowsCount[y] = rowCells.Length;
                for (int x = 0; x < rowCells.Length; x++)
                {
                    var cell = rowCells[x].GetComponent<Cell>();
                    cell.XNumber = x;
                    cell.YNumber = y;
                    var id = x + y * rowCells.Length;
                    var randomType = Random.Range(1, sprites.Length);
                    cell.ChangeSprite = sprites[randomType];
                    cell.CellType = randomType;
                    cell.Id = id;
                    IdCells.Add(id, cell);
                    _cellsCoordinate.Add((x, y), cell);
                }
            }
        }
    }
}