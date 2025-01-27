using UnityEngine;

public class OutLineObject : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private Color _outlineColor;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        var material = Instantiate(_spriteRenderer.material);

        _spriteRenderer.material = material;

        _spriteRenderer.material.SetColor("_OutLine", _outlineColor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnableSpriteRenderer(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EnableSpriteRenderer(collision, false);
    }

    private void EnableSpriteRenderer(Collider2D collision, bool enable)
    {
        var character = collision.GetComponent<CharacterMove>();

        if (character != null && character.isOwned)
        {
            _spriteRenderer.enabled = enable;
        }
    }
}
