using UnityEngine;
using UnityEngine.UI;


public class ContextMenuOption : MonoBehaviour
{    
    public enum Commands {
        Align, Orbit, Dock, Approach, WarpTo, LookAt, Examine, Lock, Jump
    }

    public Text text;

    public void SetText(string message) => text.text = message;
}
