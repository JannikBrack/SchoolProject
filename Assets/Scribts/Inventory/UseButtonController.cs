using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseButtonController : MonoBehaviour
{
    private bool isActive;
    [SerializeField] Animator animator;
    public void ButtonPressed()
    {
        if (isActive)
        {
            isActive = false;
            animator.SetTrigger("Disable");
        }
        else
        {
            isActive = true;
            animator.SetTrigger("Anable");
        }
    }
}
