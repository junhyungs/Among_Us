using System;
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

        bool isSetHighlightValue = character != null && character.isOwned;

        if (!isSetHighlightValue)
        {
            return;
        }

        var lobbyUIManager = LobbyUIManager.Instance;

        if(lobbyUIManager == null)
        {
            return;
        }

        _spriteRenderer.material.SetFloat("_HightLightValue", value);

        Action action = (value > 0f) ?
            () => lobbyUIManager.SetUseButton(_useButtonSprite, OnClickUse) :
            () => lobbyUIManager.UnSetUseButton();

        action.Invoke();
    }

    public void OnClickUse()
    {
        LobbyUIManager.Instance.CustomizeUI.OpenCustomizeUI();
    }
}
