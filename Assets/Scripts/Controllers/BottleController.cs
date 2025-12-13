using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class BottleController : MonoBehaviour
{
    public GameObject bottlePrefab;
    public Bottle source;
    public Bottle dest;
    public List<Bottle> bottles;
    public static BottleController instance;


    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There are two BottleControllers!");
        }
        instance = this;
    }

    public void SetupBottles()
    {
        int numberOfBottles = GameManager.instance.settings.numberOfBottles;
        int numTop = (numberOfBottles > 4) ? Mathf.CeilToInt(numberOfBottles / 2) : numberOfBottles;
        float space = GameManager.instance.settings.width / (numTop - 1);
        Vector3 bottlePosition = new Vector3(-GameManager.instance.settings.width / 2, GameManager.instance.settings.height, 0);

        // Calculate positions for each bottle
        for (int i = 0; i < numTop; i++)
        {
            GameObject bottleObject = Instantiate(bottlePrefab, bottlePosition, transform.rotation, transform);
            Bottle bottle = bottleObject.GetComponent<Bottle>();
            bottle.Setup(GameManager.instance.settings.bottleSettings[i]);
            bottlePosition.x += space; // store as constant
            bottles.Add(bottle);
        }


        Vector3 bottombottlePosition = new Vector3(-GameManager.instance.settings.width / 2, -GameManager.instance.settings.height, 0);
        float bottomSpace = GameManager.instance.settings.width / (numberOfBottles - numTop - 1);

        for (int i = numTop; i < numberOfBottles; i++)
        {
            GameObject bottleObject = Instantiate(bottlePrefab, bottombottlePosition, transform.rotation, transform);
            Bottle bottle = bottleObject.GetComponent<Bottle>();
            bottle.Setup(GameManager.instance.settings.bottleSettings[i]);
            bottombottlePosition.x += bottomSpace; // store as constant
            bottles.Add(bottle);
        }
    }

    public void PourOverDestination()
    {
        Bottle tempSource = source;
        Bottle tempDest = dest;
        tempSource.LockInteration();

        source = null;

        // Calculate path from source to destination
        Vector3 destPosition = tempDest.transform.position;
        destPosition.y += tempDest.GetComponent<SpriteRenderer>().bounds.extents.y;
        destPosition.x -= tempDest.GetComponent<SpriteRenderer>().bounds.extents.y;

        float topAmount = tempSource.GetTopWaterAmount();
        Color topColor = tempSource.GetTopWaterColor();
        float pourAmount = (topAmount > 1 - tempDest.totalWaterAmount) ? 1 - tempDest.totalWaterAmount : topAmount;

        var seq = DOTween.Sequence();
        seq.Append(tempSource.transform.DOMove(destPosition, 1));
        seq.AppendCallback(() =>
        {
            tempSource.RemoveTopWaterAmount(pourAmount);
            tempDest.addTopWaterAmount(pourAmount, topColor);
        });
        seq.Append(tempSource.transform.DOMove(tempSource.originalPosition, 1));
        seq.AppendCallback(() =>
        {
            dest = null;
            tempSource.UnlockInteration();
            GameManager.instance.CheckEndGameCondition();
        });

    }

    public void DestroyBottles()
    {
        foreach (Bottle bottle in bottles)
        {
            Destroy(bottle.gameObject);
        }

        bottles.Clear();

        source = null;
        dest = null;
    }
}
