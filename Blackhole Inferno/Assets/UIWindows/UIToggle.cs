using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    public Animation animation;
    private bool enabled = false;
    
    public void Enable() {
        if(enabled)
            return;
            
         animation.Play( "OnEnable" );
         enabled = true;
    }
    public void Disable() {
        if(!enabled)
            return;

        animation.Play( "OnDisable" );
        enabled = false;
    }
}
