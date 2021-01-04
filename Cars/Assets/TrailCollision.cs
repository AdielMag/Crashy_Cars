using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailCollision : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _threshold = .5f;
    [SerializeField] private float _timer = .5f;
    [SerializeField] private Vector2 _cubeSize;

    [HideInInspector] public CarController cCon;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private Vector3 lastDotPosition;
    public bool LastPointExists { get; set; }

    private void Start()
    {
        CreatePool();

        LastPointExists = false;
    }

    void Update()
    {
        if ((transform.position - lastDotPosition).magnitude > _threshold)
        {
            MakeADot(transform.position);
        }
    }

    private void CreatePool()
    {
        GameObject poolParent = new GameObject("PoolParent");
        //poolParent.transform.SetParent(transform.parent);

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        Queue<GameObject> ObjectsPool = new Queue<GameObject>();

        for (int i = 0; i < 50; i++)
        {
            GameObject obj = Instantiate(_prefab, poolParent.transform);
            obj.SetActive(false);
            ObjectsPool.Enqueue(obj);

            obj.AddComponent<TrailCollisionCube>()._collisionHandler = this;
        }

        poolDictionary.Add("TrailCube", ObjectsPool);
    }

    private void MakeADot(Vector3 newDotPosition)
    {
        GameObject dot = poolDictionary["TrailCube"].Dequeue();

        if (LastPointExists)
        {
            GameObject colliderKeeper = dot;
            colliderKeeper.transform.position =
                Vector3.Lerp(newDotPosition, lastDotPosition, 0.5f);
            colliderKeeper.transform.LookAt(newDotPosition);
            dot.transform.localScale =
                new Vector3(_cubeSize.x, _cubeSize.y,
                Vector3.Distance(newDotPosition, lastDotPosition));
        }
        lastDotPosition = newDotPosition;
        LastPointExists = true;

        dot.SetActive(true);

        poolDictionary["TrailCube"].Enqueue(dot);

        StartCoroutine(DotTimer(dot));
    }

    IEnumerator DotTimer(GameObject obj)
    {
        yield return new WaitForSeconds(_timer);
        obj.SetActive(false);
    }

    public void Colided(CarController hitCCon)
    {
        if (hitCCon == cCon)
            return;

        hitCCon.GotRammed(transform.position);
        cCon.CarRammedSuccefuly(hitCCon);
    }
}
