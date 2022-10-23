using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(menuName = "CellData/Cell")]
    public class CellValue : ScriptableObject
    {
        public Sprite sprite;
        public int score;
        public CellType cellType;
    }
}