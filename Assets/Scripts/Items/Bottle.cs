using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Bottle : MonoBehaviour
{
    public Material watermat;
    public SpriteRenderer spriteRenderer;

    public Stack<float> waterAmount = new Stack<float>();

    public Vector3 originalPosition;

    public float totalWaterAmount;

    public void Start()
    {
        if (watermat == null)
        {
            watermat = Instantiate(spriteRenderer.material);
            spriteRenderer.material = watermat;

            float a1 = watermat.GetFloat("_Amount1");
            float a2 = watermat.GetFloat("_Amount2");
            float a3 = watermat.GetFloat("_Amount3");
            float a4 = watermat.GetFloat("_Amount4");

            if (a1 > 0) waterAmount.Push(a1);
            if (a2 > 0) waterAmount.Push(a2);
            if (a3 > 0) waterAmount.Push(a3);
            if (a4 > 0) waterAmount.Push(a4);

            totalWaterAmount = a1 + a2 + a3 + a4;

            originalPosition = transform.position;
        }
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
            watermat.SetFloat("_Angle", transform.eulerAngles.z * Mathf.Deg2Rad * 0.90f);
        }

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
            BottleController.instance.source = this;
            transform.DOMoveY(1, 1).SetRelative();
        }
        // Undo selecting source bottle
        else if (BottleController.instance.source == this)
        {
            BottleController.instance.source = null;
            transform.DOMoveY(-1, 1).SetRelative();
        }
        // Check for destination bottle's validity and execute pour action
        else if ((BottleController.instance.source != null
                 && !isFull())
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
        float newAmount = waterAmount.Pop() + amount;
        waterAmount.Push(newAmount);

        watermat.SetFloat(amountName, newAmount);
        totalWaterAmount += amount;
    }

    public bool isFull()
    {
        return totalWaterAmount >= 1;
    }

    public bool IsEmpty()
    {
        return totalWaterAmount == 0;
    }
}
