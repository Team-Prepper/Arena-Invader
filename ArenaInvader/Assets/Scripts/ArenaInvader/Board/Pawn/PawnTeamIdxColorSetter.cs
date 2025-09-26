using UnityEngine;

public class PawnTeamIdxColorSetter : PawnTeamIdxSetterBase
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Color[] _teamColor;
    
    public override void SetTeamIdx(int teamIdx)
    {
        _sprite.color = _teamColor[teamIdx];
    }
}