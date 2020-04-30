using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SliderValueLabel : MonoBehaviour
{
    public TextMeshProUGUI ValueLabel;
    public ValueFormat Format;

    public enum ValueFormat
    {
        MONEY,
        CARBON
    }

    public void UpdateLabel(float value)
    {
        switch (Format)
        {
            case (ValueFormat.MONEY):
                ValueLabel.text = "$" + ((int)value).ToString();
                break;
            case (ValueFormat.CARBON):
                ValueLabel.text = ((int)value).ToString();
                break;
            default:
                Debug.LogError("Error: Unrecognized ValueFormat of value " + Format);
                break;
        }
    }
}
