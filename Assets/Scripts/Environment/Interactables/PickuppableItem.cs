using HeroicQuest.Gameplay.Interaction;
using UnityEngine;

public class PickuppableItem : MonoBehaviour, IInteractable
{
    public bool IsInteractable => isInteractable;
    private bool isInteractable = true;

    private void Start()
    {
        if (!isInteractable) { gameObject.SetActive(false); }
    }

    public void Interact()
    {
        if (!isInteractable) return;
        isInteractable = false;
        gameObject.SetActive(false);
    }

    public void StartHover()
    {
        if (!isInteractable) return;

        Debug.Log("OnStartHover");
    }

    public void EndHover()
    {
        if (!isInteractable) return;

        Debug.Log("OnEndHover");
    }
}
