using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource Audio;
    public static ExplosionSound ExplosionInstance;
    public AudioClip Click;
    private void Awake()
    {
        if (ExplosionInstance != null && ExplosionInstance != this)
        {
            Destroy(this.gameObject);
            return;
        } ;
        ExplosionInstance = this;
        DontDestroyOnLoad(this);
    }
}
