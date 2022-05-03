
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public Gun[] loadout;
    public Transform weaponParent;
    public GameObject bulletholePrefab;
    public Transform camera;
    public LayerMask canBeShot;

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
            Equip(activeSlot);
        }


        if (currentWeapon != null)
        {
            Aim(Input.GetMouseButton(1) && loadout[activeSlot].isAimeble);

            if (Input.GetMouseButtonDown(0))
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
        Transform anchor = currentWeapon.transform.Find("Anchor");
        Transform stateHip = currentWeapon.transform.Find("States/Hip");
        Transform stateADS = currentWeapon.transform.Find("States/ADS");
        if (isAiming)
        {
            anchor.position = Vector3.Lerp(anchor.position, stateADS.position, Time.deltaTime * loadout[activeSlot].aimSpeed);
        }
        else
        {
            anchor.position = Vector3.Lerp(anchor.position, stateHip.position, Time.deltaTime * loadout[activeSlot].aimSpeed);
        }
    }

    void Shot()
    {
        Transform spawn = camera;

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
