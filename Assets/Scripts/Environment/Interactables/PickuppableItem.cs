using UnityEngine;

public class PickuppableItem : MonoBehaviour, IInteractable
{
    public bool IsInteractable => isInteractable;
    private bool isInteractable = true;

    private void Start()
    {
        if (!isInteractable) { gameObject.SetActive(false); }
    }

    public void OnInteract()
    {
        if (!isInteractable) return;
        isInteractable = false;
        gameObject.SetActive(false);
    }

    public void OnStartHover()
    {
        if (!isInteractable) return;

        Debug.Log("OnStartHover");
    }

    public void OnEndHover()
    {
        if (!isInteractable) return;

        Debug.Log("OnEndHover");
    }
}
