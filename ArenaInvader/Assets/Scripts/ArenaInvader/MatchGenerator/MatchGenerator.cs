using UnityEngine;
using EasyH.Unity;
using EasyH.Unity.SoundKit;

public class MatchGenerator : MonoBehaviour
{
    [SerializeField] private MatchInfo _defaultMatchInfo;
    private IMatchInfo _matchInfo;

    [SerializeField] private PlayableCharacter _localCharacterController;

    [SerializeField] private Transform[] _playerPosition;

    private void Awake()
    {
        EnsureDefaultMatchInfo();
    }

    public void EnsureDefaultMatchInfo()
    {
        if (GameManager.Instance == null || GameManager.Instance.MatchInfo != null)
        {
            return;
        }

        GameManager.Instance.MatchInfo = _defaultMatchInfo;
        GameManager.Instance.OnMatchInfoChanged?.Invoke();
    }

    public void SetMatchInfo(IMatchInfo matchInfo)
    {
        _matchInfo = matchInfo;
    }

    public MatchInfo GetDefaultMatchInfo()
    {
        return _defaultMatchInfo;
    }

    private IMatchInfo ResolveMatchInfo()
    {
        if (_matchInfo != null)
        {
            return _matchInfo;
        }

        return _defaultMatchInfo;
    }

    private bool HasEnoughPlayerSlots(IMatchInfo matchInfo)
    {
        return _playerPosition != null && matchInfo != null
            && _playerPosition.Length >= matchInfo.PlayerInfors.Count;
    }

    public void GenerateMap()
    {
        IMatchInfo matchInfo = ResolveMatchInfo();
        if (matchInfo == null)
        {
            throw new MissingReferenceException(
                $"{name} does not have match information to generate a map.");
        }

        ResourceManager.Instance.
            ResourceConnector.ImportComponent<GameMap>(
                matchInfo.MapName);
    }

    public void Generate()
    {
        IMatchInfo matchInfo = ResolveMatchInfo();
        if (matchInfo == null)
        {
            throw new MissingReferenceException(
                $"{name} does not have match information to generate players.");
        }

        if (!HasEnoughPlayerSlots(matchInfo))
        {
            throw new System.IndexOutOfRangeException(
                $"Player position count is smaller than player count ({matchInfo.PlayerInfors.Count}).");
        }

        for (int i = 0; i < matchInfo.PlayerInfors.Count; i++)
        {
            IPlayableCharacter character = GameManager.Instance.
                Playground.InstantiateCC(
                    _playerPosition[i].position);

            character.TurnState.SetTeamIdx(i);

            character.Status.SetName(
                matchInfo.PlayerInfors[i].Name);
            character.Status.SetCharacter(
                matchInfo.PlayerInfors[i].CharacterCode);
            character.PawnOwner.SetInitial(
                matchInfo.PlayerInfors[i].CharacterCode);

        }

        SoundManager.Instance.PlayBGM("1st");
        GameManager.Instance.Playground.MatchLoadComplete();

    }

}
