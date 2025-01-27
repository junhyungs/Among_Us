using UnityEngine;

public class CustomizeLaptop : MonoBehaviour
{
    [SerializeField] private Sprite _useButtonSprite;
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        var matrial = Instantiate(_spriteRenderer.material);

        _spriteRenderer.material = matrial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetHighlightValue(collision, 1f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetHighlightValue(collision, 0f);
    }

    private void SetHighlightValue(Collider2D collision, float value)
    {
        var character = collision.GetComponent<CharacterMove>();

        if (character != null && character.isOwned)
        {
            _spriteRenderer.material.SetFloat("_HightLightValue", value);

            if(value > 0f)
            {
                LobbyUIManager.Instance.SetUseButton(_useButtonSprite, OnClickUse);
            }
            else
            {
                LobbyUIManager.Instance.UnSetUseButton();
            }
        }
    }

    public void OnClickUse()
    {
        LobbyUIManager.Instance.CustomizeUI.OpenCustomizeUI();
    }
}
