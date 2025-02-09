using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    public static SpawnPositions instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Transform[] _spawnPostions;

    private int _index;

    public int Index => _index;

    public Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = _spawnPostions[_index++].position;

        if(_index >= _spawnPostions.Length)
        {
            _index = 0;
        }

        return spawnPosition;
    }
}
