using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class EnergyProgress : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();

    public Sprite sp_empty = null;
    public Sprite sp_empty_red = null;

    public Image energy = null;
    public Image bg = null;

    public void SetEnergyProgress(float pro)
    {
        if (pro < 0.2f)
            pro = 0;
        if (pro > 0.95f)
            pro = 1f;
        int index = Mathf.FloorToInt(pro * (sprites.Count-1));
        energy.sprite = sprites[index];
        if (index <= 1)
            bg.sprite = sp_empty_red;
        else
            bg.sprite = sp_empty;
    }
}
