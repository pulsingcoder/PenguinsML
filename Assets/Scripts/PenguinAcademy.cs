using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class PenguinAcademy : Academy
{
    private PenguinArea[] penguinAreas;
    
    public override void AcademyReset()
    {
        // Get the peguin areas;
        if (penguinAreas == null)
        {
            penguinAreas = FindObjectsOfType<PenguinArea>();

        }

        // set up the area

        foreach (PenguinArea penguinarea in penguinAreas)
        {
            penguinarea.fishSpeed = resetParameters["fish_speed"];
            penguinarea.feedRadius = resetParameters["feed_radius"];
            penguinarea.ResetArea();
        }
    }
    
}
