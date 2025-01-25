using UnityEngine;
using Mirror;

public class CharacterMove : NetworkBehaviour
{
    private SpriteRenderer _characterSpriteRenderer;
    private Animator _animator;

    public bool _isMoving;
    [SyncVar]
    private float _moveSpeed = 2f;

    private void Awake()
    {
        _characterSpriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
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
        if(isOwned && _isMoving)
        {
            bool isMove = false;

            if(PlayerSettings._controlType == ControlType.KeyboardMouse)
            {
                Vector3 moveDirection = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
                CommandFlipXAndMovement(moveDirection);
                transform.position += moveDirection * _moveSpeed * Time.deltaTime;
                isMove = moveDirection.magnitude != 0f;
            }
            else 
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 moveDirection = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;
                    CommandFlipXAndMovement(moveDirection);
                    transform.position += moveDirection * _moveSpeed * Time.deltaTime;
                    isMove = moveDirection.magnitude != 0f;
                }
            }

            _animator.SetBool("isMove", isMove);
        }
    }

    [Command]
    private void CommandFlipXAndMovement(Vector3 moveDirection)
    {
        ClientRPCFlipXAndMovement(moveDirection);
    }

    [ClientRpc]
    private void ClientRPCFlipXAndMovement(Vector3 moveDirection)
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
}