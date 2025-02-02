using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineBody : MonoBehaviour
{
    [Header("Steam")]
    [SerializeField] private List<GameObject> _steamObjectList;

    [Header("Spark")]
    [SerializeField] private List<SpriteRenderer> _sparkSpriteRenderers;
    [SerializeField] private List<Sprite> _sparkSprites;

    private int _nowIndex;

    private void Start()
    {
        foreach(var steamObject in _steamObjectList)
        {
            StartCoroutine(StartRandomSteamCoroutine(steamObject));
        }

        StartCoroutine(SparkCoroutine());
    }

    private IEnumerator StartRandomSteamCoroutine(GameObject steamObject)
    {
        while (true)
        {
            float time = Random.Range(0.5f, 1.5f);
            float scale = Random.Range(1f, 2f);

            while(time >= 0f)
            {
                yield return null;

                time -= Time.deltaTime;
            }

            steamObject.SetActive(true);

            steamObject.transform.localScale = new Vector3(scale, scale, scale);

            time = 0f;

            while(time <= 0.6f)
            {
                yield return null;

                time += Time.deltaTime;
            }

            steamObject.SetActive(false);
        }
    }

    private IEnumerator SparkCoroutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.05f);

        while(true)
        {
            float time = Random.Range(0.2f, 1.5f);

            while(time >= 0f)
            {
                yield return null;
                time -= Time.deltaTime;
            }

            int[] tempArray = new int[Random.Range(2, 7)];

            for(int i = 0; i < tempArray.Length; i++)
            {
                tempArray[i] = Random.Range(0, _sparkSprites.Count);
            }

            for(int i = 0; i < tempArray.Length; i++)
            {
                yield return waitTime;

                _sparkSpriteRenderers[_nowIndex].sprite = _sparkSprites[tempArray[i]];
            }

            _sparkSpriteRenderers[_nowIndex++].sprite = null;

            if(_nowIndex >= _sparkSpriteRenderers.Count)
            {
                _nowIndex = 0;
            }
        }
    }
}
