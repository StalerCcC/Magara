using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Parallax : MonoBehaviour
{
    private float lenght,startpos;
    public GameObject Cam;
    public float Parallax_Effect;   
    void Start()
    {
        startpos=transform.position.x;
        lenght= GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float temp = (Cam.transform.position.x*(1-Parallax_Effect));
        float dist =(Cam.transform.position.x*Parallax_Effect);
        transform.position = new Vector3(startpos+dist,transform.position.y,transform.position.z);
        if (temp>startpos+lenght)
        {
            startpos += lenght;

        }
        else if (temp<startpos-lenght)
        {
            startpos-=lenght;
        }
    }
}
