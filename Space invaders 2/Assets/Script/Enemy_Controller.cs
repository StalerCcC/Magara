using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
    public LayerMask Main_Character;
    public static Enemy_Controller instance;
    public Rigidbody2D rb;
    public Transform[] patrolpoints;
    public Transform Player;
    public Transform Enemy_Attack_point;
    Vector3 direction;
    Animator anim;
    public float max_health=100;
    public float current_health;
    public float attack_range;
    public float chasing_range;
    public float attacking_speed;
    public float patroldestination;
    public float damage;
    public float expulsion_direction;
    public float expulsion;
    public float attack_duration;
    public float current_attack_time;
    public float jump_anim_time;
    public float jump_anim_duration;
    public float anim_time;
    public float wait_time;
    public float jump_range;
    public float jump_forcex;
    public float jump_forcey;
    public float patrolling_speed;
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
        
        
              Collider2D[] hit_enemies=Physics2D.OverlapCircleAll(Enemy_Attack_point.position,attack_range,Main_Character);
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
        Collider2D[] hit_enemies=Physics2D.OverlapCircleAll(Enemy_Attack_point.position,attack_range,Main_Character);
            foreach (Collider2D Enemy in hit_enemies)
            {
                jump_anim_time=0;
                jump=false;
                can_jump=false;
            
            }   
        if ( Math.Abs(Player.position.x - transform.position.x) <= jump_range&&jump==true)
        {
            Flap();
            Debug.Log("Jump_Mesafesinde");
            rb.AddForce(new Vector2(0,jump_forcey));
            jump_anim_time += Time.deltaTime;   
            anim.SetTrigger("Jump");
            can_jump=true;
        }
        if (jump_anim_time<jump_anim_duration&&can_jump)
        {
            rb.velocity=new Vector2(expulsion_direction*jump_forcex*Time.deltaTime,0);
        }
        if (jump_anim_time>jump_anim_duration)
        {
            
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
            
            is_chasing=true;
            is_attacking=false;
        }

        if (is_chasing)
        {
            if (transform.position.x>Player.position.x&&can_jump==false)
            {
                anim.SetBool("Run",true);
                rb.velocity= new Vector3(expulsion_direction*attacking_speed*Time.deltaTime,rb.velocity.y,0);
            
            }
            if (Player.position.x>transform.position.x&&can_jump==false)
            {
                anim.SetBool("Run",true);
                rb.velocity= new Vector3(expulsion_direction*attacking_speed*Time.deltaTime,rb.velocity.y,0);
            }        
        }
        else if(is_attacking==false&&is_chasing==false)
        {
            if (patroldestination==0)
            {
                transform.position = Vector2.MoveTowards(transform.position,patrolpoints[0].position,patrolling_speed*Time.deltaTime);
                anim.SetBool("Run",true);
                if (Vector2.Distance(transform.position, patrolpoints[0].position)<0.5f)
                {
                    patroldestination=1;
                    transform.localScale = new Vector3(-4,4,4);
                }
            }
            if (patroldestination==1)
            {
                transform.position = Vector2.MoveTowards(transform.position,patrolpoints[1].position,patrolling_speed*Time.deltaTime);
                anim.SetBool("Run",true);
                if (Vector2.Distance(transform.position, patrolpoints[1].position)<0.5f)
                {
                    patroldestination=0;
                    transform.localScale = new Vector3(4,4,4);
                }
            }
        }
        if (anim.GetBool("Death"))
        {
            if (current_attack_time>1)
        {
            Destroy(gameObject);
        }
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
