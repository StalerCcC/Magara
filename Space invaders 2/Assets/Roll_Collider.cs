using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll_Collider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player_Controller.instance.rolling)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled=false;
        }
        else if (!Player_Controller.instance.rolling)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled=true;
        }
    }
}
