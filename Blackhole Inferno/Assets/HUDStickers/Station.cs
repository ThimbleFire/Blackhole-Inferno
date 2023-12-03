using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Station : HUDSticker
{
    public int[] attributes = new int[5];
    private byte totalPoints = 100;

    void Start()
    { 
        CMOCommands = new List<ContextMenuOption.Commands>
        { 
            ContextMenuOption.Commands.Align,
            ContextMenuOption.Commands.WarpTo, 
            ContextMenuOption.Commands.Dock, 
            ContextMenuOption.Commands.LookAt, 
            ContextMenuOption.Commands.Examine
        };

        RandomizeAttributes();
    }

    private void RandomizeAttributes(){
        
        System.Random rand = new System.Random();

        for (int i = 0; i < attributes.Length - 1; i++)
        {
            int minPoints = totalPoints / (attributes.Length - i);
            int maxPoints = Mathf.Min(totalPoints, minPoints + 10); // Adjust the range as needed

            byte randomPoints = (byte)rand.Next(minPoints, maxPoints + 1);

            attributes[i] = randomPoints;
            totalPoints -= randomPoints;
        }

        attributes[attributes.Length - 1] = totalPoints;    
    }
}
