using UnityEngine;

public class CoreMod : HoloMod {

    public string name = "CoreMod";

    public string version = "v0.1";

    public override void RegisterWeapons(WeaponManager manager) {
        foreach (AssetBundle assetBundle in assetBundles) {
            Debug.Log(assetBundle.name);
        }
    }

    public override void RegisterMaps(MapManager manager) {
    }

    public override void RegisterProjectiles(ProjectileManager manager) {
    }
    
}
