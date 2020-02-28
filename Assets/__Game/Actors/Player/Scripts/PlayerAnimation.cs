using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public GameObject shadow;

    private Animator animator;
    private Rigidbody2D mRigidbody2D;
    private PlayerManager player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerManager>();
        mRigidbody2D = GetComponent<Rigidbody2D>();
    }

    

    // Update is called once per frame
    void Update()
    {
        PaintShadow();

        animator.SetFloat("velocity", mRigidbody2D.velocity.y);
        animator.SetBool("isAlive", player.PlayerIsAlive);
        
    }

  

    private void PaintShadow() 
    {
        if (shadow != null) 
        {

            if (!player.FloorCheck)
            {
                shadow.SetActive(false);
            }
            else 
            {
                shadow.SetActive(true);
                float scaleX = player.fireDistance / player.FloorCheck.distance;

                if (scaleX <= 0)
                {
                    shadow.SetActive(false);
                }
                else
                {
                    float positionY = player.FloorCheck.point.y;
                    Vector3 position = shadow.transform.position;
                    position.y = positionY;
                    shadow.transform.position = position;
                    if (scaleX > 1)
                        scaleX = 1;
                    Vector3 scale = shadow.transform.localScale;
                    scale.x = scaleX;
                    shadow.transform.localScale = scale;
                }
                
            }
        }
    }
}
