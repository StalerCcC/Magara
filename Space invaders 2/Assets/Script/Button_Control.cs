using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Control : MonoBehaviour
{
    public LayerMask player;
    
    public GameObject button_range;
    public GameObject Lazer;

    public float Press_range;
    void Start()
    {
        
    }

    
    void Update()
    {
          Collider2D[] hit_enemies=Physics2D.OverlapCircleAll(button_range.transform.position,Press_range,player);
            foreach (Collider2D Enemy in hit_enemies)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Destroy(Lazer);
                }

                    
            }
             
    }
}
