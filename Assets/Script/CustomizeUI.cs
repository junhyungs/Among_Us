using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    #region ColorButton & GameRuleButton
    [Header("ColorButton")]
    [SerializeField] private Button _colorButton;
    [SerializeField] private GameObject _colorPanelObject;

    [Header("GameRuleButton")]
    [SerializeField] private Button _gameRuleButton;
    [SerializeField] private GameObject _gameRulePanelObject;
    #endregion
    [Space]
    [SerializeField] private Image _characterPreviewImage;
    [SerializeField] private ColorSelectButton[] _colorSelectButtonComponents;

    private void Awake()
    {
        SetCharacterPreviewImageMaterial();
    }

    private void OnEnable()
    {
        ActiveColorPanel();
        UpdateColorButton();

        if(NetworkManager.singleton is AMONGUS_RoomManager roomManager)
        {
            var roomSlots = roomManager.roomSlots;

            foreach(var networkroomplayer in roomSlots)
            {
                var player = networkroomplayer as AMONGUS_RoomPlayer;
                if(player != null && player.isLocalPlayer)
                {
                    UpdatePreviewColor(player.CurrentPlayerColor);
                    break;
                }
            }
        }
    }

    public void ActiveColorPanel()
    {
        SetColorAndGameRulePanel(true);
    }

    public void ActiveGameRulePanel()
    {
        SetColorAndGameRulePanel(false);
    }

    private void SetColorAndGameRulePanel(bool value)
    {
        _colorButton.image.color = new Color(0f, 0f, 0f, value ? 0.75f : 0.25f);
        _gameRuleButton.image.color = new Color(0f, 0f, 0f, value ? 0.25f : 0.75f);

        _colorPanelObject.SetActive(value);
        _gameRulePanelObject.SetActive(!value);
    }

    #region ColorButton

    private void SetCharacterPreviewImageMaterial()
    {
        var material = Instantiate(_characterPreviewImage.material);

        _characterPreviewImage.material = material;
    }

    public void UpdateColorButton()
    {
        for(int i = 0; i < _colorSelectButtonComponents.Length; i++)
        {
            _colorSelectButtonComponents[i].SetInteractable(true);
        }

        if (NetworkManager.singleton is AMONGUS_RoomManager roomManager)
        {
            var roomSlots = roomManager.roomSlots;

            foreach (var networkroomplayer in roomSlots)
            {
                var amongusroomplayer = networkroomplayer as AMONGUS_RoomPlayer;

                if (amongusroomplayer != null)
                {
                    _colorSelectButtonComponents[(int)amongusroomplayer.CurrentPlayerColor].SetInteractable(false);
                }
            }
        }
    }

    public void OnStopClientColorButton(PlayerColorType playerColorType)
    {
        _colorSelectButtonComponents[(int)playerColorType].SetInteractable(true);
    }

    public void UpdatePreviewColor(PlayerColorType playerColorType)
    {
        _characterPreviewImage.material.SetColor("_Player_Color", PlayerColor.GetPlayerColor(playerColorType));
    }

    public void OnClickColorButton(int index)
    {
        if (_colorSelectButtonComponents[index]._isInteractable)
        {
            RequestCommandMessage(index);

            UpdatePreviewColor((PlayerColorType)index);
        }
    }

    private void RequestCommandMessage(int index)
    {
        AMONGUS_RoomPlayer.MyPlayer.RequestCommanSetPlayerColor((PlayerColorType)index);

        AMONGUS_RoomPlayer.MyPlayer.CharacterMove.RequestCommandSetPlayerColor((PlayerColorType)index);
    }

    public void OpenCustomizeUI()
    {
        SetMyCharacterIsMoving(false);
        gameObject.SetActive(true);
    }

    public void CloseCustomizeUI()
    {
        SetMyCharacterIsMoving(true);
        gameObject.SetActive(false);
    }

    private void SetMyCharacterIsMoving(bool isMoving)
    {
        var characterMove = AMONGUS_RoomPlayer.MyPlayer.CharacterMove;

        characterMove.IsMoving = isMoving;
    }
    #endregion
}
