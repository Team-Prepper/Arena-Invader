using System;
using System.Collections.Generic;

public interface IStatus : IObservable<IStatus> {

    public string Name { get; set; }
    public string CharacterCode { get; set; }

    public int Money { get; set; }
    public int HP { get; set; }
    public int Atk { get; }
    public int Dfs { get; }
    
    public bool IsAlive();

    public void LevelUp(int levelUpAmount);
    public void AddAtk(int atk);
    public void AddDfs(int dfs);
}