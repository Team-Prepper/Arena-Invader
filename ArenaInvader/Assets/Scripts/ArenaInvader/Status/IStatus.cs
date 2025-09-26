using System;
using System.Collections.Generic;

public interface IStatus : IObservable<IStatus>
{
    public Action OnDeathEvent { get; set; }

    public string Name { get; }
    public string CharacterCode { get; }

    void SetName(string name);
    void SetCharacter(string characterCode);

    public int HP { get; }
    public int Atk { get; }
    public int Dfs { get; }
    public int Money { get; }

    public bool IsAlive();

    public void LevelUp(int levelUpAmount);

    public void AddHP(int hp);
    public void TakeDamage(int damage);

    public void AddMoney(int money);
    public void UseMoney(int money);
    
    public void AddAtk(int atk);
    public void AddDfs(int dfs);
}