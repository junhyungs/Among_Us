using UnityEngine;
using Mirror;
using System.Collections;
using UnityEngine.UI;

public class CharacterMove : NetworkBehaviour
{
    private SpriteRenderer _characterSpriteRenderer;
    private Animator _animator;

    private bool _isMoving;
    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            if (!value)
            {
                _animator.SetBool("isMove", value);
            }

            _isMoving = value;
        }
    }

    [SyncVar]
    private float _moveSpeed = 2f;

    [SyncVar(hook = nameof(PlayerColor_Hook))]
    public PlayerColorType CurrentPlayerColor;
    public void PlayerColor_Hook(PlayerColorType _, PlayerColorType newColor)
    {
        if(_ == newColor)
        {
            return;
        }

        if(_characterSpriteRenderer == null)
        {
            _characterSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        _characterSpriteRenderer.material.SetColor("_Player_Color", PlayerColor.GetPlayerColor(newColor));
    }
    //SyncVar는 서버에서 값이 변경되면 모든 클라에게 동기화를 해줌. 
    //그 과정에서 SyncVar에 등록된 Hook이 호출되고, 다른 클라의 Hook도 호출된다. 
    //다른 클라들은 서버에서 받은 값을 동기화하고 자신의 권한으로 Hook을 호출하지만,
    //변경을 요청한 클라이언트의 값이 변경된거지 다른 클라의 값이 변경된건 아니기 때문에
    //다른 클라들은 매개변수로 둘 다 같은 값을 받게되고, hook은 호출되지만 결과적으로 색은 변경되지 않는다.

    [SerializeField]
    private Text _nameText;

    [SyncVar(hook = nameof(Hook_SetPlayerName))]
    public string _playerName;
    public void Hook_SetPlayerName(string _, string newName)
    {
        _nameText.text = newName;   
    }

    private void Awake()
    {
        if(_characterSpriteRenderer == null)
        {
            _characterSpriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        var material = Instantiate(_characterSpriteRenderer.material);
        _characterSpriteRenderer.material = material;
        _characterSpriteRenderer.material.SetColor("_Player_Color", PlayerColor.GetPlayerColor(CurrentPlayerColor));

        if (isOwned)
        {
            Camera mainCamera = Camera.main;
            mainCamera.transform.SetParent(transform);
            mainCamera.transform.localPosition = new Vector3(0f, 0f, -10f);
            mainCamera.orthographicSize = 2.5f;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if(isOwned && IsMoving)
        {
            bool isMove = false;

            if(PlayerSettings._controlType == ControlType.KeyboardMouse)
            {
                Vector3 moveDirection = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
                CommandFlipXAndMovement(moveDirection);
                RotateNameText(moveDirection);
                transform.position += moveDirection * _moveSpeed * Time.deltaTime;
                isMove = moveDirection.magnitude != 0f;
            }
            else 
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 moveDirection = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;
                    CommandFlipXAndMovement(moveDirection);
                    RotateNameText(moveDirection);
                    transform.position += moveDirection * _moveSpeed * Time.deltaTime;
                    isMove = moveDirection.magnitude != 0f;
                }
            }

            _animator.SetBool("isMove", isMove);
     
        }
    }

    private void RotateNameText(Vector3 moveDirection)
    {
        Quaternion rotation;

        if (moveDirection.x < 0f)
        {
            rotation = Quaternion.Euler(0f, 180f, 0f);

            _nameText.transform.rotation = rotation;
        }
        else if (moveDirection.x > 0f)
        {
            rotation = Quaternion.identity;

            _nameText.transform.rotation = rotation;
        }
    }

    #region Command&RPC

    [Command]
    private void CommandFlipXAndMovement(Vector3 moveDirection)
    {
        ClientRPCFlipXAndMovement(moveDirection);
    }

    [ClientRpc]
    public void ClientRPCFlipXAndMovement(Vector3 moveDirection)
    {
        if (moveDirection.x < 0f)
        {
            _characterSpriteRenderer.flipX = true;
        }
        else if (moveDirection.x > 0f)
        {
            _characterSpriteRenderer.flipX = false;
        }
    }

    public void RequestCommandSetPlayerColor(PlayerColorType colorType)
    {
        if (!isOwned)
        {
            return;
        }

        CommandSetPlayerColor(colorType);
    }

    [Command]
    private void CommandSetPlayerColor(PlayerColorType colorType)
    {
        CurrentPlayerColor = colorType;
    }

    #endregion
}
