using UnityEngine;
using EasyH.Gaming.TurnBased;

[DisallowMultipleComponent]
public class PlayableCharacterTurnState : MonoBehaviour
{
    private ICharacterController _selector;
    private int _chance;
    private int _extraDicePoint;

    public void SetController(ICharacterController selector)
    {
        _selector = selector;
    }

    public bool HasController()
    {
        return _selector != null;
    }

    public void StartTurn(IPlayableCharacter character)
    {
        _selector?.StartTurn(character);
    }

    public void StartRollDice()
    {
        _selector?.RollDice();
    }

    public void BeginTurn(IPlayableCharacter character)
    {
        if (_selector == null)
        {
            return;
        }

        _chance++;
        _selector.StartTurn(character);
    }

    public void EndTurn(IMemberState turnState, IPlayableCharacter character)
    {
        if (_selector == null || _chance <= 0)
        {
            turnState.EndTurn();
            return;
        }

        _selector.StartTurn(character);
    }

    public void AddChance()
    {
        _chance++;
    }

    public bool CanSpendChance()
    {
        return _chance > 0;
    }

    public void SpendChance()
    {
        _chance = Mathf.Max(0, _chance - 1);
    }

    public void AddExtraDicePoint(int point)
    {
        _extraDicePoint += point;
    }

    public int GetExtraDicePoint()
    {
        return _extraDicePoint;
    }

    public int ConsumeExtraDicePoint()
    {
        int value = _extraDicePoint;
        _extraDicePoint = 0;
        return value;
    }
}
