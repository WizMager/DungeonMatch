using System;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
        [SerializeField] private ButtonsContainerComponent buttonsContainer;
        private ButtonsModel _buttonsModel;

        private void Start()
        {
              InitializeCells(buttonsContainer.GetButtonViews);  
        }

        private void InitializeCells(ButtonView[] buttonViews)
        {
                for (int i = 0; i < buttonViews.Length; i++)
                {
                    buttonViews[i].Number = i;
                    buttonViews[i].OnCellClick += OnCellClickHandler;
                }
        }

        private void OnCellClickHandler(int cellNumber)
        {
            
        }

        private void OnDestroy()
        {
            foreach (var buttonView in buttonsContainer.GetButtonViews)
            {
                buttonView.OnCellClick -= OnCellClickHandler;
            }
        }
}