using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    // Start is called before the first frame update
    public static Sound SoundInstance;

    private void Awake()
    {
        if (SoundInstance != null && SoundInstance != this)
        {
            Destroy(this.gameObject);
            return;
        } ;
        SoundInstance = this;
        DontDestroyOnLoad(this);
    }
}
