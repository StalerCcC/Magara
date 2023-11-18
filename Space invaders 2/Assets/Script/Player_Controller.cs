using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.XR;
using Input = UnityEngine.Input;

public class Player_Controller : MonoBehaviour
{
    public Transform attackpoint;
    public LayerMask enemy;
    public static Player_Controller instance;
    public Rigidbody2D rb ;   
    Vector3 direction;
    private float horizontal;
    public float jump_force;
    public float speed;
    public float attack_expulsion;
    public float expulsion;
    public float expulsion_direction=1;
    public float roll_duration;
    public float roll_force;
    private float rollCurrentTime;
    public float roll_direction;
    public float attack_range;
    public float damage=40;
    public float max_health;
    public float current_health;
    
    Animator anim;
    public bool canRecieveInput= true;
    public bool InputReceived;
    public bool jump = true;
    public bool idle;
    public bool attacking;

    public bool m_rolling;
    bool face_right;

    private void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        direction = gameObject.transform.localScale;
        anim = gameObject.GetComponent<Animator>();
        current_health=max_health;
    }

    // Update is called once per frame
    private void Update()
    {

        horizontal = Input.GetAxisRaw("Horizontal");

        if (idle==true)
        {
            rb.velocity = new Vector3(horizontal * speed* Time.deltaTime ,rb.velocity.y,0);
        }
        else if(!idle)
        {
             rb.velocity = new Vector3(0,rb.velocity.y,0);
        }
        if (m_rolling==true)
        {
            rollCurrentTime +=Time.deltaTime;
        }
        if (rollCurrentTime>roll_duration)
        {
            m_rolling=false;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)&&m_rolling==false&&jump)
        {
            gameObject.layer=0;
            anim.SetTrigger("Roll");
            rollCurrentTime=0;
            rb.velocity=new Vector2(expulsion_direction*roll_force,rb.velocity.y);
            m_rolling=true;
        }

        if (Input.GetKeyDown(KeyCode.Space)&jump==true)
        {
            jump =false;
            rb.AddForce(new Vector2(0,jump_force));
        }

        if (horizontal==1&face_right==true)
        {
            Flaping();
        }
        if (horizontal==-1&face_right==false)
        {
            Flaping();
        }

        if (horizontal==1)
        {
            anim.SetBool("Run",true);
            expulsion_direction=1;
        }
        if (horizontal==-1)
        {
            anim.SetBool("Run",true);
            expulsion_direction=-1;
        }
        if (horizontal==0)
        {
            anim.SetBool("Run",false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (canRecieveInput)
            {
                InputReceived=true;
                anim.SetBool("Attack",true);
                canRecieveInput = false;
                Attack();
                
            }
            else
            {
                return;
                
            }

        }
        if (rb.velocity.y>  0)
        {
            anim.SetBool("Jump",true);
            anim.SetBool("Fall",false); 
        }
        else if (rb.velocity.y<0 )
        {
            anim.SetBool("Fall",true);
            anim.SetBool("Jump",false); 
        }
        else
        {
            anim.SetBool("Fall",false);
            anim.SetBool("Jump",false); 
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform"||collision.gameObject.tag == "Enemy")
        {
            jump=true;
        }
    }
    
    void Flaping()
    {
        face_right = !face_right;
        direction = gameObject.transform.localScale;
        direction.x *= -1;
        gameObject.transform.localScale = direction;
    }
    
    

    public void Attack()
    {
        Collider2D[] hit_enemies=Physics2D.OverlapCircleAll(attackpoint.position,attack_range,enemy);
        foreach (Collider2D Enemy in hit_enemies)
        {
            if (Enemy.GetComponent<Falling_Enemy_Controller>().current_health>0)
            {
                Enemy.GetComponent<Falling_Enemy_Controller>().current_attack_time=0;
                Enemy.GetComponent<Falling_Enemy_Controller>().Flap();
                Enemy.GetComponent<Falling_Enemy_Controller>().Take_Damage(damage);
                Enemy.GetComponent<Falling_Enemy_Controller>().rb.AddForce(new Vector2(attack_expulsion*expulsion_direction,0));
            }
            if (Enemy.GetComponent<Enemy_Controller>().current_health>0)
            {
                Enemy.GetComponent<Enemy_Controller>().current_attack_time=0;
                Enemy.GetComponent<Enemy_Controller>().Flap();
                Enemy.GetComponent<Enemy_Controller>().Take_Damage(damage);
                Enemy.GetComponent<Enemy_Controller>().rb.AddForce(new Vector2(attack_expulsion*expulsion_direction,0));
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
    
    
    public void InputManager()
    {
        if (!canRecieveInput)
        {
            canRecieveInput=true;
        }
        else
        {
            canRecieveInput=false;
        }
    }

    public void Attack_expulsion()
    {
        rb.AddForce(new Vector2(expulsion*expulsion_direction,0));
    }
    
}
