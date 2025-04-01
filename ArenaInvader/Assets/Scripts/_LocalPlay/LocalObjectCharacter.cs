public class LocalObjectCharacter : LocalStatus {
    
    public void SetTargetCharacter(string characterCode)
    {
        Name = characterCode;
        CharacterCode = characterCode;

        GameManager.Instance.Playground.ObjectCharacter = this;
        CharacterManager.Instance.SpawnPlayer(characterCode);
    }

}