namespace TheNecromancers.Gameplay.Interaction
{
    public interface IInteractable
    {
        bool IsInteractable { get; }

        void StartHover();
        void Interact();
        void EndHover();
    }
}