using UnityEngine;

public class ColorSelectButton : MonoBehaviour
{
    [SerializeField]
    public GameObject _xButtonImage;

    public bool _isInteractable = true;

    public void SetInteractable(bool isInteractable)
    {
        _isInteractable = isInteractable;

        _xButtonImage.SetActive(!isInteractable);
    }
}
