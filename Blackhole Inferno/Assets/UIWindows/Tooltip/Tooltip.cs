using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public Transform ttTrasnsform;
    public static Tooltip instance;
    public TMP_Text text;

    void Awake() {
        if (instance == null) {
            instance = this;
            Hide();
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Set(string message)
    {
        text.text = message;
        text.enabled = true;
    }

    public void Hide()
    {
        text.enabled = false;
    }

    // This only updates while mousing over an entity
    private void Update() {
        ttTrasnsform.position = Input.mousePosition + Vector3.right * text.preferredWidth / 2;
    }
    
    public void OnAnimationComplete()
    {
        Debug.Log("context menu animation over");
    }
}
