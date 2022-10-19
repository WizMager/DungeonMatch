using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ButtonsModel
{
        private readonly Dictionary<int, CellType> _cells = new ();

        public ButtonsModel(int buttonsCount)
        {
            FillCells(buttonsCount);
        }

        private void FillCells(int buttonsCount)
        {
                var cellTypes = Enum.GetNames(typeof(CellType)).Length;
                for (int i = 0; i < buttonsCount; i++)
                {
                        var randomNumberCellType = Random.Range(1, cellTypes);
                        CellType chosenCellType;
                        switch (randomNumberCellType)
                        {
                                case 1:
                                        chosenCellType = CellType.First;
                                        break;
                                case 2:
                                        chosenCellType = CellType.Second;
                                        break;
                                case 3:
                                        chosenCellType = CellType.Third;
                                        break;
                                case 4:
                                        chosenCellType = CellType.Fourth;
                                        break;
                                default:
                                        chosenCellType = CellType.None;
                                        break;
                        }
                        _cells.Add(buttonsCount, chosenCellType);
                }
        }
}