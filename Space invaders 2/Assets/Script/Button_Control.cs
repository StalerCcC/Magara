using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Control : MonoBehaviour
{
    public LayerMask player;
    public GameObject button_range;
    public GameObject Lazer;
    void Start()
    {
        
    }

    
    void Update()
    {
          Collider2D[] hit_enemies=Physics2D.OverlapCircleAll(Enemy_Attack_point.position,attack_range,roll_anim);
            foreach (Collider2D Enemy in hit_enemies)
            {
                if (Enemy.GetComponent<Player_Controller>().current_health>0)
                {
 
                    Enemy.GetComponent<Player_Controller>().Take_Damage(damage);
                    Enemy.GetComponent<Player_Controller>().rb.AddForce(new Vector2(expulsion*expulsion_direction,0));
                    
                }
            
            } 
    }
}
