using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : HUDSticker
{
    void Start()
    {
        signatureRadius = 65.0f;

        CMOCommands = new List<ContextMenuOption.Commands>
        { 
            ContextMenuOption.Commands.Align,
            ContextMenuOption.Commands.WarpTo, 
            ContextMenuOption.Commands.Dock, 
            ContextMenuOption.Commands.LookAt, 
            ContextMenuOption.Commands.Examine
        };
    }
}
