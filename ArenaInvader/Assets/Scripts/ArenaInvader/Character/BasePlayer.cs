using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    [SerializeField] private GamePawn[] _pawns;

    public IList<GamePawn> Pawns => _pawns;
    
}