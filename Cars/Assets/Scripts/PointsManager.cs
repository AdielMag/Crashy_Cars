using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class PointsManager : MonoBehaviour
{
    public int points;
    public int pointsNeeded;

    [Space]
    public MoneyIndicator moneyIndicator;
    public Transform dollar;

    ObjectPooler objPool;

    bool isBot;
    private void Start()
    {
        isBot = !GetComponent<CarController>().joystick;

        if (dollar)
            dollar.localScale = Vector3.zero;

        objPool = ObjectPooler.instance;
    }

    public void AddPoints(int amount = 1)
    {
        points += amount;

        UpdateIndicator();

        if (!isBot && points >= pointsNeeded && LevelManager.instance)
        {
            LevelManager.instance.LevelCompleted();
        }
    }

    private void UpdateIndicator()
    {
        float targetValue = (float)points / (float)pointsNeeded;

        if(moneyIndicator)
            moneyIndicator.UpdateUI(targetValue);
        else if(dollar)
        {
            dollar.DOScale(1 * targetValue, 2);
        }
    }

    public void ThrowAllPoints(Vector3 lastPos)
    {
        for (int i = points; i > 0; i--)
        {
            Transform moneyCollectable =
                objPool.SpawnFromPool("Money Collectable",
                lastPos, Quaternion.identity).transform;

            float radius = 10;
            Vector3 throwDir = new Vector3(
                Random.Range(-radius, radius), 0, Random.Range(-radius, radius));

            Vector3 targetPos = new Vector3(moneyCollectable.position.x + throwDir.x, 0, moneyCollectable.position.z + throwDir.z);

            Sequence mSeq = DOTween.Sequence();

            float time = 1;
            mSeq.Append(moneyCollectable.DOJump(targetPos, Random.Range(1, 3f), 1, time));
        }

        points = 0;

        UpdateIndicator();
    }
}
