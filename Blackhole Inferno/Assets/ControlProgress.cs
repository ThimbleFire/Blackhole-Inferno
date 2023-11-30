using UnityEngine;
using UnityEngine.UI;

public class ControlProgress : MonoBehaviour
{
    public Image imgIcon;
    public Text txtProgram;
    public Text txtStatus;
    
    public RectTransform imgProgress;
    public Text txtProgress;

    public void RunProgram(string programName, Sprite sprite)
    {
        txtProgram.text = "PROGRAM: " + programName.ToUpper();
        imgIcon.sprite = sprite;
        txtProgress.text = "00.00% COMPLETE";
    }

    /// <summary>where 1.0f equals 100%</summary>
    public void Set(float percent, float warpSpeed = 0.0f)
    {
        // Format the floating-point value as a string with two decimal places
        string formattedString = (percent * 100.0f).ToString("F2");
        formattedString = formattedString.PadLeft(5, '0');

        txtProgress.text = percent > 0.99f ? "100.00% COMPLETE" : $"{formattedString}% COMPLETE";
        imgProgress.sizeDelta = new Vector2( 285.2f * percent, 36);
    
        if(warpSpeed != 0.0f)
            txtStatus.text = $"IN PROGRESS {warpSpeed} AU";
    }
}
