using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSoundPlayer : MonoBehaviour
{
    public void PlaySound(string key) {
        SFXManager.Instance.PlaySFX(key);
    }
}
