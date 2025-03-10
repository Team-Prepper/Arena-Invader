using System.Collections.Generic;

public interface IInventory {

    public IList<ItemData> Items { get; }

    public void SetCC(ICharacterController cc);
    
    public void UseItem(ItemData item);
    public void DiscardItem(ItemData item);

}