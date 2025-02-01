using UnityEngine;
using UnityEngine.UI;

public class GameRuleItem : MonoBehaviour
{
    [SerializeField] private GameRuleStore _ruleStore;
    [SerializeField] private GameObject _inactiveObject;
    private Toggle _myToggle;
   
    private void Awake()
    {
        _myToggle = _inactiveObject.transform.GetComponentInChildren<Toggle>();

        if (!AMONGUS_RoomPlayer.MyPlayer.isServer)
        {
            if (_myToggle != null)
            {
                _myToggle.interactable = false;
                //Toggle의 isOn 값에 따라 체크 박스가 활성화, 비활성화 됨.
                //근데 isOn에 값을 할당하면 onValueChange 이벤트가 실행되기 때문에 이미지 처리만 할거라면 
                //SetIsOnWithoutNotify(true)로 설정하여 이벤트가 발생되지 않도록 함.
                _myToggle.SetIsOnWithoutNotify(true);
            }
            //_inactiveObject.SetActive(false); 
        }
    }

    private void OnEnable()
    {
        
    }
}
