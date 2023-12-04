using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
    public static Ship LPC;
    public float currentWarpSpeed;
    public float accelerationRate;
    public float accelerationRate2;
    public float maximumWarpSpeed;
    
    public List<Instruction> instructions = new List<Instruction>();
    public GameObject prefabExpandingAddition;

    public Vector3 rot;

    private static Dictionary<ContextMenuOption.Commands, Packet> packets;
    private delegate IEnumerator Packet();
    
    void Awake()
    {
        LPC = this;

        packets = new Dictionary<ContextMenuOption.Commands, Packet>()
        {
            {ContextMenuOption.Commands.Align, RotateToTargetCoroutine}, 
            {ContextMenuOption.Commands.WarpTo, WarpToTargetCoroutine}, 
            {ContextMenuOption.Commands.Dock, DockCoroutine}
        };
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
            InstructionStep();
        }
    }

    private IEnumerator DockCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;

        if(Vector3.Distance(sticker.absoluteWorldPosition, absoluteWorldPosition) > sticker.signatureRadius)
        {
                instructions.Insert(0, new Instruction(sticker, ContextMenuOption.Commands.WarpTo));
                InstructionStep();
                yield break;
        }

        UIExpandingAddition window = Instantiate(prefabExpandingAddition).GetComponentInChildren<UIExpandingAddition>();
        window.Build(null, "RUNNING PROGRAM: DOCK", Color.red);

        yield return new WaitForSeconds(5f);

        instructions.RemoveAt(0);
        GameObject.Destroy(window.transform.parent.gameObject);
    }

    private IEnumerator WarpToTargetCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;

        if(sticker.absoluteWorldPosition != rot)
        {
            instructions.Insert(0, new Instruction(sticker, ContextMenuOption.Commands.Align));
            InstructionStep();
            yield break;
        }

        // 1.0f = 1km
        currentWarpSpeed = 0.0f;
        accelerationRate = 100.0f;
        float distanceAtTimeOfWarp = Vector3.Distance(absoluteWorldPosition, sticker.absoluteWorldPosition) - sticker.signatureRadius;
        UIExpandingAddition window = Instantiate(prefabExpandingAddition).GetComponentInChildren<UIExpandingAddition>();
        window.Build(null, $"RUNNING PROGRAM: WARP", Color.red);
        window.enableLoadingBar = true;

        while ( instructions.Count > 0 && instructions[0].Command == ContextMenuOption.Commands.WarpTo )
        {
            float remainingDistance = Vector3.Distance(absoluteWorldPosition, sticker.absoluteWorldPosition);
            float percentageCompletion = Mathf.Clamp01(1.0f - ((remainingDistance - sticker.signatureRadius) / distanceAtTimeOfWarp));
            window.loadingBar.SetValue(percentageCompletion);
            float warpStep = Mathf.Clamp(currentWarpSpeed, 0.0f, maximumWarpSpeed) * Time.deltaTime;
            float lerpFactor = Mathf.Clamp01(warpStep / remainingDistance);
            absoluteWorldPosition = Vector3.Lerp(absoluteWorldPosition, sticker.absoluteWorldPosition, lerpFactor);
            transform.position = absoluteWorldPosition;
            if (remainingDistance < sticker.signatureRadius)
            {
                instructions.RemoveAt(0);
                GameObject.Destroy(window.transform.parent.gameObject);
                InstructionStep();
            }
            else if (currentWarpSpeed < maximumWarpSpeed )
            {
                accelerationRate += accelerationRate2 * Time.deltaTime;
                currentWarpSpeed += accelerationRate * Time.deltaTime;
            }

            yield return null;
        }
    }

    private void InstructionStep()
    {
        if(instructions.Count > 0)
            if(packets.TryGetValue(instructions[0].Command, out Packet packet)) {
                StartCoroutine(packet.Invoke());
            }
            else Debug.LogError("No Coroutine found for " + instructions[0].Command);
    }

    private IEnumerator RotateToTargetCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;

        float initialAngleToRotate = Vector3.Angle(rot, sticker.absoluteWorldPosition);
        float rotationSpeed = 3.5f;
        float rotationThreshold = 0.1f;
        UIExpandingAddition window = Instantiate(prefabExpandingAddition).GetComponentInChildren<UIExpandingAddition>();
        window.Build(null, "RUNNING PROGRAM: ALIGN", Color.red);
        window.enableLoadingBar = true;

        while ( instructions.Count > 0 && instructions[0].Command == ContextMenuOption.Commands.Align )
        {
            float rotLerp = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
            rot = Vector3.Slerp(rot, sticker.absoluteWorldPosition, rotLerp);
            float remainingAngle = Vector3.Angle(rot, sticker.absoluteWorldPosition);

            float percentageCompletion = Mathf.Clamp01(1.0f - (remainingAngle / initialAngleToRotate));
            window.loadingBar.SetValue(percentageCompletion);
  
            if (remainingAngle < rotationThreshold) {
                rot = sticker.absoluteWorldPosition;
                instructions.RemoveAt(0);
                GameObject.Destroy(window.transform.parent.gameObject);
                InstructionStep();
             }
             yield return null;
        }
    }

    // Vehicle speed
    // string speed = currentWarpSpeed > 150.0f ? 
    //     (currentWarpSpeed / 150.0f).ToString("F2").PadLeft(5, '0') + "AU" : 
    //     currentWarpSpeed.ToString("F2").PadLeft(5, '0') + "km";
}
