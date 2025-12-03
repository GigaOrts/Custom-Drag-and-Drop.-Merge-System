using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    private const int GridSize = 3 * 3;

    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Item _itemPrefab;
    [SerializeField] private Transform _container;

    [SerializeField] private Button _spawnItemButton;

    private List<Cell> _cells = new List<Cell>(GridSize);

    private void OnEnable()
    {
        _spawnItemButton.onClick.AddListener(SpawnItem);
    }

    private void OnDisable()
    {
        _spawnItemButton.onClick.RemoveListener(SpawnItem);
    }

    private void Start()
    {
        SetupGrid();
        SetupItems();
    }

    private List<Cell> GetEmptyCells()
    {
        List<Cell> list = new List<Cell>();

        foreach (Cell cell in _cells)
        {
            if (cell.IsEmpty)
            {
                list.Add(cell);
            }
        }

        return list;
    }

    private void SpawnItem()
    {
        List<Cell> emptyCells = GetEmptyCells();

        if (emptyCells.Count == 0)
        {
            //TODO: Make Popup Message
            return;
        }

        int randomIndex = Random.Range(0, emptyCells.Count);
        Cell cell = emptyCells[randomIndex];

        Item item = Instantiate(_itemPrefab);
        cell.AcceptItem(item);
    }

    private void SetupGrid()
    {
        for (int i = 0; i < _cells.Capacity; i++)
        {
            Cell cell = Instantiate(_cellPrefab, _container);
            _cells.Add(cell);
        }
    }

    private void SetupItems()
    {
        for (int i = 0; i < 2; i++)
        {
            SpawnItem();
        }
    }
}
