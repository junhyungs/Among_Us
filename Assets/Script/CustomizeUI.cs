using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField] private Image _characterPreviewImage;
    [SerializeField] private ColorSelectButton[] _colorSelectButtonComponents;

    private void Awake()
    {
        SetCharacterPreviewImageMaterial();
    }

    private void SetCharacterPreviewImageMaterial()
    {
        var material = Instantiate(_characterPreviewImage.material);

        _characterPreviewImage.material = material;
    }

    private void OnEnable()
    {
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

    public void UpdateColorButton()
    {
        for(int i = 0; i < _colorSelectButtonComponents.Length; i++)
        {
            _colorSelectButtonComponents[i].SetInteractable(true);
        }

        //Debug.Log(AMONGUS_User.Instance.SyncHashSet);

        //var syncSet = AMONGUS_User.Instance.SyncHashSet;

        //foreach (var roomPlayer in syncSet)
        //{
        //    if(roomPlayer != null)
        //    {
        //        _colorSelectButtonComponents[(int)roomPlayer.CurrentPlayerColor].SetInteractable(false);
        //    }
        //}

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
}
