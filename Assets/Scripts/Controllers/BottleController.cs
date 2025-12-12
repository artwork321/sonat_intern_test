using UnityEngine;
using DG.Tweening;

public class BottleController : MonoBehaviour
{
    public GameObject bottlePrefab;
    public Bottle source;
    public Bottle dest;
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
        Debug.Log(numberOfBottles);
        float numTop = (numberOfBottles > 4) ? Mathf.Ceil(numberOfBottles / 2) : numberOfBottles;
        float space = GameManager.instance.settings.width / (numTop - 1);
        Vector3 bottlePosition = new Vector3(-GameManager.instance.settings.width / 2, GameManager.instance.settings.height, 0);

        // Calculate positions for each bottle
        for (int i = 0; i < numTop; i++)
        {
            GameObject bottleObject = Instantiate(bottlePrefab, bottlePosition, transform.rotation, transform);
            Bottle bottle = bottleObject.GetComponent<Bottle>();
            bottle.Setup(GameManager.instance.settings.bottleSettings[i]);
            bottlePosition.x += space; // store as constant
        }


        Vector3 bottombottlePosition = new Vector3(-GameManager.instance.settings.width / 2, -GameManager.instance.settings.height, 0);
        float bottomSpace = GameManager.instance.settings.width / (numberOfBottles - numTop - 1);

        for (int i = 0; i < numberOfBottles - numTop; i++)
        {
            GameObject bottleObject = Instantiate(bottlePrefab, bottombottlePosition, transform.rotation, transform);
            Bottle bottle = bottleObject.GetComponent<Bottle>();
            bottle.Setup(GameManager.instance.settings.bottleSettings[i]);
            bottombottlePosition.x += bottomSpace; // store as constant
        }
    }

    public void PourOverDestination()
    {
        // Calculate path from source to destination
        Vector3 destPosition = dest.transform.position;
        destPosition.y += dest.GetComponent<SpriteRenderer>().bounds.extents.y;
        destPosition.x -= dest.GetComponent<SpriteRenderer>().bounds.extents.y;

        float topAmount = source.GetTopWaterAmount();
        Color topColor = source.GetTopWaterColor();
        float pourAmount = (topAmount > 1 - dest.totalWaterAmount) ? 1 - dest.totalWaterAmount : topAmount;

        var seq = DOTween.Sequence();
        seq.Append(source.transform.DOMove(destPosition, 1));
        seq.AppendCallback(() =>
        {
            source.RemoveTopWaterAmount(pourAmount);
            dest.addTopWaterAmount(pourAmount, topColor);
        });
        seq.Append(source.transform.DOMove(source.originalPosition, 1));
        seq.AppendCallback(() =>
        {
            source = null;
            dest = null;
        });
    }
}
