using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Rendering.UI;

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField] private Image[] _crewImages;
    [SerializeField] private Button[] _imposterCountButtons;
    [SerializeField] private Button[] _maxPlayerCountButtons;

    private CreateRoomData _roomData;

    private void Start()
    {
        for (int i = 0; i < _crewImages.Length; i++)
        {
            var copyMaterial = Instantiate(_crewImages[i].material);

            _crewImages[i].material = copyMaterial;
        }

        _roomData = new CreateRoomData() { ImposterCount = 1, MaxPlayerCount = 10 };

        UpdateCrewImages();
    }

    public void UpdateImposterCount(int count)
    {
        ImposterCount(count);
        SetMaxPlayerCount(count);
    }

    private void ImposterCount(int count)
    {
        _roomData.ImposterCount = count;

        for (int i = 0; i < _imposterCountButtons.Length; i++)
        {
            if (i == count - 1)
            {
                _imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                _imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    private void SetMaxPlayerCount(int count)
    {
        int limitMaxPlayerCount = count == 1 ? 4 : count == 2 ? 7 : 9;

        if (_roomData.MaxPlayerCount < limitMaxPlayerCount)
        {
            UpdateMaxPlayerCount(limitMaxPlayerCount);
        }
        else
        {
            UpdateMaxPlayerCount(_roomData.MaxPlayerCount);
        }
    }

    public void UpdateMaxPlayerCount(int count)
    {
        _roomData.MaxPlayerCount = count;

        for (int i = 0; i < _maxPlayerCountButtons.Length; i++)
        {
            if (i == count - 4)
            {
                _maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                _maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }

        int limitImposterCount = count <= 6 ? 1 : count <= 8 ? 2 : 3;

        if (_roomData.ImposterCount > limitImposterCount)
        {
            _roomData.ImposterCount = limitImposterCount;

            ImposterCount(limitImposterCount);
        }

        UpdateCrewImages();
    }

    private void UpdateCrewImages()
    {
        for (int i = 0; i < _crewImages.Length; i++)
        {
            _crewImages[i].material.SetColor("_Player_Color", Color.white);
        }

        int imposterCount = _roomData.ImposterCount;
        int index = 0;

        while (imposterCount != 0)
        {
            if (index >= _roomData.MaxPlayerCount)
            {
                index = 0;
            }

            if (_crewImages[index].material.GetColor("_Player_Color") != Color.red && Random.Range(0, 5) == 0)
            {
                _crewImages[index].material.SetColor("_Player_Color", Color.red);
                imposterCount--;
            }

            index++;
        }

        for (int i = 0; i < _crewImages.Length; i++)
        {
            if (i < _roomData.MaxPlayerCount)
            {
                _crewImages[i].gameObject.SetActive(true);
            }
            else
            {
                _crewImages[i].gameObject.SetActive(false);
            }
        }
    }

    public void CreateRoom() 
    {
        var roomManager = AMONGUS_RoomManager.Instance;

        roomManager.MinPlayerCount = _roomData.ImposterCount == 1 ? 4 : _roomData.ImposterCount == 2 ? 7 : 9;
        roomManager.ImposterCount = _roomData.ImposterCount;
        roomManager.maxConnections = _roomData.MaxPlayerCount;

        roomManager.StartHost();
    }
}

public class CreateRoomData
{
    public int ImposterCount { get; set; }
    public int MaxPlayerCount { get; set; }
}