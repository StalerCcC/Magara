using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{
   
    public static Enemy_Controller instance;
    public Rigidbody2D rb;
    public Transform[] patrolpoints;
    public Transform Player;
    Vector3 direction;
    Animator anim;
    public float max_health=100;
    public float current_health;
    public float attack_range;
    public float chasing_range;
    public float attacking_speed;
    public float patroldestination;
    public bool face_right;
    public bool is_attacking;
    public bool is_chasing;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        current_health=max_health;
    }
    
    void Update()
    {
        if (Math.Abs(Player.position.x-transform.position.x)<attack_range)
        {
            
            is_attacking=true;
            is_chasing=false;
        }
        if (is_attacking)
        {
            
        }











        if (Math.Abs(Player.position.x-transform.position.x)<chasing_range)
        {
            
            is_chasing=true;
        }

        if (is_chasing)
        {
            if (transform.position.x>Player.position.x)
            {
                transform.position += Vector3.left*attacking_speed*Time.deltaTime;
                transform.localScale = new Vector3(-2,2,2);
            
            }
            if (Player.position.x>transform.position.x)
            {
                transform.position += Vector3.right*attacking_speed*Time.deltaTime;
                transform.localScale = new Vector3(2,2,2);
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
        
        
    }
    public void Take_Damage(float damage)
    {
        anim.SetTrigger("Hurt");
        current_health -= damage;
        if (current_health<=0)
        {
            Die();
        }
    }
    public void Die()
    {
        anim.SetBool("Death", true);
    }
    
    
    private void Attack()
    {
        
    }
    void Flap()
    {
        face_right = !face_right;
        direction = gameObject.transform.localScale;
        direction.x *= -1;
        gameObject.transform.localScale = direction;
    }
}
