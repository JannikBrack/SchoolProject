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
    private bool EmptySlot;
    private float lastShot;



    #endregion
    private void Awake()
    {
        Equip(0);
    }
    #region Start&Update

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeSlot = 0;
            Equip(activeSlot);
            uiSlots[0].GetComponent<Image>().color = Color.gray;
            uiSlots[1].GetComponent<Image>().color = color;
            uiSlots[2].GetComponent<Image>().color = color;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeSlot = 1;
            Equip(activeSlot);
            uiSlots[1].GetComponent<Image>().color = Color.gray;
            uiSlots[0].GetComponent<Image>().color = color;
            uiSlots[2].GetComponent<Image>().color = color;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeSlot = 2;
            Equip(activeSlot);
            uiSlots[2].GetComponent<Image>().color = Color.gray;
            uiSlots[1].GetComponent<Image>().color = color;
            uiSlots[0].GetComponent<Image>().color = color;
        }

        if (currentWeapon != null) Aim(Input.GetMouseButton(1) && loadout[activeSlot].isAimable);

        if (Input.GetMouseButtonDown(0) && !invOpenClose.InvOpen) Attack();
    }
    #endregion

    #region Methods
    public void Equip(int slot)
    {
        if (loadout[slot] == null)
        {
            Destroy(currentWeapon);
            EmptySlot = true;
        }
        else
        {
            EmptySlot = false;
            if (currentWeapon != null) Destroy(currentWeapon);
            newEquipment = Instantiate(loadout[slot].prefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;
            currentWeapon = newEquipment;
        }
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
        
            if (EmptySlot)
            {
                //punch
                if (Physics.Raycast(spawn.position, spawn.forward, out hit, 2f, canBeShot))
                {
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        EnemyHealth enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();
                        if (loadout[activeSlot] == null) enemyHealth.GetDamage(0.05f);
                    }
                }
            }
            else if (loadout[2] != null)
            {
                //punch
                if (Physics.Raycast(spawn.position, spawn.forward, out hit, 2f, canBeShot))
                {
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        EnemyHealth enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();
                        enemyHealth.GetDamage(loadout[activeSlot].damage);
                    }
                }
            }
            else
            {
                if (Physics.Raycast(spawn.position, spawn.forward, out hit, 1000f, canBeShot))
                {
                    GameObject newHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);
                    newHole.transform.LookAt(hit.point + hit.normal);
                    if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        EnemyHealth enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();
                        enemyHealth.GetDamage(loadout[activeSlot].damage);
                        Destroy(newHole, 5f / (hit.collider.gameObject.GetComponent<EnemyController>().speed * 2.5f));
                    }
                    else
                        Destroy(newHole, 5f);
                }
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
