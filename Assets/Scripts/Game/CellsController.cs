using System.Collections.Generic;
using ComponentScripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
        public class CellsController : MonoBehaviour
        {
                [SerializeField] private RowsContainerComponent rowsContainerComponent;
                [SerializeField] private Sprite[] cellSprites;
                [SerializeField] private TMP_Text scoreText;
                [SerializeField] private Button mainMenuButton;
                [SerializeField] private float cellSwapTime;
                [SerializeField] private float cellsPopTime;
                private CellsModel _cellsModel;
                private CellsSwapper _cellsSwapper;
                private int _score;

                private void Start()
                {
                        Initialization();
                        foreach (var cell in _cellsModel.IdCells.Values)
                        {
                                cell.OnCellClicked += OnCellClickedHandler;
                        }
                        mainMenuButton.onClick.AddListener(ReturnMenu);
                        _cellsSwapper.OnSuccessSwap += OnSuccessSwapHandler;
                }

                

                private void Initialization()
                {
                        _cellsModel = new CellsModel(rowsContainerComponent, cellSprites);
                        var checkCellsTypeMatches = new CheckCellsTypeMatches(_cellsModel);
                        _cellsSwapper = new CellsSwapper(cellSwapTime, checkCellsTypeMatches);

                        
                }

                private void OnCellClickedHandler(int id)
                {
                        _cellsSwapper.Swap(_cellsModel.GetCell(id));
                }
                
                private void OnSuccessSwapHandler(List<Cell> matchCells)
                {
                        foreach (var cell in matchCells)
                        {
                                Debug.Log(cell.Id);
                        }
                }

                private void ReturnMenu()
                {
                        SceneManager.LoadScene(0);
                }

                private void OnDestroy()
                {
                        mainMenuButton.onClick.RemoveListener(ReturnMenu);
                        foreach (var cell in _cellsModel.IdCells.Values)
                        {
                                cell.OnCellClicked -= OnCellClickedHandler;
                        }
                }
        }
}