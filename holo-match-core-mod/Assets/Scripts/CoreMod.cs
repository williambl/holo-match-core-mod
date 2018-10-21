using UnityEngine;

public class CoreMod : HoloMod {

    public string name = "CoreMod";

    public string version = "v0.1";

    public override void RegisterWeapons(WeaponManager manager) {
        AssetBundle bundle = assetBundles.Find(x => x.name == "core-weapons");
        string[] names = bundle.GetAllAssetNames();
        foreach (string name in names) {
            if (name.EndsWith(".prefab")) {
                Debug.Log("Adding weapon " + name);
                manager.AddWeaponToRegistry((GameObject)bundle.LoadAsset(name));
            }
        }
    }

    public override void RegisterMaps(MapManager manager) {
    }

    public override void RegisterProjectiles(ProjectileManager manager) {
        AssetBundle bundle = assetBundles.Find(x => x.name == "core-projectiles");
        string[] names = bundle.GetAllAssetNames();
        foreach (string name in names) {
            if (name.EndsWith(".prefab")) {
                Debug.Log("Adding projectile " + name);
                Debug.Log(manager.name);
                manager.AddProjectileToRegistry((GameObject)bundle.LoadAsset(name));
            }
        }

    }
    
}
