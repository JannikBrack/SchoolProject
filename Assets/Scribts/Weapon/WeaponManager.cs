using UnityEngine.UI;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public Weapon[] loadout;
    [SerializeField] Transform weaponParent;
    [SerializeField] GameObject bulletholePrefab;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask canBeShot;
    [SerializeField] InvOpenClose invOpenClose;
    public GameObject[] uiSlots = new GameObject[3];
    public GameObject[] invWeaponSlots = new GameObject[3];
    [SerializeField] Color color;


    private GameObject currentWeapon;
    GameObject newEquipment;
    private int activeSlot;
    #endregion

    #region Start&Update

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && loadout[0] != null)
        {
            activeSlot = 0;
            Equip(0);
            uiSlots[0].GetComponent<Image>().color = Color.gray;
            uiSlots[1].GetComponent<Image>().color = color;
            uiSlots[2].GetComponent<Image>().color = color;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && loadout[1] != null)
        {
            activeSlot = 1;
            Equip(1);
            uiSlots[1].GetComponent<Image>().color = Color.gray;
            uiSlots[0].GetComponent<Image>().color = color;
            uiSlots[2].GetComponent<Image>().color = color;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && loadout[2] != null)
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
                Attack();
            }
        }
    }
    #endregion

    #region Methods
    public void Equip(int slot)
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

    void Attack()
    {
        Transform spawn = cam;

        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(spawn.position, spawn.forward, out hit, 1000f, canBeShot))
        {
            GameObject newHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);
            newHole.transform.LookAt(hit.point + hit.normal);
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();
                enemyHealth.GetDamage(loadout[activeSlot].damage);
            }
            Destroy(newHole, 5f);
        }
    }

    public void SwitchWeaponIcon(Weapon weapon, bool setActive)
    {
        uiSlots[weapon.weaponType].GetComponent<Image>().sprite = weapon.weaponIcon;
        uiSlots[weapon.weaponType].SetActive(setActive);
        invWeaponSlots[weapon.weaponType].GetComponent<Image>().sprite = weapon.weaponIcon;
        invWeaponSlots[weapon.weaponType].GetComponent<Image>().name = weapon.weaponIcon.name;
        invWeaponSlots[weapon.weaponType].SetActive(setActive);
    }
    #endregion
}
