using TMPro;
using UnityEngine;

public class MathGate : MonoBehaviour
{
    public string operationType;
    public float value;
    public TextMeshPro textDisplay;

    void Start()
    {
        if (textDisplay == null)
            textDisplay = GetComponentInChildren<TextMeshPro>();

        if (textDisplay != null)
            textDisplay.text = GetOperationDisplay();
    }

    string GetOperationDisplay()
    {
        switch (operationType)
        {
            case "multiply": return "×" + value;
            case "add": return "+" + value;
            case "subtract": return "-" + value;
            case "divide": return "÷" + value;
            default: return "?";
        }
    }
}
