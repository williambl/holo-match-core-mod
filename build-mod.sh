mod_name=CoreMod
mod_dir=holo-match-core-mod

if [ ! -d $1 ]
then
    echo Path to holomatch_data:
    read data_dir
else
    data_dir=$1
fi

if [ -d $mod_name ]
then
    echo Please remove $mod_name directory. I won\'t do it in case i accidentally remove / or something.
    exit
else
    mkdir $mod_name
fi

echo Copying AssetBundles...
find ./$mod_dir/Assets/AssetBundles/ -name *.assetbundle -exec cp {} $mod_name \;

echo AssetBundles Copied.

echo Building C# Scripts...

mcs `find ./$mod_dir/Assets/Scripts/ -name *.cs -print | tr '\n' ' '` -out:$mod_name/$mod_name.dll -target:library -r:$data_dir/Managed/Assembly-CSharp.dll -r:$data_dir/Managed/UnityEngine.dll -r:$data_dir/Managed/UnityEngine.Networking.dll -r:$data_dir/Managed/UnityEngine.CoreModule.dll -r:$data_dir/Managed/UnityEngine.AssetBundleModule.dll -r:$data_dir/Managed/UnityEngine.PhysicsModule.dll -r:$data_dir/Managed/UnityEngine.UI.dll

echo C# Scripts Built!
