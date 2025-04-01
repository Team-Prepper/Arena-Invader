using System;
using UnityEngine;

namespace BoardGame
{
    public abstract class IMoveTo : MonoBehaviour
    {
        public abstract void MoveTo(Vector3 pos, Action callback);
    }
}