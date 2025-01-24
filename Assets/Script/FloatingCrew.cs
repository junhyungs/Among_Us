using UnityEngine;

public class FloatingCrew : MonoBehaviour
{
    public PlayerColorType ColorType;

    private SpriteRenderer _spriteRenderer;
    private Vector3 _moveDirection;
    private float _moveSpeed;
    private float _rotateSpeed;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFloatingCrewData(Sprite sprite, PlayerColorType colorType,
        Vector3 moveDirection, float moveSpeed, float rotateSpeed, float size)
    {
        ColorType = colorType;
        _moveDirection = moveDirection;
        _moveSpeed = moveSpeed;
        _rotateSpeed = rotateSpeed;

        _spriteRenderer.sprite = sprite;
        _spriteRenderer.material.SetColor("_Player_Color", PlayerColor.GetPlayerColor(colorType));

        transform.localScale = new Vector3(size, size, size);
        _spriteRenderer.sortingOrder = (int)Mathf.Lerp(1, 32767, size);
    }

    //transform.rotation -> Quaternion. (x,y,z,w)로 구성. 
    //transform.rotation.eulerAngles -> Vector3. (x,y,z)로 구성. 각각의 축을 기준으로 몇 도 회전했는지를 나타낸다. (즉 현재 회전값)
    private void Update()
    {
        transform.position += _moveDirection * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, _rotateSpeed));
    }
}
