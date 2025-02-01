using UnityEngine;
using UnityEngine.UI;

public enum ToggleType
{
    IsRecommandRule,
    ConfirmEjects,
    AnonymousVotes,
    VisualTasks
}

public class GameRuleItem_Toggle : GameRuleItem
{
    [Header("RuleStore")]
    [SerializeField] private GameRuleStore _ruleStore;
    [Header("ToggleName")]
    [SerializeField] private ToggleType _toggleName;

    private Toggle _myToggle;

    protected override void Awake()
    {
        base.Awake();

        if (!AMONGUS_RoomPlayer.MyPlayer.isServer)
        {
            _myToggle = InactiveObject.transform.GetComponentInChildren<Toggle>();

            if ( _myToggle != null )
            {
                _myToggle.interactable = false;
            }
        }
    }

    private void OnEnable()
    {
        if (!AMONGUS_RoomPlayer.MyPlayer.isServer)
        {
            RefreshToggle();
        }
    }

    private void RefreshToggle()
    {
        switch( _toggleName )
        {
            case ToggleType.IsRecommandRule:
                IsOn(_ruleStore.IsRecommandRule);
                break;
            case ToggleType.ConfirmEjects:
                IsOn(_ruleStore.ConfirmEjects);
                break;
            case ToggleType.AnonymousVotes:
                IsOn(_ruleStore.AnonymousVotes);
                break;
            case ToggleType.VisualTasks:
                IsOn(_ruleStore.VisualTasks);
                break;
        }
    }

    private void IsOn(bool isOn)
    {
        _myToggle.SetIsOnWithoutNotify(isOn);
    }
}
