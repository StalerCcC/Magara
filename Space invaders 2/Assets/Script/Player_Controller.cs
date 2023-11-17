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
    public static Player_Controller instance;
    public Rigidbody2D rb ;
    private float horizontal;
    Vector3 direction;
    bool face_right;
    public float jump_force;
    public float speed;
    public float expulsion;
    public float expulsion_direction=1;
    bool jump = true;
    Animator anim;
    public bool canRecieveInput= true;
    public bool InputReceived;

    public bool idle;

    private void Start()
    {
        instance = this;
        rb = gameObject.GetComponent<Rigidbody2D>();
        direction = gameObject.transform.localScale;
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (idle==true)
        {
            rb.velocity = new Vector3(horizontal * speed* Time.deltaTime ,rb.velocity.y,0);
        }
        else
        {
             rb.velocity = new Vector3(0,rb.velocity.y,0);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
        }

        if (Input.GetKeyDown(KeyCode.Space)&jump==true)
        {
            Debug.Log("Space");
            jump =false;
            rb.AddForce(new Vector2(0,jump_force));
        }

        if (horizontal==1&face_right==true)
        {
            Flap();
        }
        if (horizontal==-1&face_right==false)
        {
            Flap();
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
        if (collision.gameObject.tag == "Platform")
        {
            jump=true;
        }
    }
    
    void Flap()
    {
        face_right = !face_right;
        direction = gameObject.transform.localScale;
        direction.x *= -1;
        gameObject.transform.localScale = direction;
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (canRecieveInput)
            {
                InputReceived=true;
                canRecieveInput = false;
            }
            else
            {
                return;
            }
        }
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
