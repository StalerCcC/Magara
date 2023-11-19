using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ground_Check : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag=="Platform")
        {
            Player_Controller.instance.jump=true;
            Player_Controller.instance.fall_audio=true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
