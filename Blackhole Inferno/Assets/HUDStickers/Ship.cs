I'musing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
    public static Ship LPC;
    private HUDSticker interactingWithSticker = null;
    private LoadingBar bar;
    private bool finishedRotating = true;
    private bool finishedWarping = true;
    public float currentWarpSpeed = 0.0f;
    private float accelerationRate = 0.01f;
    private float accelerationRate2 = 0.01f;
    private float distanceAtTimeOfWarp = 0.0f;
    private float initialAngleToRotate = 0.0f;
    private bool warpAfterAlign = false;

    public UIExpandingAddition window;

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

        UpdateRotation();
        UpdateWarp();
    }

    public void UpdateWarp() {
        if (finishedWarping)
            return;

        float maximumWarpSpeed = 3.5f
        // Calculate the remaining distance
        float remainingDistance = Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition);
        // Calculate the percentage of travel completion
        float percentageCompletion = Mathf.Clamp01(1.0f - ((remainingDistance - interactingWithSticker.signatureRadius) / distanceAtTimeOfWarp));
        UpdateLoadingBar(percentageCompletion);

        float warpStep = Mathf.Clamp(currentWarpSpeed, 0.0f, maximumWarpSpeed) * Time.deltaTime;
        // Calculate the interpolation factor outside of Lerp for readability
        float lerpFactor = Mathf.Clamp01(warpStep / remainingDistance);

        absoluteWorldPosition = Vector3.Lerp(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition, lerpFactor);

        transform.position = absoluteWorldPosition;

        // Check if the warping is complete
        if (remainingDistance < interactingWithSticker.signatureRadius) {
            finishedWarping = true;
        }
        else {
            accelerationRate += accelerationRate2 * Time.deltaTime;
            currentWarpSpeed += accelerationRate * Time.deltaTime;
        }
    }

    public void UpdateRotation() {
        if (finishedRotating)
            return;
       
        float rotationSpeed = 3.5f;
        float rotLerp = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
        // Calculate the remaining angle to rotate
        float remainingAngle = Vector3.Angle(rot, interactingWithSticker.absoluteWorldPosition);
        // Perform the rotation
        rot = Vector3.Slerp(rot, interactingWithSticker.absoluteWorldPosition, rotLerp);
        // Calculate the percentage of rotation completion
        float percentageCompletion = Mathf.Clamp01(1.0f - (remainingAngle / initialAngleToRotate));
        UpdateLoadingBar(percentageCompletion);
        // Check if the rotation is complete
        float rotationThreshold = 0.1f;
        if (remainingAngle < rotationThreshold)
        {
            rot = interactingWithSticker.absoluteWorldPosition;
            finishedRotating = true;
            // if we initiate align because of a warp program, automatically perform warp after finished aligning
            if(warpAfterAlign) {
                SetWarpTo(interactingWithSticker);
                warpAfterAlign = false;
            }
        }
    }

    public void SetRotateTo(HUDSticker sticker) {
        if(!finishedRotating) {
            Debug.Log("Unable to align. Already aligning");
            return;
        }

        interactingWithSticker = sticker;
        finishedRotating = false;
        initialAngleToRotate = Vector3.Angle(rot, sticker.absoluteWorldPosition);

        window.Build(null, "PROGRAM: ALIGN", Color.red);
    }
    
    public void SetWarpTo(HUDSticker sticker) {
        if(!finishedWarping) {
            Debug.LogError("Unable to warp. Already warping");
            return;
        }

        if(rot != sticker.absoluteWorldPosition) {
            SetRotateTo(sticker);
            warpAfterAlign = true;
            return;
        }

        interactingWithSticker = sticker;
        finishedWarping = false;
        currentWarpSpeed = 0.0f;
        accelerationRate = 0.0f;

        // Calculate the initial distance without subtracting the signature radius
        distanceAtTimeOfWarp = Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) - interactingWithSticker.signatureRadius;

        window.Build(null, "PROGRAM: WARP", Color.red);
    }

    private void UpdateLoadingBar(float percentageCompletion) => window.loadingBar.SetValue(percentageCompletion);
}
