using UnityEngine;

public class RowCellContainerComponent : MonoBehaviour
{
       [SerializeField] private GameObject[] rowCells;

       public GameObject[] GetRowCells => rowCells;
}