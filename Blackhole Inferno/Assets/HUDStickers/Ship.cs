using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
    public static Ship LPC;

    private Queue<Instruction> instructions = new Queue<Instruction>();

    private LoadingBar bar;

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
    }

    private IEnumerator WarpToTargetCoroutine()
    {
        float currentWarpSpeed = 0.0f;
        float accelerationRate = 0.01f;
        float accelerationRate2 = 0.01f;
        float distanceAtTimeOfWarp = 0.0f;
        float maximumWarpSpeed = 3.5f;

        while (instructions.Peek().Instruction == ContextMenuOption.Commands.WarpTo )
        {
            float remainingDistance = Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition);

            float percentageCompletion = Mathf.Clamp01(1.0f - ((remainingDistance - interactingWithSticker.signatureRadius) / distanceAtTimeOfWarp));

            UpdateLoadingBar(percentageCompletion);

            float warpStep = Mathf.Clamp(currentWarpSpeed, 0.0f, maximumWarpSpeed) * Time.deltaTime;

            float lerpFactor = Mathf.Clamp01(warpStep / remainingDistance);

            absoluteWorldPosition = Vector3.Lerp(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition, lerpFactor);

            transform.position = absoluteWorldPosition;

            if (remainingDistance < interactingWithSticker.signatureRadius)
            {
                instructions.Dequeue();
            }
            else
            {
                accelerationRate += accelerationRate2 * Time.deltaTime;
                currentWarpSpeed += accelerationRate * Time.deltaTime;
            }

            yield return null;
        }
    }

    public void SetWarpTo(HUDSticker sticker)
    {
        if (!finishedWarping)
        {
            Debug.LogError("Unable to warp. Already warping");
            return;
        }
   
        if (rot != sticker.absoluteWorldPosition)
        {
            SetRotateTo(sticker);
            warpAfterAlign = true;
            return;
        }

        interactingWithSticker = sticker;
        finishedWarping = false;
        currentWarpSpeed = 0.0f;
        accelerationRate = 0.0f;

        distanceAtTimeOfWarp = Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) - interactingWithSticker.signatureRadius;

        window.Build(null, "PROGRAM: WARP", Color.red);

        StartCoroutine(WarpToTargetCoroutine());
    }

    private IEnumerator RotateToTargetCoroutine()
    {
        float initialAngleToRotate = 0.0f;
        float rotationSpeed = 3.5f;
        float rotationThreshold = 0.1f;

        while (instructions.Peek().Instruction == ContextMenuOption.Commands.AlignTo )
        {
            float rotLerp = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
            float remainingAngle = Vector3.Angle(rot, interactingWithSticker.absoluteWorldPosition);
            rot = Vector3.Slerp(rot, interactingWithSticker.absoluteWorldPosition, rotLerp);

            float percentageCompletion = Mathf.Clamp01(1.0f - (remainingAngle / initialAngleToRotate));
            UpdateLoadingBar(percentageCompletion);
  
            if (remainingAngle < rotationThreshold)
            {
                rot = interactingWithSticker.absoluteWorldPosition;
                instructions.Dequeue();
             }
             yield return null;
        }
    }

    public void SetRotateTo(HUDSticker sticker)
    {
        if (!finishedRotating)
        {
            Debug.Log("Unable to align. Already aligning");
            return;
        }

        interactingWithSticker = sticker;
        finishedRotating = false;
        initialAngleToRotate = Vector3.Angle(rot, sticker.absoluteWorldPosition);

        window.Build(null, "PROGRAM: ALIGN", Color.red);

        StartCoroutine(RotateToTargetCoroutine());
    }

    private void UpdateLoadingBar(float percentageCompletion) => window.loadingBar.SetValue(percentageCompletion);
}
