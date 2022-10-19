using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonView : MonoBehaviour
{
        public Action<int> OnCellClick;
        [SerializeField] private Image image;
        [SerializeField] private Button button;
        private int _number = -1;

        public int Number
        {
                get => _number;
                set
                {
                        if (_number == -1)
                        {
                                if (value >= 0)
                                {
                                        _number = value;    
                                }
                                else
                                {
                                        Debug.LogError("You can't set a negative number for cell(button).");
                                }
                        }
                        else
                        {
                                Debug.LogError("You can't change a number of cell(button) more than one time.");
                        } 
                }
        }

        private void Start()
        {
                button.onClick.AddListener(OnButtonClickHandler);
        }

        private void OnButtonClickHandler()
        {
                OnCellClick?.Invoke(Number);
        }

        private void OnDestroy()
        {
                button.onClick.RemoveListener(OnButtonClickHandler);    
        }
}