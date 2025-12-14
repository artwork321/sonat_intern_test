using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Bottle : MonoBehaviour
{
    public Material watermat;
    public SpriteRenderer spriteRenderer;

    public AnimationCurve scaleOffsetCurve;

    public AnimationCurve fillAmountCurve;

    public Stack<float> waterAmount = new Stack<float>();

    public Vector3 originalPosition;

    public float totalWaterAmount;

    private BoxCollider2D bottleCollider;

    private bool isAnimating = false;

    public void Awake()
    {
        if (watermat == null)
        {
            watermat = Instantiate(spriteRenderer.material);
            spriteRenderer.material = watermat;
        }
        bottleCollider = GetComponent<BoxCollider2D>();

        totalWaterAmount = 0;
    }

    public void Setup(BottleSettings settings)
    {
        for (int i = 0; i < settings.eachColor.Count; i++)
        {
            string colorName = "_Color" + (i + 1);
            watermat.SetColor(colorName, settings.eachColor[i]);
        }

        for (int i = 0; i < settings.eachWaterAmount.Count; i++)
        {
            string amountName = "_Amount" + (i + 1);
            float a = settings.eachWaterAmount[i];
            watermat.SetFloat(amountName, a);

            if (a > 0) waterAmount.Push(a);
            totalWaterAmount += a;
        }

        watermat.SetFloat("_VisibleAmount", totalWaterAmount);
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        OnClick();
    }

    public void Update()
    {
        if (watermat)
        {
            watermat.SetVector("_PosWorld", transform.position);
            watermat.SetFloat("_Bottom", spriteRenderer.bounds.min.y);
            watermat.SetFloat("_HeightImage", spriteRenderer.bounds.size.y);
        }

        float angle = transform.eulerAngles.z;

        if (angle > 180f)
        {
            angle = 360f - angle;
        }

        // if (scaleOffsetCurve != null) watermat.SetFloat("_ScaleOffset", scaleOffsetCurve.Evaluate(angle));
        if (fillAmountCurve != null) watermat.SetFloat("_VisibleAmount", fillAmountCurve.Evaluate(angle));

    }

    public void LockInteration()
    {
        bottleCollider.enabled = false;
    }

    public void UnlockInteration()
    {
        bottleCollider.enabled = true;
    }

    public void OnClick()
    {

        // Set source bottle
        if (BottleController.instance.source == null && IsEmpty())
        {
            return;
        }
        else if (BottleController.instance.source == null && !IsEmpty())
        {
            if (!IsComplete() && this != BottleController.instance.dest)
            {
                BottleController.instance.source = this;
                transform.DOMoveY(1, 1).SetRelative();
            }

        }
        // Undo selecting source bottle
        else if (BottleController.instance.source == this)
        {
            BottleController.instance.source = null;
            transform.DOMoveY(-1, 1).SetRelative();
        }

        // Check for destination bottle's validity and execute pour action
        else if ((BottleController.instance.source != null &&
                this.GetTopWaterColor() == BottleController.instance.source.GetTopWaterColor() && !isFull())
                || IsEmpty())
        {
            BottleController.instance.dest = this;
            BottleController.instance.PourOverDestination();
        }
    }

    public float GetTopWaterAmount()
    {
        if (totalWaterAmount == 0) return totalWaterAmount;
        return waterAmount.Peek();
    }

    public Color GetTopWaterColor()
    {
        if (totalWaterAmount == 0) return Color.clear;

        string colorName = "_Color" + waterAmount.Count;
        return watermat.GetColor(colorName);
    }

    public void RemoveTopWaterAmount(float amount)
    {
        string amountName = "_Amount" + waterAmount.Count;
        string colorName = "_Color" + waterAmount.Count;

        float topAmount = GetTopWaterAmount();

        // Update stack to reflect the amount of water in the bottle
        if (topAmount == amount)
        {
            // Remove all the top water
            waterAmount.Pop();
            watermat.SetColor(colorName, Color.clear);
        }
        else
        {
            // Decrease the top amount by amount
            waterAmount.Pop();
            waterAmount.Push(topAmount - amount);
        }

        watermat.SetFloat(amountName, topAmount - amount);
        totalWaterAmount -= amount;
        // watermat.SetFloat("_VisibleAmount", totalWaterAmount);
    }

    public void addTopWaterAmount(float amount, Color color)
    {
        // Pour new color if bottle is empty
        if (totalWaterAmount == 0)
        {
            watermat.SetColor("_Color1", color);
            waterAmount.Push(0);
        }

        string amountName = "_Amount" + waterAmount.Count;

        // Update stack to reflect the new top amount
        float startAmount = waterAmount.Pop();
        float newAmount = startAmount + amount;
        float newTotal = totalWaterAmount + amount;
        waterAmount.Push(newAmount);


        if (!isAnimating)
            StartCoroutine(AnimateWaterFill(startAmount, newAmount, newTotal, amountName));
    }

    private IEnumerator AnimateWaterFill(float startAmount, float endAmount, float newTotal, string amountName)
    {
        isAnimating = true;
        float elapsedTime = 0f;
        float volumeChange = endAmount - startAmount;
        float waterAmount = startAmount;

        while (elapsedTime < 1f)
        {
            waterAmount = Mathf.Lerp(startAmount, endAmount, elapsedTime / 1f);
            watermat.SetFloat(amountName, waterAmount);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        watermat.SetFloat(amountName, endAmount);
        totalWaterAmount = newTotal;
        isAnimating = false;
    }

    public bool isFull()
    {
        return totalWaterAmount >= 1;
    }

    public bool IsEmpty()
    {
        return totalWaterAmount == 0;
    }

    public bool IsComplete()
    {
        return waterAmount.Count == 1 && totalWaterAmount == 1;
    }

}
