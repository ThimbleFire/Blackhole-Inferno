using JetBrains.Annotations;
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
            {ContextMenuOption.Commands.Jump, JumpCoroutine},
            {ContextMenuOption.Commands.Lock, LockTargetCoroutine }
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

        // Create the warp progress window
        UIExpandingAddition window = Instantiate(prefabExpandingAddition).GetComponentInChildren<UIExpandingAddition>();
        window.Build(null, "RUNNING PROGRAM: WARP", Color.red);
        window.enableLoadingBar = true;
        window.OnDestroyEvent += OnWinDes;

        //
        Vector3 startPoint = worldPosition;
        Vector3 endPoint = Vector3.MoveTowards(sticker.worldPosition, startPoint, 1000);
        Vector3 endPoint2 = Vector3.MoveTowards(sticker.worldPosition, startPoint, sticker.signatureRadius);
        float accelaration = 250.0f;
        float maxSpeed = 2250.0f;

        float currentSpeed = 0.0f;
        float distance = Vector3.Distance(startPoint, endPoint);
        float currentDistance = 0.0f;

        bool arrived = false;

        while ( instructions.Count > 0 && instructions[0].Command == ContextMenuOption.Commands.WarpTo )
        {
            if (arrived == false)
            {
                currentSpeed += accelaration * Time.deltaTime;
                currentDistance += currentSpeed * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
                float t = Mathf.Clamp01(currentDistance / distance);
                window.loadingBar.SetValue(t);
                worldPosition = Vector3.Lerp(startPoint, endPoint, t);
                if (t >= 1.0f)
                {
                    sticker.Arrived();
                    arrived = true;
                    GameObject.Destroy(window.transform.parent.gameObject);
                    // setup deceleration phase
                    startPoint = worldPosition;
                    distance = Vector3.Distance(startPoint, endPoint2);
                    currentDistance = 0.0f;
                    currentSpeed = 100.0f;
                }
            }
            else
            {
                currentDistance += currentSpeed * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0.0f);
                float t = Mathf.Clamp01(currentDistance / distance);
                worldPosition = Vector3.Lerp(startPoint, endPoint2, t);
                if (t >= 1.0f)
                {
                    sticker.Arrived();
                    arrived = true;
                    currentDistance = 0f;
                    currentSpeed = 0f;
                    //
                    instructions.RemoveAt(0);
                    currentLocation = sticker;
                    yield break;
                }
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

    private IEnumerator LockTargetCoroutine()
    {
        HUDSticker sticker = instructions[0].Sticker;

        bool lockAcheived = false;
        float timer = 0.0f;

        while(lockAcheived == false)
        {
            timer += Time.deltaTime;

            if(timer >= sticker.signatureRadius) {
                lockAcheived = true;
            }
            
            yield return null;   
        }

        //sticker.LockTarget
        Debug.Log("Target Locked: " + sticker.name);
    }
}
