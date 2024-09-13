namespace InfinityCraft.Interaction
{
    public interface IInteractable
    {
        string Name { get; set; }

        void Interact();
    }
}