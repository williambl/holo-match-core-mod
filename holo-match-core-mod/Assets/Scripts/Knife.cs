using UnityEngine;
using UnityEngine.Networking;

public class Knife : Weapon {

    [HideInInspector]
    public new string name = "Knife";

    [HideInInspector]
    public new EnumSlot slot = EnumSlot.SPECIAL;

    [HideInInspector]
    public new EnumAmmoType ammoType = EnumAmmoType.NONE;
    [HideInInspector]
    public new EnumWeaponType type = EnumWeaponType.MELEE;
    [HideInInspector]
    public new EnumFireType fireType = EnumFireType.SEMI_AUTO;

    [HideInInspector]
    public new int ammo = 0;
    [HideInInspector]
    public new int maxAmmo = 0;
    [HideInInspector]
    public new bool infiniteAmmo = true;

    [HideInInspector]
    public new float fireCooldown = 0.1f;
    [HideInInspector]
    public new float reloadTime = 0f;

    PlayerController pc;
    Camera cam;

    float range = 1f;
    Vector3 offset = new Vector3(0, 0, 0.7f);

    new void Start () {
        pc = GetComponent<PlayerController>();
        cam = GetComponentInChildren<Camera>();
    }

    new void Update () {
        if (!isLocalPlayer || !weaponGObject.activeInHierarchy || pc.pauseController.isPaused)
            return;

        if (Input.GetButtonDown("Fire1")) {

            //If we still need to wait until the next fire, then don't do anything
            if (Time.time < nextFireTime)
                return;

            Fire();
        }
        pc.ammoText.text = "-";
    }

    public new void End () {
    }

    public new void Fire () {
        CmdAttack();
        nextFireTime = Time.time + fireCooldown; 
    }

    public new void Reload () {
        ammo = maxAmmo;
    }

    [Command]
    public void CmdAttack () {
        Collider[] colls = Physics.OverlapSphere(pc.transform.position, 1);
        foreach (Collider coll in colls) {
            PlayerController pcHit = coll.GetComponent<PlayerController>();
            if (pcHit != null && pcHit != pc)
                pcHit.health.TakeDamage(10);
        }
    }
}
