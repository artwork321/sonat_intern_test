using UnityEngine;
using DG.Tweening;

public class BottleController : MonoBehaviour
{
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
