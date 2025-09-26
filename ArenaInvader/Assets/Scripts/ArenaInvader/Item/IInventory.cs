using System.Collections.Generic;

public interface IInventory {

    public IList<string> Items { get; }

    public void SetCC(IPlayableCharacter cc);
    
    public void UseItem(string item);
    public void DiscardItem(string item);

}