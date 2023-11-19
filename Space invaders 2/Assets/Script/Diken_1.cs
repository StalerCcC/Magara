using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Diken_1 : MonoBehaviour
{   
  
    public float forcex;
    public float forcey;
    public float damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            Player_Controller.instance.rb.AddForce(new Vector2(forcex,forcey));
            Player_Controller.instance.Take_Damage(damage);
        }
    }
}
