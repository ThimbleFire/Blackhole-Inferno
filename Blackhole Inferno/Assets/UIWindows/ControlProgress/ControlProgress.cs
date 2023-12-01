using UnityEngine;
using UnityEngine.UI;

public class ControlProgress : MonoBehaviour
{
    public RectTransform cpTransform;
    public Image imgIcon;
    public Text txtProgram;
    public Text txtStatus;
    
    public RectTransform imgProgress;
    public Text txtProgress;

    public UIToggle uIToggle;

    private void Awake() {
        uIToggle.Disable();
    }

    public void RunProgram(string programName, Sprite sprite)
    {
        cpTransform.gameObject.SetActive(true);
        txtProgram.text = "PROGRAM: " + programName.ToUpper();
        imgIcon.sprite = sprite;
        txtProgress.text = "00.00% COMPLETE";
        txtStatus.text = $"IN PROGRESS";
        uIToggle.Enable();
    }

    public void Complete() {
        txtStatus.text = $"SHUTTING DOWN";
        uIToggle.Disable();
    }

    public void OnFadeOutComplete_SetInactive()
    {
        cpTransform.gameObject.SetActive(false);
    }

    /// <summary>where 1.0f equals 100%</summary>
    public void Set(float percent, float warpSpeed = 0.0f)
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
    
        if(warpSpeed != 0.0f)
            txtStatus.text = $"IN PROGRESS {warpSpeed} AU";
    }

    public void OnAnimationComplete()
    {
        Debug.Log("animation over");
    }
}
