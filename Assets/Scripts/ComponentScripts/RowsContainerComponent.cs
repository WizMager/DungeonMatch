using UnityEngine;

namespace ComponentScripts
{
        public class RowsContainerComponent : MonoBehaviour
        {
                [SerializeField] private RowCellContainerComponent[] rowsContainer;

                public RowCellContainerComponent[] GetRowsContainers => rowsContainer;
        }
}