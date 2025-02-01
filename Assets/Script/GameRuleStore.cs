using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Text;

public enum KillRange
{
    Short, Normal, Long
}

public enum TaskBarUpdates
{
    Always, Meetings, Never, Default
}

public struct GameRuleData
{
    public bool ConfirmEjects;
    public int EmergencyMeetings;
    public int EmergencyMeetingsCoolDown;
    public int MeetingTime;
    public int VoteTime;
    public bool AnonymousVotes;
    public float MoveSpeed;
    public float CrewSight;
    public float ImposterSight;
    public float KillCoolDown;
    public KillRange KillRange;
    public bool VisualTasks;
    public TaskBarUpdates TaskBarUpdates;
    public int CommomTask;
    public int ComplexTask;
    public int SimpleTask;
}

public class GameRuleStore : NetworkBehaviour
{
    [SyncVar(hook = nameof(Hook_RecommandRule))]
    private bool _isRecommandRule;
    public bool IsRecommandRule {  get { return _isRecommandRule; } }

    [SerializeField] private Toggle _isRecommandRuleToggle;
    private void Hook_RecommandRule(bool _, bool value)
    {
        _isRecommandRuleToggle.SetIsOnWithoutNotify(value);

        UpdateGameRuleOverview();
    }

    public void OnRecommandRuleToggle(bool value)
    {
        _isRecommandRule = value;

        if (value)
        {
            SetRecommandRule();
        }
    }

    [SyncVar(hook = nameof(Hook_ConfirmEjects))]
    private bool _confirmEjects;
    public bool ConfirmEjects {  get { return _confirmEjects; } }
    [SerializeField] private Toggle _confirmEjectsToggle;
    private void Hook_ConfirmEjects(bool _, bool value)
    {
        _confirmEjectsToggle.SetIsOnWithoutNotify(value);

        UpdateGameRuleOverview();
    }

    public void OnChangeConfirmEjectsToggle(bool value)
    {
        _isRecommandRule = false;

        _confirmEjects = value;
    }

    [SyncVar(hook  = nameof(Hook_EmergencyMeetings))]
    private int _emergencyMeetings;
    [SerializeField] private Text _emergencyMeetingsText;
    private void Hook_EmergencyMeetings(int _, int emergencyMeetings)
    {
        _emergencyMeetingsText.text = emergencyMeetings.ToString();

        UpdateGameRuleOverview();
    }

    public void OnChangeEmergencyMeetings(bool isPlus)
    {
        _emergencyMeetings = Mathf.Clamp(_emergencyMeetings + (isPlus ? 1 : -1), 0, 9);

        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_EmergencyMeetingsCoolDown))]
    private int _emergencyMeetingsCoolDown;
    [SerializeField] private Text _emergencyMetingsCoolDownText;
    private void Hook_EmergencyMeetingsCoolDown(int _, int emergencyMeetingsCoolDown)
    {
        _emergencyMetingsCoolDownText.text = string.Format("{0}s", emergencyMeetingsCoolDown);
        UpdateGameRuleOverview();
    }

    public void OnChangeEmergencyMeetingsCoolDown(bool isPlus)
    {
        _emergencyMeetingsCoolDown = Mathf.Clamp(_emergencyMeetingsCoolDown + (isPlus ? 5 : -5), 0, 60);
        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_MeetingTime))]
    private int _meetingTime;
    [SerializeField] private Text _meetingTimeText;
    private void Hook_MeetingTime(int _, int meetingTime)
    {
        _meetingTimeText.text = string.Format("{0}s", meetingTime);
        UpdateGameRuleOverview();
    }

    public void OnChangeMeetingTime(bool isPlus)
    {
        _meetingTime = Mathf.Clamp(_meetingTime + (isPlus ? 5 : -5), 0, 120);
        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_VoteTime))]
    private int _voteTime;
    [SerializeField] private Text _voteTimeText;
    private void Hook_VoteTime(int _, int voteTime)
    {
        _voteTimeText.text = string.Format("{0}s", voteTime);
        UpdateGameRuleOverview();
    }

    public void OnChangeVoteTime(bool isPlus)
    {
        _voteTime = Mathf.Clamp(_voteTime + (isPlus ? 5 : -5), 0, 300);

        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_AnonyMousVotes))]
    private bool _anonymousVotes = true;
    public bool AnonymousVotes { get { return _anonymousVotes; } }
    [SerializeField] private Toggle _anonymousVotesToggle;
    private void Hook_AnonyMousVotes(bool _, bool value)
    {
        _anonymousVotesToggle.SetIsOnWithoutNotify(value);

        UpdateGameRuleOverview();
    }

    public void OnChangeAnonymousVotes(bool value)
    {
        _isRecommandRule = false;
        
        _anonymousVotes = value;
    }

    [SyncVar(hook = nameof(Hook_MoveSpeed))]
    private float _moveSpeed;
    [SerializeField] private Text _moveSpeedText;
    private void Hook_MoveSpeed(float _, float moveSpeed)
    {
        _moveSpeedText.text = string.Format("{0 : 0.0}x", moveSpeed);
        UpdateGameRuleOverview();
    }

    public void OnChangeMoveSpeed(bool isPlus)
    {
        _moveSpeed = Mathf.Clamp(_moveSpeed + (isPlus ? 0.25f : -0.25f), 0.5f, 3f);
        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_CrewSight))]
    private float _crewSight;
    [SerializeField] private Text _crewSightText;
    private void Hook_CrewSight(float _, float crewSight)
    {
        _crewSightText.text = string.Format("{0 : 0.0}x", crewSight);
        UpdateGameRuleOverview();
    }

    public void OnChangeCrewSight(bool isPlus)
    {
        _crewSight = Mathf.Clamp(_crewSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_ImposterSight))]
    private float _imposterSight;
    [SerializeField] private Text _imposterSightText;
    private void Hook_ImposterSight(float _, float imposterSight)
    {
        _imposterSightText.text = string.Format("{0 : 0.0}x", imposterSight);
        UpdateGameRuleOverview();
    }

    public void OnChangeImposterSight(bool isPlus)
    {
        _imposterSight = Mathf.Clamp(_imposterSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_KillCoolDown))]
    private float _killCoolDown;
    [SerializeField] private Text _killCoolDownText;
    private void Hook_KillCoolDown(float _, float killCoolDown)
    {
        _killCoolDownText.text = string.Format("{0 : 0.0}s", killCoolDown);
        UpdateGameRuleOverview();
    }

    public void OnChangeKillCoolDown(bool isPlus)
    {
        _killCoolDown = Mathf.Clamp(_killCoolDown + (isPlus ? 2.5f : -2.5f), 10f, 60f);
        _isRecommandRule = false;
    } 

    [SyncVar(hook = nameof(Hook_KillRange))]
    private KillRange _killRange;
    [SerializeField] private Text _killRangeText;
    private void Hook_KillRange(KillRange _,  KillRange killRange)
    {
        _killRangeText.text = killRange.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangeKillRange(bool isPlus)
    {
        _killRange = (KillRange)Mathf.Clamp((int)_killRange + (isPlus ? 1 : -1), 0, 2);
        _isRecommandRule = false;
    }

    [SyncVar(hook = nameof(Hook_VisualTasks))]
    private bool _visualTasks;
    public bool VisualTasks { get { return _visualTasks; }  }
    [SerializeField] private Toggle _visualTasksToggle;
    private void Hook_VisualTasks(bool _, bool visualTasks)
    {
        _visualTasksToggle.SetIsOnWithoutNotify(visualTasks);

        UpdateGameRuleOverview();
    }

    public void OnChangeVisualTasks(bool value)
    {
        _isRecommandRule = false;        
        _visualTasks = value;
    }

    [SyncVar(hook = nameof(Hook_TaskBarUpdates))]
    private TaskBarUpdates _taskBarUpdates = TaskBarUpdates.Default;
    [SerializeField] private Text _taskBarUpdatesText;
    private void Hook_TaskBarUpdates(TaskBarUpdates _, TaskBarUpdates taskBarUpdates)
    {
        _taskBarUpdatesText.text = taskBarUpdates.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangeTaskBarUpdates(bool isPlus)
    {
        _taskBarUpdates = (TaskBarUpdates)Mathf.Clamp((int)_taskBarUpdates + (isPlus ? 1 : -1), 0, 2);
        _isRecommandRule = false;
        _isRecommandRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(Hook_CommomTask))]
    private int _commomTask;
    [SerializeField] private Text _commomTaskText;
    private void Hook_CommomTask(int _, int commomTask)
    {
        _commomTaskText.text = commomTask.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangeCommomTask(bool isPlus)
    {
        _commomTask = Mathf.Clamp(_commomTask + (isPlus ? 1 : -1), 0, 2);
        _isRecommandRule = false;
        _isRecommandRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(Hook_ComplexTask))]
    private int _complexTask;
    [SerializeField] private Text _complexTaskText;
    private void Hook_ComplexTask(int _, int complexTask)
    {
        _complexTaskText.text = complexTask.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangeComplexTask(bool isPlus)
    {
        _complexTask = Mathf.Clamp(_complexTask + (isPlus ? 1 : -1), 0, 3);
        _isRecommandRule = false;
        _isRecommandRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(Hook_SimpleTask))]
    private int _simpleTask;
    [SerializeField] private Text _simpleTaskText;
    private void Hook_SimpleTask(int _, int simpleTask)
    {
        _simpleTaskText.text = simpleTask.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangeSimpleTask(bool isPlus)
    {
        _simpleTask = Mathf.Clamp(_simpleTask + (isPlus ? 1 : -1), 0, 5);
        _isRecommandRule = false;
        _isRecommandRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(Hook_ImposterCount))]
    private int _imposterCount;
    [SerializeField] private Text _imposterText;
    private void Hook_ImposterCount(int _, int imposterCount)
    {
        _imposterText.text = imposterCount.ToString();
        UpdateGameRuleOverview();
    }

    [SerializeField] private Text _gameRuleOverviewText;

    public void UpdateGameRuleOverview()
    {
        StringBuilder stringBuilder = new StringBuilder(_isRecommandRule ? "��õ ����\n" : "Ŀ���� ����\n");
        stringBuilder.AppendLine("�� : The Skeld");
        stringBuilder.AppendLine($"�������� : {_imposterCount}");
        //���� ������ ���� ������ Ŭ�󿡼��� CreateRoom�� ���� �ν��Ͻ��� �����͸� ���� �ʾұ� ������ ī��Ʈ�� 0��.
        //����(Host)�� ������ �� ���� �ν��Ͻ��� CreateRoom �Լ��� ���� �������� ���� �Ҵ��ϰ� �����߱� ������ ������ �ν��Ͻ����� ���� ����.
        //�� �޼���� Hook, �� Ŭ���̾�Ʈ���� ȣ��Ǳ� ������ Ŭ���̾�Ʈ ���� �ν��Ͻ��� �������͸� ���������� �ؼ� ������ �Ǵ°���. �������� ���� �Ҵ�(����ȭ)�������.
        stringBuilder.AppendLine(string.Format("Comfirm Ejects : {0}", _confirmEjects ? "����" : "����"));
        stringBuilder.AppendLine($"��� ȸ�� : {_emergencyMeetings}");
        stringBuilder.AppendLine(string.Format("Anonymous Votes : {0}", _anonymousVotes ? "����" : "����"));
        stringBuilder.AppendLine($"��� ȸ�� ��Ÿ�� : {_emergencyMeetingsCoolDown}");
        stringBuilder.AppendLine($"ȸ�� ���� �ð� : {_meetingTime}");
        stringBuilder.AppendLine($"��ǥ ���� �ð� : {_voteTime}");
        stringBuilder.AppendLine($"�̵� �ӵ� : {_moveSpeed}");
        stringBuilder.AppendLine($"ũ��� �þ� : {_crewSight}");
        stringBuilder.AppendLine($"�������� �þ� : {_imposterSight}");
        stringBuilder.AppendLine($"ų ��Ÿ�� : {_killCoolDown}");
        stringBuilder.AppendLine($"ų ���� : {_killRange}");
        stringBuilder.AppendLine($"Task Bar Updates : {_taskBarUpdates}");
        stringBuilder.AppendLine(string.Format("Visual Tasks : {0}", _visualTasks ? "����" : "����"));
        stringBuilder.AppendLine($"���� �ӹ� : {_commomTask}");
        stringBuilder.AppendLine($"������ �ӹ� : {_complexTask}");
        stringBuilder.AppendLine($"������ �ӹ� : {_simpleTask}");

        _gameRuleOverviewText.text = stringBuilder.ToString();
    }

    private void SetRecommandRule() //��õ �� 
    {
        _isRecommandRule = true;
        _confirmEjects = true;
        _emergencyMeetings = 1;
        _emergencyMeetingsCoolDown = 15;
        _meetingTime = 15;
        _voteTime = 120;
        _moveSpeed = 1f;
        _crewSight = 1f;
        _imposterSight = 1.5f;
        _killCoolDown = 45f;
        _killRange = KillRange.Normal;
        _visualTasks = true;
        _commomTask = 1;
        _complexTask = 1;
        _simpleTask = 2;
    }

    [Server]
    private void OnServerInitializeGameRuleStore()
    {
        var roomManager = AMONGUS_RoomManager.Instance;

        if(roomManager != null)
        {
            _imposterCount = roomManager.ImposterCount;
            _anonymousVotes = false;
            _taskBarUpdates = TaskBarUpdates.Always;

            SetRecommandRule();
        }
    }
    //SyncVar�� ���� ������� ������ NetworkBehaviour�� 
    //if (!SyncVarEqual(value, ref field)) ������ false�� �ǹǷ� ������� ����.

    private void Start()
    {
        if (isServer)
        {
            OnServerInitializeGameRuleStore();
        }
    }
}
