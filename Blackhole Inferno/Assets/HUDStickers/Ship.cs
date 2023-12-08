using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
    public static Ship LPC;

    public HUDSticker currentLocation;

    public List<Instruction> instructions = new List<Instruction>();
    public GameObject prefabExpandingAddition;

    public Vector3 rot;

    private static Dictionary<ContextMenuOption.Commands, Packet> packets;
    private delegate IEnumerator Packet();
    
    protected override void Awake()
    {
        LPC = this;
        packets = new Dictionary<ContextMenuOption.Commands, Packet>() {
            {ContextMenuOption.Commands.Align, RotateToTargetCoroutine}, 
            {ContextMenuOption.Commands.WarpTo, WarpToTargetCoroutine}, 
            {ContextMenuOption.Commands.Dock, DockCoroutine},
            {ContextMenuOption.Commands.Jump, JumpCoroutine}
        };
        
        base.Awake();
    }

    void Start()
    {
        CMOCommands = new List<ContextMenuOption.Commands>
        {
            ContextMenuOption.Commands.Align,
            ContextMenuOption.Commands.Lock,
            ContextMenuOption.Commands.LookAt, 
            ContextMenuOption.Commands.Examine,
        };
    }

    public void Instruct(Instruction instruction)
    {
        instructions.Add(instruction);
        if(instructions.Count == 1)
        {
            StartCoroutine(InstructionStepCoroutine());
        }
    }

    void Update()
    {
        // Scale the ship depending on distance from the camera
        var size = (Camera.main.transform.position - transform.position).magnitude;
        float scale = 0.005f;
        transform.localScale = new Vector3(size,size,size) * scale;
    }

    void LateUpdate()
    {
        // Rotate the ship so its full image is visible to the camera
        // We can replace this with a call from the camera when the player performs a mouse drag / orbit
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    private IEnumerator DockCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;
        if(Vector3.Distance(sticker.worldPosition, worldPosition) > sticker.signatureRadius)
        {
            instructions.Insert(0, new Instruction(sticker, ContextMenuOption.Commands.WarpTo));
            StartCoroutine(InstructionStepCoroutine());
            yield break;
        }
        UIExpandingAddition window = Instantiate(prefabExpandingAddition).GetComponentInChildren<UIExpandingAddition>();
        window.Build(null, "RUNNING PROGRAM: DOCK", Color.red);
        window.OnDestroyEvent += OnWinDes;
        yield return new WaitForSeconds(5f);
        instructions.RemoveAt(0);
        GameObject.Destroy(window.transform.parent.gameObject);
    }

    private IEnumerator JumpCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;
        if(Vector3.Distance(sticker.worldPosition, worldPosition) > sticker.signatureRadius)
        {
            instructions.Insert(0, new Instruction(sticker, ContextMenuOption.Commands.WarpTo));
            StartCoroutine(InstructionStepCoroutine());
            yield break;
        }

        yield return null;
    }

    private IEnumerator WarpToTargetCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;

        // Ensure the player ship is aligned to the destination, if not, insert an align instruction
        if(sticker.worldPosition != rot) {
            instructions.Insert(0, new Instruction(sticker, ContextMenuOption.Commands.Align));
            StartCoroutine(InstructionStepCoroutine());
            yield break;
        }
        currentLocation.Leaving();

        // 
        float currentWarpSpeed = 2250f;
        float maximumWarpSpeed = 2250f;
        float distanceAtTimeOfWarp = Vector3.Distance(worldPosition, sticker.worldPosition);

        // Create the warp progress window
        UIExpandingAddition window = Instantiate(prefabExpandingAddition).GetComponentInChildren<UIExpandingAddition>();
        window.Build(null, "RUNNING PROGRAM: WARP", Color.red);
        window.enableLoadingBar = true;
        window.OnDestroyEvent += OnWinDes;

        while ( instructions.Count > 0 && instructions[0].Command == ContextMenuOption.Commands.WarpTo )
        {
            float remainingDistance = Vector3.Distance(worldPosition, sticker.worldPosition);
            float percentageCompletion = Mathf.Clamp01(1.0f - remainingDistance / distanceAtTimeOfWarp); // remaining distance - signature radius
            window.loadingBar.SetValue(percentageCompletion);
            float warpStep = Mathf.Clamp(currentWarpSpeed, 0.0f, maximumWarpSpeed) * Time.deltaTime;
            float lerpFactor = Mathf.Clamp01(warpStep / remainingDistance);
            worldPosition = Vector3.Lerp(worldPosition, sticker.worldPosition, lerpFactor);
            transform.position = worldPosition;

            if(remainingDistance <= 1000.0f)
                sticker.Arrived();        

            if (remainingDistance == 0.0f)
            {
                instructions.RemoveAt(0);
                GameObject.Destroy(window.transform.parent.gameObject);
                currentLocation = sticker;
                yield break;
            }

            yield return null;
        }
    }

    private void OnWinDes() => StartCoroutine(InstructionStepCoroutine());

    private IEnumerator InstructionStepCoroutine()
    {
        yield return null;
        if(instructions.Count > 0)
            if(packets.TryGetValue(instructions[0].Command, out Packet packet)) {
                StartCoroutine(packet.Invoke());
            }
            else Debug.LogError("No Coroutine found for " + instructions[0].Command);
    }

    private IEnumerator RotateToTargetCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;

        float initialAngleToRotate = Vector3.Angle(rot, sticker.worldPosition);
        float rotationSpeed = 3.5f;
        float rotationThreshold = 0.1f;
        UIExpandingAddition window = Instantiate(prefabExpandingAddition).GetComponentInChildren<UIExpandingAddition>();
        window.Build(null, "RUNNING PROGRAM: ALIGN", Color.red);
        window.enableLoadingBar = true;
        window.OnDestroyEvent += OnWinDes;

        while ( instructions.Count > 0 && instructions[0].Command == ContextMenuOption.Commands.Align )
        {
            float rotLerp = Mathf.Clamp01(Time.deltaTime * rotationSpeed);
            rot = Vector3.Slerp(rot, sticker.worldPosition, rotLerp);
            float remainingAngle = Vector3.Angle(rot, sticker.worldPosition);

            float percentageCompletion = Mathf.Clamp01(1.0f - (remainingAngle / initialAngleToRotate));
            window.loadingBar.SetValue(percentageCompletion);
  
            if (remainingAngle < rotationThreshold) {
                rot = sticker.worldPosition;
                instructions.RemoveAt(0);
                GameObject.Destroy(window.transform.parent.gameObject);
                yield break;
             }
             yield return null;
        }
    }
}
