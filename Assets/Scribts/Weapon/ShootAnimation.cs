using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimation : MonoBehaviour
{
    public Animator animator;
    public int shoot;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.Play("Shoot");
            shoot = 0;
        }
    }
}
