using System.Collections.Generic;
using UnityEngine;

public class CrewFloater : MonoBehaviour
{
    [SerializeField] private GameObject _crewPrefab;
    [SerializeField] private List<Sprite> _sprites;

    private bool[] _isCrewStates = new bool[12];
    private float _timer;
    private float _distance = 11f;

    private void Start()
    {
        for(int i = 0; i < 12; i++)
        {
            SpawnFloatingCrew((PlayerColorType)i, Random.Range(0f, _distance));
        }
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            SpawnFloatingCrew((PlayerColorType)Random.Range(0f, 12f), _distance);
            _timer = 1f;
        }    
    }

    public void SpawnFloatingCrew(PlayerColorType colorType, float distance)
    {
        if (!_isCrewStates[(int)colorType])
        {
            _isCrewStates[(int)colorType] = true;

            float angle = Random.Range(0f, 360f);
            Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * distance;
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
            float floatingSpeed = Random.Range(1f, 4f);
            float rotateSpeed = Random.Range(-3f, 3f);

            var crew = Instantiate(_crewPrefab, spawnPos, Quaternion.identity).GetComponent<FloatingCrew>();
            crew.SetFloatingCrewData(_sprites[Random.Range(0, _sprites.Count)], colorType, direction, floatingSpeed, rotateSpeed, Random.Range(0.5f, 1f));
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        var crew = collider2D.GetComponent<FloatingCrew>();
        if (crew != null)
        {
            _isCrewStates[(int)crew.ColorType] = false;
            Destroy(crew.gameObject);
        }
    }
}
