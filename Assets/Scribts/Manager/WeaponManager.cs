using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    #region Variables
    public Weapon[] loadout;
    [SerializeField] GameObject bulletholePrefab;
    public GameObject[] uiSlots = new GameObject[3];
    public GameObject[] invWeaponSlots = new GameObject[3];

    [SerializeField] Transform secParent;
    [SerializeField] Transform primParent;
    [SerializeField] Transform meelParent;
    [SerializeField] Transform cam;
    
    [SerializeField] InvOpenClose invOpenClose;
    [SerializeField] ItemManager itemManager;

    [SerializeField] TextMeshProUGUI ammoText;

    [SerializeField] LayerMask canBeShot;

    
    [SerializeField] Color color;

    private GameObject currentWeapon;
    private GameObject newEquipment;

    private int activeSlot;
    private bool emptySlot;
    private float CooldownTime;
    private bool emptyWeapon;

    //Ammonition
    private int magAmmoAmount;
    private int magSize;
    private int ammoAmount;
    private int reloadAmmo;

    #endregion

    //ResetEquip
    private void Awake()
    {
        Equip(0);
    }
    #region Start&Update

    void Update()
    {
        RefreshImportantInteger();

        //Equip Weapons
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

        //Test if Player can reload 
        if (Input.GetKeyDown(KeyCode.R)) Reload();

        //test if the weapon slot is Empty
        if ( activeSlot != 2 && loadout[activeSlot] != null && ((loadout[activeSlot].ammoAmount == 0 && loadout[activeSlot].currentMagAmmoAmount == 0) || loadout[activeSlot].currentMagAmmoAmount == 0)) emptyWeapon = true;

        //Fix ammunitionUI if a Weapon is Equiped which does not need ammunition. Else show current ammoamount.
        if (activeSlot == 2 || loadout[activeSlot] == null) ammoText.text = "-/-";
        else
            ammoText.text = loadout[activeSlot].currentMagAmmoAmount.ToString() + "/" + loadout[activeSlot].magazineSize.ToString();

        //Cooldown for single shotweapon
        if(loadout[activeSlot] != null &&  loadout[activeSlot].weaponType == 0)
        {
            if (CooldownTime > 0)
            {
                CooldownTime -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButton(0) && !invOpenClose.InvOpen && !PlayerManager.instance.deadPlayer && !PlayerManager.instance.gamePaused && !emptyWeapon)
                {
                    Attack(false);
                    if (loadout[activeSlot] != null)
                        ResetCooldown(loadout[activeSlot].cooldownTime);
                    else
                        ResetCooldown(0);
                }
            }
        }
        else if (loadout[activeSlot] != null && loadout[activeSlot].weaponType == 1)
        {
            if (CooldownTime > 0)
            {
                CooldownTime -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && !invOpenClose.InvOpen && !PlayerManager.instance.deadPlayer && !PlayerManager.instance.gamePaused && !emptyWeapon)
                {
                    Attack(false);
                    if (loadout[activeSlot] != null)
                        ResetCooldown(loadout[activeSlot].cooldownTime);
                    else
                        ResetCooldown(0);
                }
            }
        }
        else if (loadout[activeSlot] != null && loadout[activeSlot].weaponType == 2)
        {
            if (CooldownTime > 0)
            {
                CooldownTime -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && !invOpenClose.InvOpen && !PlayerManager.instance.deadPlayer && !PlayerManager.instance.gamePaused)
                {
                    Debug.Log(1);
                    Attack(true);
                    if (loadout[activeSlot] != null)
                        ResetCooldown(loadout[activeSlot].cooldownTime);
                    else
                        ResetCooldown(0);
                }
            }
        }



    }
    #endregion

    #region Methods
    //Equips the chosen Weaponslot
    public void Equip(int slot)
    {
        if (loadout[slot] == null)
        {
            Destroy(currentWeapon);
            emptySlot = true;
        }
        else
        {
            emptySlot = false;
            if (currentWeapon != null) Destroy(currentWeapon);
            if (loadout[slot].weaponType == 1)
            newEquipment = Instantiate(loadout[slot].prefab, secParent.position, secParent.rotation, secParent) as GameObject;
            else if (loadout[slot].weaponType == 2)
                newEquipment = Instantiate(loadout[slot].prefab, meelParent.position, meelParent.rotation, meelParent) as GameObject;
            else if (loadout[slot].weaponType == 0)
                newEquipment = Instantiate(loadout[slot].prefab, primParent.position, primParent.rotation, primParent) as GameObject;
            newEquipment.transform.localPosition = Vector3.zero;
            newEquipment.transform.localEulerAngles = Vector3.zero;
            currentWeapon = newEquipment;
        }
    }

    //Get iportant Information from other Classes
    private void RefreshImportantInteger()
    {
        if(loadout[activeSlot] != null)
        {
            magAmmoAmount = CurrentMagAmmoAmount;
            magSize = MagazineSize;
            ammoAmount = AmmoAmount;
        }
    }

    //is allowing the player to reload the gun, based on the ammunition in the inventory and how huge the magazine capacity size is
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

    /*The player can attack in 3 different ways: punching, hitting with the knife, and shooting
     *There is also a damage calculation if-query when the player bought the Skill "Deadly Precision"*/
    void Attack(bool melee)
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


        if (melee)
        {
            //Knife attack
            if (Physics.Raycast(spawn.position, spawn.forward, out hit, 4f, canBeShot))
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
            //Shoot
            if (Physics.Raycast(spawn.position, spawn.forward, out hit, 1000f, canBeShot))
            {
                loadout[activeSlot].ShootAnimation.Shoot();

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

    //Resets the cooldown to shoot a singleshot
    private void ResetCooldown(float cooldown)
    {
        CooldownTime = cooldown;
    }

    //is Switching Icons
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
