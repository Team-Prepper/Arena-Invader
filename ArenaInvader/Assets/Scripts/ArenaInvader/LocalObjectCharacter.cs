public class LocalObjectCharacter : Status {
    
    public void SetTargetCharacter(string characterCode)
    {
        SetName(characterCode);
        SetCharacter(characterCode);

        GameManager.Instance.Playground.ObjectCharacter = this;
        CharacterManager.Instance.SpawnPlayer(characterCode);
    }
    
}