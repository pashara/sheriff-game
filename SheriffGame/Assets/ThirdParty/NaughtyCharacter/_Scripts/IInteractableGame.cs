namespace NaughtyCharacter
{
    public interface IInteractableGame
    {
        bool CanInteract(Character character);
        void Interact(Character character);
    }
}