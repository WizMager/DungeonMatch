using UnityEngine;

public class ButtonsContainerComponent : MonoBehaviour
{
        [SerializeField] private ButtonView[] buttonViews;

        public ButtonView[] GetButtonViews => buttonViews;
}