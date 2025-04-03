using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
        public Action<int> OnCellClicked;
        [SerializeField] private Image startCellImage;
        [SerializeField] private Button cellButton;
        private Image _image;
        private int _xNumber;
        private int _yNumber;
        private int _id;
        private CellType _cellType;

        public Image Image
        {
                get => _image;
                set
                {
                        _image = value;
                        cellButton.targetGraphic = _image;
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
        public Sprite SetImage
        {
                set
                {
                        if (startCellImage.sprite == value) return;
                        startCellImage.sprite = value;
                }
        }
        public CellType CellType
        {
                get => _cellType;
                set
                {
                        if (_cellType == value) return;
                        _cellType = value;
                }
        }

        private void Start()
        {
                Image = startCellImage;
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