using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField] private Image _characterPreviewImage;
    [SerializeField] private ColorSelectButton[] _colorSelectButtonComponents;

    private void Start()
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

        if(NetworkManager.singleton is AMONGUS_RoomManager roomManager)
        {
            var roomSlots = roomManager.roomSlots;

            foreach (var networkroomplayer in roomSlots)
            {
                var amongusroomplayer = networkroomplayer as AMONGUS_RoomPlayer;

                if(amongusroomplayer != null)
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
            AMONGUS_RoomPlayer.MyRoomPlayer.CommandSetPlayerColor((PlayerColorType)index);
            UpdatePreviewColor((PlayerColorType)index);
        }
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
        var myRoomPlayer = AMONGUS_RoomPlayer.MyRoomPlayer;

        var characterMoveComponent = myRoomPlayer._characterMove;

        characterMoveComponent.IsMoving = isMoving;
    }
}
