using System;
using System.Collections.Generic;

public interface IInventory {
    public Action OnItemsChanged { get; set; }

    public IList<string> Items { get; }

    public void SetCC(IPlayableCharacter cc);
    public void AddItem(string item);
    
    public void UseItem(string item);
    public void DiscardItem(string item);

}
