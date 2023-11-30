using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
    public static Ship LPC;
    private HUDSticker interactingWithSticker = null;
    private bool finishedRotating = true;
    private bool finishedWarping = true;

    void Awake()
    {
        LPC = this;
    }

    void Start()
    {
        signatureRadius = 65.0f;

        CMOCommands = new List<ContextMenuOption.Commands>
        { 
            ContextMenuOption.Commands.Align,
            //ContextMenuOption.Commands.Approach,
            ContextMenuOption.Commands.Lock,
            ContextMenuOption.Commands.LookAt, 
            ContextMenuOption.Commands.Examine,
        };
    }

    protected override void Update()
    {
        // rotate the theoretical direction to allow for accurate forward propulsion
        // transform rotation is reserved for facing the camera
        if (finishedRotating == false)
        {
            float rotationSpeed = 3.5f;
            float t = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
            rot = Vector3.Slerp(rot, interactingWithSticker.absoluteWorldPosition, t);

            // Check if the rotation is complete
            if (Vector3.Angle(rot, interactingWithSticker.absoluteWorldPosition) < 0.1f)
            {
                rot = interactingWithSticker.absoluteWorldPosition;
                finishedRotating = true;
                Debug.Log("aligned");
            }
        }
        else if (finishedWarping == false)
        {
            // Move transform forward in the direction of rot
            float warpSpeed = 3.5f;
            float warpStep = warpSpeed * Time.deltaTime;
            absoluteWorldPosition = Vector3.Lerp(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition, warpStep);
            transform.position = absoluteWorldPosition;
            
            Debug.Log(Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition));

            // Check if the warping is complete
            if (Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) < interactingWithSticker.signatureRadius) {
                finishedWarping = true;
            }
        }

        base.Update();
    }
    

    public void SetRotateTo(HUDSticker sticker) {
        interactingWithSticker = sticker;
        finishedRotating = false;
    }
    
    public void SetWarpTo(HUDSticker sticker) {
        interactingWithSticker = sticker;
        finishedWarping = false;
    }
}
