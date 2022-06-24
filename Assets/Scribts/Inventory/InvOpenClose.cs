using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvOpenClose : MonoBehaviour
{
    [SerializeField] Animator InvAnimator;
    [SerializeField] GameObject InvPrefab;
    public bool InvOpen = false;

    //opens or closes the inventory when tab is pressed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!InvOpen)
            {
                InvPrefab.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                InvAnimator.Play("InventoryFadeIn");
                InvOpen = true;
            }
            else
            {
                InvAnimator.Play("InvetoryFadeOut");
                InvOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
