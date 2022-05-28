using UnityEngine.UI;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    [SerializeField] Weapon[] loadout;
    [SerializeField] Transform weaponParent;
    [SerializeField] GameObject bulletholePrefab;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask canBeShot;
    [SerializeField] InvOpenClose invOpenClose;
    public GameObject[] uiSlots;
    [SerializeField] Color color;


    private GameObject currentWeapon;
    GameObject newEquipment;
    private int activeSlot;
    #endregion

    #region Start&Update

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeSlot = 0;
            Equip(0);
            uiSlots[0].GetComponent<Image>().color = Color.gray;
            uiSlots[1].GetComponent<Image>().color = color;
            uiSlots[2].GetComponent<Image>().color = color;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeSlot = 1;
            Equip(1);
            uiSlots[1].GetComponent<Image>().color = Color.gray;
            uiSlots[0].GetComponent<Image>().color = color;
            uiSlots[2].GetComponent<Image>().color = color;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeSlot = 2;
            Equip(2);
            uiSlots[2].GetComponent<Image>().color = Color.gray;
            uiSlots[1].GetComponent<Image>().color = color;
            uiSlots[0].GetComponent<Image>().color = color;
        }


        if (currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1) && loadout[activeSlot].isAimable);

            if (Input.GetMouseButtonDown(0) && !invOpenClose.InvOpen)
            {
                Shot();
            }
        }
    }
    #endregion

    #region Methods
    void Equip(int slot)
    {
        if (currentWeapon != null) Destroy(currentWeapon);

        newEquipment = Instantiate(loadout[slot].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        newEquipment.transform.localPosition = Vector3.zero;
        newEquipment.transform.localEulerAngles = Vector3.zero;
        currentWeapon = newEquipment;
    }

    void Aim(bool isAiming)
    {
        if (isAiming)
        {
            
        }
        else
        {
            
        }
    }

    void Shot()
    {
        Transform spawn = cam;

        RaycastHit hit = new RaycastHit();

       if(Physics.Raycast(spawn.position, spawn.forward, out hit, 1000f, canBeShot))
        {
            GameObject newHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);
            newHole.transform.LookAt(hit.point + hit.normal);
            Destroy(newHole, 5f);
        }
    }
    #endregion
}
