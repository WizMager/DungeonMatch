using System;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
        public Action<int> OnCellClicked;
        [SerializeField] private Image cellImage;
        [SerializeField] private Button cellButton;
        private int _xNumber;
        private int _yNumber;
        private int _id;

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
                        if (cellImage.sprite == value) return;
                        cellImage.sprite = value;
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