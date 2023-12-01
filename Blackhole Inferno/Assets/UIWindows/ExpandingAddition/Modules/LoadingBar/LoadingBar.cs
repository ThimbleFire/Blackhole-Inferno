using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public delegate void ExpansionCompleteHandler();
    public event ExpansionCompleteHandler LoadingComplete;

    public Text txtProgress;
    public RectTransform imgProgress;

    public void SetValue(float percent)
    {
        // Format the floating-point value as a string with two decimal places
        string formattedString = (percent * 100.0f).ToString("F2");
        formattedString = formattedString.PadLeft(5, '0');

        if (percent >= 0.99f)
        {
            txtProgress.text = "100.00% COMPLETE";
        }
        else
        {
            txtProgress.text = $"{formattedString}% COMPLETE";
        }

        imgProgress.sizeDelta = new Vector2( 285.2f * percent, 36);
    }
}
