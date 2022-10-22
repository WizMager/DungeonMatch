using UnityEngine;

namespace ComponentScripts
{
       public class RowCellContainerComponent : MonoBehaviour
       {
              [SerializeField] private GameObject[] rowCells;

              public GameObject[] GetRowCells => rowCells;
       }
}