using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Reflection;

/// <summary>
/// ExpandingAddition is a modular window designed to store other widow elements
/// </summary>
public class UIExpandingAddition : MonoBehaviour
{
    public enum Modules
    {
        LoadingBar
    };

    public bool enableLoadingBar { get; set; } = false;

    public Image symbol;
    public Text title;
    public Animation animation;

    public LoadingBar loadingBar;
    
    public delegate void OnDestroyAction();
    public event OnDestroyAction OnDestroyEvent;

    /// <summary> Initiate the animations that build the window</summary>
    public void Build(Sprite _symbol, string _title, Color color)
    {
        //symbol.sprite = _symbol;
        title.text = _title;
        title.color = color;
        
        animation.Play("ExpandHorizontalBounds");
    }
    
    public void OnExpandHorizontalBounds_Complete()
    {
        if(enableLoadingBar)
            loadingBar.gameObject.SetActive(true);

        // expand the loading bar
    }

    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }
}
