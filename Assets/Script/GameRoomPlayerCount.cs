using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections;

public class GameRoomPlayerCount : NetworkBehaviour
{
    [SyncVar]
    private int _minPlayerCount;
    [SyncVar]
    private int _maxPlayerCount;
    [SyncVar]
    private int _currentPlayerCount;

    public int SyncCurrentPlayerCount
    {
        get => _currentPlayerCount;
        set
        {
            if (isServer)
            {
                _currentPlayerCount = value;
            }
        }
    }

    [SerializeField] private Text _playerCountText;

    private void Start()
    {
        if (isServer)
        {
            InitializePlayerCount();
        }
    }

    private void InitializePlayerCount()
    {
        var roomManager = AMONGUS_RoomManager.Instance;

        if (roomManager != null)
        {
            _minPlayerCount = roomManager.MinPlayerCount;
            _maxPlayerCount = roomManager.maxConnections;
        }
    }

    public void OnUpdatePlayerCountText()
    {
        bool isStartable = _currentPlayerCount >= _minPlayerCount;

        _playerCountText.color = isStartable ? Color.white : Color.red;

        _playerCountText.text = string.Format("{0} / {1}", _currentPlayerCount, _maxPlayerCount);

        if (isServer)
        {
            LobbyUIManager.Instance.SetInteractableButton(isStartable);
        }
    }
}
