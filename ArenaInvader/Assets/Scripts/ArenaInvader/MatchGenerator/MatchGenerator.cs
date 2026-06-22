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

    private IMatchInfor ResolveMatchInfor()
    {
        if (_matchInfor != null)
        {
            return _matchInfor;
        }

        return _defaultMatchiInfor;
    }

    private bool HasEnoughPlayerSlots(IMatchInfor matchInfor)
    {
        return _playerPosition != null && matchInfor != null
            && _playerPosition.Length >= matchInfor.PlayerInfors.Count;
    }

    public void GenerateMap()
    {
        IMatchInfor matchInfor = ResolveMatchInfor();
        if (matchInfor == null)
        {
            throw new MissingReferenceException(
                $"{name} does not have match information to generate a map.");
        }

        ResourceManager.Instance.
            ResourceConnector.ImportComponent<GameMap>(
                matchInfor.MapName);
    }

    public void Generate()
    {
        IMatchInfor matchInfor = ResolveMatchInfor();
        if (matchInfor == null)
        {
            throw new MissingReferenceException(
                $"{name} does not have match information to generate players.");
        }

        if (!HasEnoughPlayerSlots(matchInfor))
        {
            throw new System.IndexOutOfRangeException(
                $"Player position count is smaller than player count ({matchInfor.PlayerInfors.Count}).");
        }

        for (int i = 0; i < matchInfor.PlayerInfors.Count; i++)
        {
            IPlayableCharacter character = GameManager.Instance.
                Playground.InstantiateCC(
                    _playerPosition[i].position);

            character.TurnState.SetTeamIdx(i);

            character.Status.SetName(
                matchInfor.PlayerInfors[i].Name);
            character.Status.SetCharacter(
                matchInfor.PlayerInfors[i].CharacterCode);
            character.PawnOwner.SetInitial(
                matchInfor.PlayerInfors[i].CharacterCode);

        }

        SoundManager.Instance.PlayBGM("1st");
        GameManager.Instance.Playground.MatchLoadComplete();

    }

}
