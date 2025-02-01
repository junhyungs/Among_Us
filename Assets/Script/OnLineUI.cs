using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class OnLineUI : MonoBehaviour
{
    [SerializeField] private InputField _nickNameInputField;
    [SerializeField] private GameObject _createRoomUI;

    private Animator _animator;

    private void Awake()
    {
        _animator = _nickNameInputField.gameObject.GetComponent<Animator>();
    }

    public void OnClickCreateRoomButton()
    {
        if (!string.IsNullOrWhiteSpace(_nickNameInputField.text))
        {
            PlayerSettings._nickName = _nickNameInputField.text;

            gameObject.SetActive(false);
            _createRoomUI.SetActive(true);
        }
        else
        {
            _animator.SetTrigger("On");
        }
    }

    public void OnClickStartClient()
    {
        if (!string.IsNullOrWhiteSpace(_nickNameInputField.text))
        {
            PlayerSettings._nickName = _nickNameInputField.text;

            AMONGUS_RoomManager.singleton.StartClient();
        }
        else
        {
            _animator.SetTrigger("On");
        }
    }
}
