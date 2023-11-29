using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : HUDSticker
{
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
}
