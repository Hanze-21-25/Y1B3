using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    private bool isPressed { get; set; }

    private void Start()
    {
        isPressed = false;
    }

    public void Switch()
    {
        isPressed = !isPressed;
        Debug.Log("Switched");
        LaunchInteraction();
    }

    protected abstract void LaunchInteraction();
}
