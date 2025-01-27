using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

public class LobbyUIManager : MonoBehaviour
{
    #region Instance
    private static LobbyUIManager _instance;

    public static LobbyUIManager Instance => _instance;
    #endregion

    #region CustomizeUI
    [SerializeField] private CustomizeUI _customizeUI;
    public CustomizeUI CustomizeUI => _customizeUI;
    #endregion

    #region UseButton
    [SerializeField] private Button _useButton;
    [SerializeField] private Sprite _originSprite;
    #endregion


    private void Awake()
    {
        _instance = this;
    }

    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        _useButton.image.sprite = sprite;
        _useButton.onClick.AddListener(action);
        _useButton.interactable = true;
    }

    public void UnSetUseButton()
    {
        _useButton.image.sprite = _originSprite;
        _useButton.onClick.RemoveAllListeners();
        _useButton.interactable = false;
    }
}
