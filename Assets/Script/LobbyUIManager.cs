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
    
    #region GameRoomPlayerCount
    [SerializeField] private GameRoomPlayerCount _gameRoomPlayerCount;
    public GameRoomPlayerCount GameRoomPlayerCount => _gameRoomPlayerCount;
    #endregion

    #region StartButton
    [SerializeField] private Button _startButton;
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

    public void ActiveStartButton()
    {
        _startButton.gameObject.SetActive(true);
    }

    public void SetInteractableButton(bool isInteractable)
    {
        _startButton.interactable = isInteractable;
    }

    /// <summary>
    /// Server
    /// </summary>
    public void OnClickStartButton()
    {
        if (!AMONGUS_RoomPlayer.MyPlayer.isServer)
        {
            return;
        }

        var roomManager = AMONGUS_RoomManager.Instance;

        var roomSlots = roomManager.roomSlots;

        foreach(var networkRoomPlayer in roomSlots)
        {
            var amongusRoomPlayer = networkRoomPlayer as AMONGUS_RoomPlayer;

            amongusRoomPlayer.ReadyToBegin();
        }

        roomManager.ServerChangeScene(roomManager.GameplayScene);
    }
}
