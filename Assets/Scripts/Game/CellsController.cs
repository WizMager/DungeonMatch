using System.Collections.Generic;
using ComponentScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Game
{
        public class CellsController : MonoBehaviour
        {
                [SerializeField] private RowsContainerComponent rowsContainerComponent;
                [SerializeField] private CellValue[] cellsValues;
                [SerializeField] private TMP_Text scoreText;
                [SerializeField] private Button mainMenuButton;
                [SerializeField] private float cellSwapTime;
                [SerializeField] private float cellsPopTime;
                private CellsData _cellsData;
                private CellsSwapper _cellsSwapper;
                private PopCells _popCells;
                private bool _popCellsNow;

                private void Start()
                {
                        Initialization();
                        foreach (var cell in _cellsData.IdCells.Values)
                        {
                                cell.OnCellClicked += OnCellClickedHandler;
                        }
                        mainMenuButton.onClick.AddListener(ReturnMenu);
                        _cellsSwapper.OnSuccessSwap += OnSuccessSwapHandler;
                        _popCells.OnPopCells += OnPopCellsHandler;
                        _popCells.PopAll();
                }

                private void OnPopCellsHandler(bool popCellsNow)
                {
                        _popCellsNow = popCellsNow;
                }


                private void Initialization()
                {
                        _cellsData = new CellsData(rowsContainerComponent, cellsValues);
                        var checkCellsTypeMatches = new CheckCells(_cellsData);
                        var scoreCounter = new ScoreCounter(scoreText, cellsValues);
                        _cellsSwapper = new CellsSwapper(cellSwapTime, checkCellsTypeMatches);
                        _popCells = new PopCells(_cellsData, checkCellsTypeMatches, scoreCounter, cellsPopTime);
                }

                private void OnCellClickedHandler(int id)
                {
                        if (_popCellsNow) return;
                        _cellsSwapper.Swap(_cellsData.GetCell(id));
                }
                
                private void OnSuccessSwapHandler(List<Cell> matchCells)
                {
                        _popCells.PopAll();
                }

                private void ReturnMenu()
                {
                        SceneManager.LoadScene(0);
                }

                private void OnDestroy()
                {
                        mainMenuButton.onClick.RemoveListener(ReturnMenu);
                        foreach (var cell in _cellsData.IdCells.Values)
                        {
                                cell.OnCellClicked -= OnCellClickedHandler;
                        }
                }
        }
}