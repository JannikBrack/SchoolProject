using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvManager1 : MonoBehaviour
{
    public GameObject Inv;
    bool InvOpen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!InvOpen)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                Inv.SetActive(true);
                InvOpen = true;
            }
            else
            {
                Inv.SetActive(false);
                InvOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
