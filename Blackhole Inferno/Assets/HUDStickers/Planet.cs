using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : HUDSticker
{
    void Start()
    {
        CMOCommands = new List<ContextMenuOption.Commands>
        { 
            ContextMenuOption.Commands.Align,
            ContextMenuOption.Commands.WarpTo,
            ContextMenuOption.Commands.LookAt, 
            ContextMenuOption.Commands.Examine
        };
    }
}