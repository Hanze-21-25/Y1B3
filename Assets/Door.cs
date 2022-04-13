using UnityEngine;

public class Door : Interactable
{
    protected override void LaunchInteraction()
    {
        Debug.Log("Launched.");
    }
}
