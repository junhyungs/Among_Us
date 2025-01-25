using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    public enum SortType
    {
        Static,
        Update
    }

    [SerializeField] private SortType _sortType;

    private SpriteSorter _sorter;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _sorter = FindFirstObjectByType<SpriteSorter>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sortingOrder = _sorter.GetSortingOrder(gameObject);
    }

    private void Update()
    {
        if(_sortType == SortType.Update)
        {
            _spriteRenderer.sortingOrder = _sorter.GetSortingOrder(gameObject);
        }
    }
}
