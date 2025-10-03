using UnityEngine;
using EasyH.Unity;
using EasyH.Unity.SoundKit;

public class MatchGenerator : MonoBehaviour
{
    [SerializeField] private MatchInfor _defaultMatchiInfor;
    private IMatchInfor _matchInfor;

    [SerializeField] private PlayableCharacter _localCharacterController;

    [SerializeField] private Transform[] _playerPosition;

    public void SetMatchInfor(IMatchInfor matchInfor)
    {
        _matchInfor = matchInfor;
    }

    public void GenerateMap()
    {
        if (_matchInfor == null)
        {
            _matchInfor = _defaultMatchiInfor;
        }
        ResourceManager.Instance.
            ResourceConnector.ImportComponent<GameMap>(
                GameManager.Instance.MatchInfor.MapName);
    }

    public void Generate()
    {
        if (_matchInfor == null)
        {
            _matchInfor = _defaultMatchiInfor;
        }

        for (int i = 0; i < _matchInfor.PlayerInfors.Count; i++)
        {
            IPlayableCharacter character = GameManager.Instance.
                Playground.InstantiateCC(
                    _playerPosition[i].position);

            character.TurnState.SetTeamIdx(i);

            character.Status.SetName(
                _matchInfor.PlayerInfors[i].Name);
            character.Status.SetCharacter(
                _matchInfor.PlayerInfors[i].CharacterCode);
            character.PawnOwner.SetInitial(
                _matchInfor.PlayerInfors[i].CharacterCode);

        }

        SoundManager.Instance.PlayBGM("1st");
        GameManager.Instance.Playground.MatchLoadComplete();

    }

}
