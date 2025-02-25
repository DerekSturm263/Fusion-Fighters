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

[CreateAssetMenu(menuName = "Quantum/InfoAsset/Behavior", order = Quantum.EditorDefines.AssetMenuPriorityStart + 209)]
public partial class BehaviorAsset : InfoAssetAsset {
  public Quantum.Behavior Settings_Behavior;

  public override string AssetObjectPropertyPath => nameof(Settings_Behavior);
  
  public override Quantum.AssetObject AssetObject => Settings_Behavior;
  
  public override void Reset() {
    if (Settings_Behavior == null) {
      Settings_Behavior = new Quantum.Behavior();
    }
    base.Reset();
  }
}

public static partial class BehaviorAssetExts {
  public static BehaviorAsset GetUnityAsset(this Behavior data) {
    return data == null ? null : UnityDB.FindAsset<BehaviorAsset>(data);
  }
}
