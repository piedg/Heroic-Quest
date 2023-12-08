public interface IInteractable
{
    bool IsInteractable { get; }

    void OnStartHover();
    void OnInteract();
    void OnEndHover();
}