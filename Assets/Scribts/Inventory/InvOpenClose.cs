using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvOpenClose : MonoBehaviour
{
    [SerializeField] Animator InvAnimator;
    [SerializeField] GameObject InvPrefab;
    public bool InvOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!InvOpen)
            {
                Debug.Log("1");
                InvPrefab.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                InvAnimator.Play("InventoryFadeIn");
                InvOpen = true;
            }
            else
            {
                Debug.Log("2");
                InvAnimator.Play("InvetoryFadeOut");
                InvOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
