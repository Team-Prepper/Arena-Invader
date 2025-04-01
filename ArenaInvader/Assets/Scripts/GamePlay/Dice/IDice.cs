using System;
using UnityEngine;

public abstract class IDice : MonoBehaviour {

    protected GUIDice _guiDice;

    public void SetGUIDice(GUIDice dice) {
        _guiDice = dice;
    }
    
    public abstract void Initial(int seed);
    public abstract void Roll(Action<int> callback);
    public abstract void Shot(float value, Action<int> callback = null);

}
