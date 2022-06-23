using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public Weapon[] loadout;
    [SerializeField] Transform weaponParent;
    [SerializeField] GameObject bulletholePrefab;
    [SerializeField] Transform cam;
    [SerializeField] LayerMask canBeShot;
    [SerializeField] InvOpenClose invOpenClose;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] ItemManager itemManager;
    public GameObject[] uiSlots = new GameObject[3];
    public GameObject[] invWeaponSlots = new GameObject[3];
    [SerializeField] Color color;
    private GameObject currentWeapon;
    GameObject newEquipment;
    private int activeSlot;
    private bool EmptySlot;
    private float CooldownTime;
    private bool emptyWeapon;

    private int magAmmoAmount;
    private int magSize;
    private int ammoAmount;
    private int reloadAmmo;

    #endregion
    private void Awake()
    {
        Equip(0);
    }
    #region Start&Update

    void Update()
    {
        RefreshImportantInteger();
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

        if (Input.GetKeyDown(KeyCode.R)) Reload();

        if (loadout[activeSlot] != null && ((loadout[activeSlot].ammoAmount == 0 && loadout[activeSlot].currentMagAmmoAmount == 0) || loadout[activeSlot].currentMagAmmoAmount == 0)) emptyWeapon = true;

        if (activeSlot == 2 || loadout[activeSlot] == null) ammoText.text = "-/-";
        else
            ammoText.text = loadout[activeSlot].currentMagAmmoAmount.ToString() + "/" + loadout[activeSlot].magazineSize.ToString();

        if (CooldownTime > 0)
        {
            CooldownTime -= Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !invOpenClose.InvOpen && !PlayerManager.instance.deadPlayer && !PlayerManager.instance.gamePaused && !emptyWeapon)
            {
                Attack();
                if (loadout[activeSlot] != null)
                    ResetCooldown(loadout[activeSlot].cooldownTime);
                else
                    ResetCooldown(0);
            }
        }

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

    private void RefreshImportantInteger()
    {
        if(loadout[activeSlot] != null)
        {
            magAmmoAmount = CurrentMagAmmoAmount;
            magSize = MagazineSize;
            ammoAmount = AmmoAmount;
        }
    }

    private void Reload()
    {
        
        if (loadout[activeSlot] != null && magSize <= ammoAmount)
        {
            //ReloadAnimation

            reloadAmmo = magSize - magAmmoAmount;
            CurrentMagAmmoAmount += reloadAmmo;
            itemManager.RemoveUsedAmmo(loadout[activeSlot].ammoID,reloadAmmo);

            emptyWeapon = false;
        }
        else if (loadout[activeSlot] != null && magSize > ammoAmount)
        {
            //ReloadAnimation
            reloadAmmo = magSize - magAmmoAmount;
            if (reloadAmmo >= AmmoAmount)
            {
                CurrentMagAmmoAmount += AmmoAmount;
                itemManager.RemoveUsedAmmo(loadout[activeSlot].ammoID, AmmoAmount);
            }
            else
            {
                CurrentMagAmmoAmount += reloadAmmo;
                itemManager.RemoveUsedAmmo(loadout[activeSlot].ammoID, reloadAmmo);
            }
            emptyWeapon = false;
        }
        
        
    }

    void Attack()
    {
        Transform spawn = cam;
        RaycastHit hit = new RaycastHit();
        float calculatedDamage = 0f;

        if (PlayerManager.instance.deadlyPrecision)
        {
            if (Random.Range(0,100) <= 20)
            {
                if (loadout[activeSlot] == null)
                {
                    calculatedDamage = 0.1f;
                }
                else
                    calculatedDamage = loadout[activeSlot].damage * 2;
            }
            else if(loadout[activeSlot] == null)
            { 
                calculatedDamage = 0.05f;
            }
            else
                calculatedDamage = loadout[activeSlot].damage;
        }
        else if (loadout[activeSlot] == null)
        {
            calculatedDamage = 0.05f;
        }
        else calculatedDamage = loadout[activeSlot].damage;

        if (EmptySlot)
        {
            //punch
            if (Physics.Raycast(spawn.position, spawn.forward, out hit, 2f, canBeShot))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyHealthManager enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>();
                    if (loadout[activeSlot] == null) enemyHealth.GetDamage(enemyHealth.EnemyHealth * 0.05f);
                }
            }
        }
        else if (loadout[2] != null && activeSlot == 2)
        {
            //punch
            if (Physics.Raycast(spawn.position, spawn.forward, out hit, 2f, canBeShot))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyHealthManager enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>();
                    enemyHealth.GetDamage(calculatedDamage);
                }
            }
        }
        else
        {
            if (Physics.Raycast(spawn.position, spawn.forward, out hit, 1000f, canBeShot))
            {
                Debug.Log(1);
                loadout[activeSlot].ShootAnimation.shoot = 1;
                GameObject newHole = Instantiate(bulletholePrefab, hit.point + hit.normal * 0.001f, Quaternion.identity);
                newHole.transform.LookAt(hit.point + hit.normal);
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    EnemyHealthManager enemyHealth = hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>();

                    enemyHealth.GetDamage(calculatedDamage);

                    Destroy(newHole, 0.05f);
                }
                else
                    Destroy(newHole, 5f);
                loadout[activeSlot].currentMagAmmoAmount--;
            }
        }
    }
    private void ResetCooldown(float cooldown)
    {
        CooldownTime = cooldown;
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

    #region Get/Set Methods
    private int CurrentMagAmmoAmount { get => loadout[activeSlot].currentMagAmmoAmount; set => loadout[activeSlot].currentMagAmmoAmount = value; }
    private int MagazineSize { get => loadout[activeSlot].magazineSize;}
    private int AmmoAmount { get => loadout[activeSlot].ammoAmount; set => loadout[activeSlot].ammoAmount = value; }
    #endregion
}
