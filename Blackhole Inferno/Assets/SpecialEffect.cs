using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialEffect : MonoBehaviour
{
    private Animation animation;
    private Image image;

    void Awake() { 
        animation = GetComponent<Animation>();
        image = GetComponent<Image>();
    }

    public void Warp() {
        image.enabled = true;
        animation.Play("Jump");
    }

    public void AnimationComplete() {
        image.enabled = false;
    }
}
