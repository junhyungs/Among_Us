using UnityEngine;

public class GameRuleItem : MonoBehaviour
{
    [SerializeField] private GameObject _inactiveObject;
    public GameObject InactiveObject => _inactiveObject;
    
    protected virtual void Awake() { }
}
