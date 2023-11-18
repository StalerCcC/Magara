using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Falling_Enemy_Controller : MonoBehaviour
{
    public LayerMask Main_Character;
    public LayerMask roll_anim;
    public static Falling_Enemy_Controller instance;
    public Rigidbody2D rb;
    public Transform Player;
    public Transform Enemy_Attack_point;
    Vector3 direction;
    Animator anim;
    public float max_health=100;
    public float current_health;
    public float attack_range;
    public float chasing_range;
    public float attacking_speed;
    public float damage;
    public float expulsion_direction;
    public float expulsion;
    public float attack_duration;
    public float current_attack_time;
    public float anim_time;
    public float wait_time;
    public bool  jump;
    public bool face_right;
    public bool is_attacking;
    public bool is_chasing;
    public bool attacking;
    public bool attack_anim;
    public bool can_jump;




    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        current_health=max_health;
        jump = true;
    }
    public void Attack()
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
    
    void Update()
    {
        if (!anim.GetBool("Enemy_Falled"))
        {
            rb.velocity=new Vector2(0,rb.velocity.y);
        }
            
        if (attacking)
        {
            current_attack_time+=Time.deltaTime;
        }
        if (current_attack_time>attack_duration)
        {
            attacking=false;
        }

        if (Math.Abs(Player.position.x-transform.position.x)<attack_range)
        {
            is_attacking=true;
            is_chasing=false;
        }
        if (is_attacking&&!attacking)
        {
            anim.SetTrigger("Hit");
            attack_anim=true;
            current_attack_time=0;
            attacking=true;
        }
        if (attack_anim==true)
        {
            anim_time+=Time.deltaTime;
        }
        if (anim_time>wait_time)
        {
            Attack();
            anim_time=0;
            attack_anim=false;
        }
        if (is_attacking)
        {
            Flap();
            anim.SetBool("Run",false);
            rb.velocity=new Vector2(0,rb.velocity.y);
        }

        if (transform.position.x>Player.position.x)
        {
            expulsion_direction=-1;
            
        }
        if (Player.position.x>transform.position.x)
        {
            expulsion_direction=1;
        }  

        if (Math.Abs(Player.position.x-transform.position.x)<chasing_range&&Math.Abs(Player.position.x-transform.position.x)>attack_range)
        {
            anim.SetBool("Enemy_Falled",true);
            anim.SetTrigger("Enemy_Falling");
            rb.gravityScale=5;
            if (anim.GetBool("Touch_ground"))
            {
                is_chasing=true;
                is_attacking=false;  
            }
            
        }

        if (is_chasing)
        {
            

            if (transform.position.x>Player.position.x)
            {
                anim.SetBool("Run",true);
                rb.velocity= new Vector3(expulsion_direction*attacking_speed*Time.deltaTime,rb.velocity.y,0);
            
            }
            if (Player.position.x>transform.position.x)
            {
                anim.SetBool("Run",true);
                rb.velocity= new Vector3(expulsion_direction*attacking_speed*Time.deltaTime,rb.velocity.y,0);
            }        
        }    
        if (anim.GetBool("Death"))
        {
            
            if (current_attack_time>0.65f)
        {
            Destroy(gameObject);
        }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Platform")
        {
            anim.SetBool("Touch_ground",true);
        }
    }
    
    public void Take_Damage(float damage)
    {
        
        current_health -= damage;
        anim.SetTrigger("Hurt");
        if (current_health<=0)
        {
            Die();
        }
        if (transform.position.x>Player.position.x)
            {
                transform.localScale = new Vector3(-4,4,4);
            
            }
            if (Player.position.x>transform.position.x)
            {
                transform.localScale = new Vector3(4,4,4);
            } 
        
    }
    public void Die()
    {
        
        anim.SetBool("Death", true);
        

    }

    public void Flap()
    {
        face_right = !face_right;
        direction = gameObject.transform.localScale;
        direction.x = expulsion_direction*4;
        gameObject.transform.localScale = direction;
    }
}
