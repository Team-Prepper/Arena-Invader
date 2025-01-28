using System;
using System.Collections;
using System.Collections.Generic;
using EHTool;
using UnityEngine;

public class SwordAuraController : MonoSingleton<SwordAuraController>
{
    [SerializeField] GameObject[] _swordAura;
    
    public void SetSwordAura(int index)
    {
        for (int i = 0; i < _swordAura.Length; i++)
        {
            _swordAura[i].SetActive(i == index);
        }
        Invoke("ResetAura", 1.5f);
    }

    private void ResetAura()
    {
        foreach (var vfx in _swordAura)
        {
            vfx.SetActive(false);
        }
    }
}
