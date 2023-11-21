using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volume : MonoBehaviour
{
    public AudioSource speak;
    public AudioSource shit;

    public float anim_time;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim_time+=Time.deltaTime;
        if (anim_time<6.2f&&anim_time>6.1f)
        {
            speak.Play();
        }
    }
}
