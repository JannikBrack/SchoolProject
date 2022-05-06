using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    [SerializeField] Animator InvAnimator;
    [SerializeField] GameObject InvPrefab;
    public bool InvOpen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!InvOpen)
            {
                InvPrefab.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                InvAnimator.SetTrigger("InvetoryOpen");
                InvOpen = true;
            }
            else
            {
                InvAnimator.SetTrigger("InventoryClose");
                InvOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
