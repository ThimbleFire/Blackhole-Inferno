using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : HUDSticker
{
    void Start()
    {
        signatureRadius = 256.0f;

        CMOCommands = new List<ContextMenuOption.Commands>
        { 
            ContextMenuOption.Commands.Align,
            ContextMenuOption.Commands.WarpTo,
            ContextMenuOption.Commands.LookAt, 
            ContextMenuOption.Commands.Examine
        };
    }
}