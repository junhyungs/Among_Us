using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] private Transform _back;
    [SerializeField] private Transform _front;

    public int GetSortingOrder(GameObject obj)
    {
        float objectDistance = Mathf.Abs(_back.position.y - obj.transform.position.y);
        float totalDistance = Mathf.Abs(_back.position.y - _front.position.y);

        return (int)Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objectDistance / totalDistance);
        //System.Int16.MinValue -> Int16의 최솟값. -32,768
        //System.Int16.MaxValue -> Int16의 최댓값. 32,767
        //objectDistance / totalDistance 비율계산.  최대거리에 따른 현재 오브젝트와의 비율.
    }
}
