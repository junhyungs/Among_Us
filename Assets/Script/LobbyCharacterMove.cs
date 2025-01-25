using UnityEngine;

public class LobbyCharacterMove : CharacterMove
{
    public void OnCompleteSpawn()
    {
        if (isOwned)
        {
            _isMoving = true;
        }
    }
}
