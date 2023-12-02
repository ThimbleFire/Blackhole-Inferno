using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : HUDSticker
{
    public class AttributeDistribution
    {
        private int[] attributes = new int[5];
     
        public AttributeDistribution()
        {
            RandomlyDistributePoints(100);
        }

        private void RandomlyDistributePoints(int totalPoints)
        {
            Random random = new Random();

            for (int i = 0; i < attributes.Length - 1; i++)
            {
                int randomPoints = random.Next(0, totalPoints + 1);
                attributes[i] = randomPoints;
                totalPoints -= randomPoints;
            }

            // Assign the remaining points to the last attribute
           attributes[attributes.Length - 1] = totalPoints;
        }
    }

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
