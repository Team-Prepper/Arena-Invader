using System;
using System.Collections.Generic;

public interface IStatus : IObservable<IStatus> {

    public string Name { get; set; }
    public string CharacterCode { get; set; }

    public int Money { get; set; }
    public int HP { get; set; }
    public int Atk { get; set; }
    public int Dfs { get; set; }
    public List<ItemData> Items { get; }
    
    public void SetCC(ICharacterController cc);
    public bool IsAlive();

    public void LevelUp(int levelUpAmount);
    public void UseItem(ItemData item);
    public void DiscardItem(ItemData item);
}