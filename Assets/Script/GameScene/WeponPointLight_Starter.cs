using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WeponPointLight_Starter : MonoBehaviour
{
    private WaitForSeconds _waitTime;
    private List<WeaponPointLight> _childList;

    private void Start()
    {
        InitializeWeaponPointLightStarter();

        StartCoroutine(TurnOnPipeLightCoroutine());
    }

    private void InitializeWeaponPointLightStarter()
    {
        _waitTime = new WaitForSeconds(2f);

        AddList();
    }

    private void AddList()
    {
        _childList = new List<WeaponPointLight>();

        foreach(Transform childTransform in transform)
        {
            WeaponPointLight weaponPointLight = childTransform.GetComponent<WeaponPointLight>();

            if(weaponPointLight != null)
            {
                _childList.Add(weaponPointLight);
            }
        }
    }

    private IEnumerator TurnOnPipeLightCoroutine()
    {
        while (true)
        {
            yield return _waitTime;

            foreach(var child in _childList)
            {
                child.TrunOnLight();
            }
        }
    }
}
