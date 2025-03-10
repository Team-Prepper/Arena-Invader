using Unity.Netcode;

public class UNetObjectCharacter : UNetStatus {
    public void SetTargetCharacter(string characterCode)
    {
        Name = characterCode;
        CharacterCode = characterCode;
        
        SetTargetCharacterClientRpc(characterCode);

        GameManager.Instance.Playground.ObjectCharacter = this;
    }

    [ClientRpc]
    void SetTargetCharacterClientRpc(string characterCode) {

        Name = characterCode;
        CharacterCode = characterCode;

        GameManager.Instance.Playground.ObjectCharacter = this;
        CharacterManager.Instance.SpawnPlayer(characterCode);

    }
}