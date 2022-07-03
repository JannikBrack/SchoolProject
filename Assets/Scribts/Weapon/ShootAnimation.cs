using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAnimation : MonoBehaviour
{
    public Animator animator;
    public enum weaponState {SHOOT, WAIT};
    public weaponState state;

    private void Update()
    {
        if(state == weaponState.SHOOT) 
        {
            state = weaponState.WAIT;
        }
    }

    public void Shoot()
    {
        state = weaponState.SHOOT;
    }
}
