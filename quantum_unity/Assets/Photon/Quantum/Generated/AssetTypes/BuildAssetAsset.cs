// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial  
// declarations in another file.
// </auto-generated>

using Quantum;
using UnityEngine;

[CreateAssetMenu(menuName = "Quantum/InfoAsset/BuildAsset", order = Quantum.EditorDefines.AssetMenuPriorityStart + 209)]
public partial class BuildAssetAsset : InfoAssetAsset {
  public Quantum.BuildAsset Settings_BuildAsset;

  public override string AssetObjectPropertyPath => nameof(Settings_BuildAsset);
  
  public override Quantum.AssetObject AssetObject => Settings_BuildAsset;
  
  public override void Reset() {
    if (Settings_BuildAsset == null) {
      Settings_BuildAsset = new Quantum.BuildAsset();
    }
    base.Reset();
  }
}

public static partial class BuildAssetAssetExts {
  public static BuildAssetAsset GetUnityAsset(this BuildAsset data) {
    return data == null ? null : UnityDB.FindAsset<BuildAssetAsset>(data);
  }
}
