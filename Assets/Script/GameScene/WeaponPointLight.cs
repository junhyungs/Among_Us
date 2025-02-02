using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeaponPointLight : MonoBehaviour
{
    private Animator _animator;
    private WaitForSeconds _waitTime;
    private List<WeaponPointLight> _childList;

    private readonly int _onAnimation = Animator.StringToHash("On");

    private void Start()
    {
        InitializeWeaponPointLight();
    }

    private void InitializeWeaponPointLight()
    {
        _animator = GetComponent<Animator>();
        _waitTime = new WaitForSeconds(0.5f);

        AddList();
    }

    private void AddList()
    {
        _childList = new List<WeaponPointLight>();

        foreach(Transform childtransform in transform)
        {
            WeaponPointLight weaponPointLight = childtransform.GetComponent<WeaponPointLight>();

            if(weaponPointLight != null)
            {
                _childList.Add(weaponPointLight);
            }
        }
    }

    public void TrunOnLight()
    {
        _animator.SetTrigger(_onAnimation);

        StartCoroutine(TurnOnLightCoroutine());
    }

    private IEnumerator TurnOnLightCoroutine()
    {
        yield return _waitTime;

        foreach(var child in _childList)
        {
            child.TrunOnLight();
        }
    }
}
