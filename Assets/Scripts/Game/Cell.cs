using System;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Game
{
        public class Cell : MonoBehaviour
        {
                public Action<int> OnCellClicked;
                [SerializeField] private Image cellImage;
                [SerializeField] private Button cellButton;
                private int _xNumber;
                private int _yNumber;
                private int _id;
                private CellType _cellType;

                public Image Image
                {
                        get => cellImage;
                        set
                        {
                                if (value == cellImage) return;
                                cellImage = value;
                                cellButton.targetGraphic = cellImage;
                        }
                }
                public int XNumber
                {
                        get => _xNumber;
                        set
                        {
                                if (value >= 0)
                                {
                                        _xNumber = value;
                                }
                                else
                                {
                                        Debug.LogError("Input xNumber value is negative.");
                                }
                        }
                }
                public int YNumber
                {
                        get => _yNumber;
                        set
                        {
                                if (value >= 0)
                                {
                                        _yNumber = value;
                                }
                                else
                                {
                                        Debug.LogError("Input yNumber value is negative.");
                                }    
                        }
                }
                public int Id
                {
                        get => _id;
                        set
                        {
                                if (value < 0)
                                {
                                        Debug.LogError("Id can't be negative");    
                                }
                                else
                                {
                                        _id = value;
                                }
                        }
                }
                public Sprite ChangeSprite
                {
                        set
                        {
                                if (Image.sprite == value) return;
                                Image.sprite = value;
                        }
                }
                public CellType CellType
                {
                        get => _cellType;
                        set
                        {
                                if (value == _cellType) return;
                                _cellType = value;
                        }
                }

                private void Start()
                {
                        cellButton.onClick.AddListener(OnCellClickHandler);
                }

                private void OnCellClickHandler()
                {
                        OnCellClicked?.Invoke(Id);
                }

                private void OnDestroy()
                {
                        cellButton.onClick.RemoveListener(OnCellClickHandler);  
                }
        }
}