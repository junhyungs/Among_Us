using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Button _mouseControlButton;
    [SerializeField] private Button _keyBoardMouseControlButton;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected virtual void OnEnable()
    {
        SetPlayerSettings();
    }

    protected void SetPlayerMove(bool isMoving)
    {
        var myRoomPlayer = AMONGUS_RoomPlayer.MyPlayer;

        var myCharacterMove = myRoomPlayer.CharacterMove;

        if (myRoomPlayer == null ||
            myCharacterMove == null)
        {
            return;
        }

        myCharacterMove.IsMoving = isMoving;
    }

    public void SetControlMode(int controlType)
    {
        PlayerSettings._controlType = (ControlType)controlType;
        SetPlayerSettings();
    }

    private void SetPlayerSettings()
    {
        switch (PlayerSettings._controlType)
        {
            case ControlType.Mouse:
                _mouseControlButton.image.color = Color.green;
                _keyBoardMouseControlButton.image.color = Color.white;
                break;
            case ControlType.KeyboardMouse:
                _mouseControlButton.image.color = Color.white;
                _keyBoardMouseControlButton.image.color = Color.green;
                break;
        }
    }

    public void Close()
    {
        StartCoroutine(CloseDelay());
    }

    private IEnumerator CloseDelay()
    {
        _animator.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        _animator.ResetTrigger("close");
    }
}
