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
    public bool face_right;
    public bool is_attacking;
    public bool is_chasing;
    public bool attacking;



    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        current_health=max_health;
    }
    
    void Update()
    {
        
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
            Attack();
            current_attack_time=0;
            attacking=true;
        }
        if (is_attacking)
        {
            anim.SetBool("Run",false);
            rb.velocity=new Vector2(0,rb.velocity.y);
        }

        if (rb.velocity.x>0)
        {
            expulsion_direction=1;
        }
        if (rb.velocity.x<0)
        {
            expulsion_direction=-1;
        }

        if (Math.Abs(Player.position.x-transform.position.x)<chasing_range&&Math.Abs(Player.position.x-transform.position.x)>attack_range)
        {
            
            is_chasing=true;
            is_attacking=false;
        }

        if (is_chasing)
        {
            if (transform.position.x>Player.position.x)
            {
                anim.SetBool("Run",true);
                rb.velocity= new Vector3(-1*attacking_speed*Time.deltaTime,rb.velocity.y,0);
            
            }
            if (Player.position.x>transform.position.x)
            {
                anim.SetBool("Run",true);
                rb.velocity= new Vector3(attacking_speed*Time.deltaTime,rb.velocity.y,0);
            }        
        }
        else if(is_attacking==false&&is_chasing==false)
        {
            if (patroldestination==0)
            {
                transform.position = Vector2.MoveTowards(transform.position,patrolpoints[0].position,attacking_speed*Time.deltaTime);
                anim.SetBool("Run",true);
                if (Vector2.Distance(transform.position, patrolpoints[0].position)<0.5f)
                {
                    patroldestination=1;
                    Flap();
                }
            }
            if (patroldestination==1)
            {
                transform.position = Vector2.MoveTowards(transform.position,patrolpoints[1].position,attacking_speed*Time.deltaTime);
                anim.SetBool("Run",true);
                if (Vector2.Distance(transform.position, patrolpoints[1].position)<0.5f)
                {
                    patroldestination=0;
                    Flap();
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
                transform.localScale = new Vector3(4,4,4);
            
            }
            if (Player.position.x>transform.position.x)
            {
                transform.localScale = new Vector3(-4,4,4);
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
        direction.x *= -1;
        gameObject.transform.localScale = direction;
    }
}
