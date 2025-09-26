using Unity.Netcode;

public class UNetObjectCharacter : UNetStatus {

    private void Start()
    { 
        GameManager.Instance.Playground.ObjectCharacter = this;
        
    }
    
    public void SetTargetCharacter(string characterCode)
    {
        SetName(characterCode);
        SetCharacter(characterCode);

        SetTargetCharacterClientRpc(characterCode);

        GameManager.Instance.Playground.ObjectCharacter = this;
    }

    [ClientRpc]
    void SetTargetCharacterClientRpc(string characterCode) {

        CharacterManager.Instance.SpawnPlayer(characterCode);

    }
}