using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
    public static Ship LPC;
    public ControlProgress controlProgress;
    private HUDSticker interactingWithSticker = null;
    private bool finishedRotating = true;
    private bool finishedWarping = true;
    public float currentWarpSpeed = 0.0f;
    private float accelerationRate = 0.01f;
    private float accelerationRate2 = 0.01f;
    private float distanceAtTimeOfWarp = 0.0f;

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
        base.Update();

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
        else if (!finishedWarping)
        {
            // Move transform forward in the direction of rot
            float warpStep = Mathf.Clamp(currentWarpSpeed, 0.0f, 3.5f) * Time.deltaTime;

            // Calculate the remaining distance
            float remainingDistance = Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) - interactingWithSticker.signatureRadius;

            // Ensure remainingDistance is non-negative
            remainingDistance = Mathf.Max(remainingDistance, 0f);

            // Calculate the percentage of travel completion
            float percentageCompletion = Mathf.Clamp01(1.0f - (remainingDistance / distanceAtTimeOfWarp));


            absoluteWorldPosition = Vector3.Lerp(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition, warpStep);
            transform.position = absoluteWorldPosition;

            // Check if the warping is complete
            if (Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) < interactingWithSticker.signatureRadius)
                finishedWarping = true;
            else
            {
                accelerationRate += accelerationRate2 * Time.deltaTime;
                currentWarpSpeed += accelerationRate * Time.deltaTime;
            }

            controlProgress.Set(percentageCompletion, currentWarpSpeed);
        }
    }

    public void SetRotateTo(HUDSticker sticker) {
        if(!finishedRotating) {
            Debug.Log("Unable to align. Already aligning");
            return;
        }
        
        interactingWithSticker = sticker;
        finishedRotating = false;
    }
    
    public void SetWarpTo(HUDSticker sticker) {
        if(!finishedWarping) {
            Debug.Log("Unable to warp. Already warping");
            return;
        }

        interactingWithSticker = sticker;
        finishedWarping = false;
        currentWarpSpeed = 0.0f;
        accelerationRate = 0.0f;

        // Calculate the initial distance without subtracting the signature radius
        distanceAtTimeOfWarp = Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) - interactingWithSticker.signatureRadius;

        controlProgress.RunProgram("Warp", sticker.Sprite);
}
}
