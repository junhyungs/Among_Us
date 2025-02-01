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
                //Toggle�� isOn ���� ���� üũ �ڽ��� Ȱ��ȭ, ��Ȱ��ȭ ��.
                //�ٵ� isOn�� ���� �Ҵ��ϸ� onValueChange �̺�Ʈ�� ����Ǳ� ������ �̹��� ó���� �ҰŶ�� 
                //SetIsOnWithoutNotify(true)�� �����Ͽ� �̺�Ʈ�� �߻����� �ʵ��� ��.
                _myToggle.SetIsOnWithoutNotify(true);
            }
            //_inactiveObject.SetActive(false); 
        }
    }

    private void OnEnable()
    {
        
    }
}
