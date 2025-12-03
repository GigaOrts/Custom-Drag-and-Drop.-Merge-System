using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Item : MonoBehaviour
{
    public RectTransform RectTransform { get; private set; }
    public Cell CurrentCell { get; private set; }
    
    [SerializeField] public ItemData Data;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textField;

    void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Initialize(Data);
    }

    public void Initialize(ItemData data)
    {
        Data = data;
        image.sprite = data.sprite;
        textField.text = $"{data.level}";
    }

    public void SetCell(Cell cell)
    {
        CurrentCell = cell;
    }

    public void Detach()
    {
        if (CurrentCell != null)
        {
            CurrentCell.Clear();
            CurrentCell = null;
        }
    }

    public bool CanMergeWith(Item other)
    {
        return Data.level == other.Data.level;
    }

    public void MergeInto(Item target)
    {
        Destroy(gameObject);

        if (target.Data.nextLevel != null)
        {
            target.Initialize(target.Data.nextLevel);
        }
        else
        {
            //TODO: Popup message $"Max level reached - level {target.Data.level}" 
        }
    }
}