using UnityEngine;
using UnityEngine.UI;


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
}
