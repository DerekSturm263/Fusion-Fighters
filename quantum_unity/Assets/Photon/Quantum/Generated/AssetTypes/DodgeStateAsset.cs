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

[CreateAssetMenu(menuName = "Quantum/PlayerState/ActionState/DodgeState", order = Quantum.EditorDefines.AssetMenuPriorityStart + 393)]
public partial class DodgeStateAsset : ActionStateAsset {
  public Quantum.DodgeState Settings_DodgeState;

  public override string AssetObjectPropertyPath => nameof(Settings_DodgeState);
  
  public override Quantum.AssetObject AssetObject => Settings_DodgeState;
  
  public override void Reset() {
    if (Settings_DodgeState == null) {
      Settings_DodgeState = new Quantum.DodgeState();
    }
    base.Reset();
  }
}

public static partial class DodgeStateAssetExts {
  public static DodgeStateAsset GetUnityAsset(this DodgeState data) {
    return data == null ? null : UnityDB.FindAsset<DodgeStateAsset>(data);
  }
}
