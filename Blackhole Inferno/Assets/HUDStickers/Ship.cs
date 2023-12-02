using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
    public static Ship LPC;

    private Queue<Instruction> instructions = new Queue<Instruction>();
    private Dictionary<ContextMenuOptions.Command, Action> ActionDictionary = new Dictionary<ContextMenuOptions.Command, Action>();
    private LoadingBar bar;
    public UIExpandingAddition window;

    void Awake()
    {
        LPC = this;
        ActionDictionary.Add(ContextMenuOptions.Command.WarpTo, WarpToTargetCoroutine);
        ActionDictionary.Add(ContextMenuOptions.Command.Align,  RotateToTargetCoroutine);
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

    public void Instruct(Instruction instruction) {
        instructions.Add(instruction);
        if(instructions.Count == 1) {
            window.Build(null, "PROGRAM: " + instructions.Peek().Command.ToString().To upper(), Color.red);
            ActionDictionary[instructions.Peek().Command].Invoke(instructions.Peek().Sticker);
            instructions.Dequeue();
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator WarpToTargetCoroutine()
    {
        HUDSticker sticker = instructions.Peek().Sticker;

        float currentWarpSpeed = 0.0f;
        float accelerationRate = 0.01f;
        float accelerationRate2 = 0.01f;
        float distanceAtTimeOfWarp = 0.0f;
        float maximumWarpSpeed = 3.5f;
        float distanceAtTimeOfWarp = Vector3.Distance(absoluteWorldPosition, interactingWithSticker.absoluteWorldPosition) - interactingWithSticker.signatureRadius;

        while ( instructions.Peek().Command == ContextMenuOption.Commands.WarpTo )
        {
            float remainingDistance = Vector3.Distance(absoluteWorldPosition, sticker.absoluteWorldPosition);
            float percentageCompletion = Mathf.Clamp01(1.0f - ((remainingDistance - sticker.signatureRadius) / distanceAtTimeOfWarp));
            UpdateLoadingBar(percentageCompletion);
            float warpStep = Mathf.Clamp(currentWarpSpeed, 0.0f, maximumWarpSpeed) * Time.deltaTime;
            float lerpFactor = Mathf.Clamp01(warpStep / remainingDistance);
            absoluteWorldPosition = Vector3.Lerp(absoluteWorldPosition, sticker.absoluteWorldPosition, lerpFactor);
            transform.position = absoluteWorldPosition;
            if (remainingDistance < sticker.signatureRadius) {
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

    private IEnumerator RotateToTargetCoroutine()
    {
        HUDSticker sticker = instructions.Peek().Sticker;

        float initialAngleToRotate = Vector3.Angle(rot, sticker.absoluteWorldPosition);
        float rotationSpeed = 3.5f;
        float rotationThreshold = 0.1f;

        while ( instructions.Peek().Command == ContextMenuOption.Commands.AlignTo )
        {
            float rotLerp = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
            rot = Vector3.Slerp(rot, sticker.absoluteWorldPosition, rotLerp);
            float remainingAngle = Vector3.Angle(rot, sticker.absoluteWorldPosition);

            float percentageCompletion = Mathf.Clamp01(1.0f - (remainingAngle / initialAngleToRotate));
            UpdateLoadingBar(percentageCompletion);
  
            if (remainingAngle < rotationThreshold) {
                rot = sticker.absoluteWorldPosition;
                instructions.Dequeue();
             }
             yield return null;
        }
    }

    private void UpdateLoadingBar(float percentageCompletion) => window.loadingBar.SetValue(percentageCompletion);
}
