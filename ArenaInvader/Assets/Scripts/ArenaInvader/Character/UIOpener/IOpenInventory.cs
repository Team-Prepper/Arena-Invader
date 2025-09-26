public interface IOpenInventory
{

    public void Initial(IPlayableCharacter cc);
    public GUIInventory OpenInventory();
    public void ShowUseItem(string itemCode);
}