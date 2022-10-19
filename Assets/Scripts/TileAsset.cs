using UnityEngine;

[CreateAssetMenu(menuName = "TileAsset")]
public class TileAsset : ScriptableObject
{
    public Sprite sprite;
    public CellType cellType;
}