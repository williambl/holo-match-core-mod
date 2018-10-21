using UnityEngine;
using UnityEngine.Networking;

public class AssaultRifle : Weapon {

    [HideInInspector]
    public new string name = "Assault Rifle";

    [HideInInspector]
    public new EnumSlot slot = EnumSlot.PRIMARY;

    [HideInInspector]
    public new EnumAmmoType ammoType = EnumAmmoType.MEDIUM;
    [HideInInspector]
    public new EnumWeaponType type = EnumWeaponType.ASSAULT_RIFLE;
    [HideInInspector]
    public new EnumFireType fireType = EnumFireType.AUTO;

    [HideInInspector]
    public new int ammo = 30;
    [HideInInspector]
    public new int maxAmmo = 30;
    [HideInInspector]
    public new bool infiniteAmmo = false;

    [HideInInspector]
    public new float fireCooldown = 0.1f;
    [HideInInspector]
    public new float reloadTime = 5f;

    [HideInInspector]
    public new int damage = 25;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    PlayerController pc;

    float bulletSpeed = 200;

    new void Start () {
        pc = GetComponent<PlayerController>();
        bulletPrefab = ProjectileManager.projectileManager.GetProjectileFromRegistry("Bullet");
    }

    new void Update () {
        if (!isLocalPlayer || !weaponGObject.activeInHierarchy || pc.pauseController.isPaused)
            return;

        if (Input.GetButton("Fire1")) {

            //If we are out of ammo, reload and forbid firing until reload time is over
            if (ammo <= 0 && !infiniteAmmo) {
 
                nextFireTime = Time.time + reloadTime;
 
                Reload();
                return;
            }

            //If we still need to wait until the next fire, then don't do anything
            if (Time.time < nextFireTime)
                return;

            Fire();
        }
        pc.ammoText.text = ammo+"/"+maxAmmo;
    }

    public new void End () {
    }

    public new void Fire () {
        CmdInstantiateAndAccelerate();
        ammo--;
        nextFireTime = Time.time + fireCooldown; 
    }

    public new void Reload () {
        ammo = maxAmmo;
    }

    [Command]
    public void CmdInstantiateAndAccelerate () {
        //Create a new bullet GameObject
        GameObject bullet = (GameObject)Object.Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.playerFired = pc;
        bulletComponent.damage = damage;

        //Work out the direction to shoot it in
        Ray ray = new Ray(pc.cam.transform.position+pc.cam.transform.forward, pc.cam.transform.forward);
        RaycastHit hit;
        Vector3 direction;

        Vector3 aimPoint = Physics.Raycast(ray, out hit, 100) ? hit.point : pc.cam.transform.position+pc.cam.transform.forward*100;
        direction = (aimPoint-bulletSpawn.position).normalized;

        bullet.transform.LookAt(direction);

        //Give it a push
        bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

        //Spawn it on the network
        NetworkServer.Spawn(bullet);
    }
}
