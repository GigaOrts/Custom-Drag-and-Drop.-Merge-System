using UnityEngine;

[CreateAssetMenu(menuName = "Merge/Item Data", fileName = "NewItemData")]
public class ItemData : ScriptableObject
{
    [Header("Main Data")]
    public int level;
    public Sprite sprite;

    [Header("Next level Data (null = max)")]
    public ItemData nextLevel;
}