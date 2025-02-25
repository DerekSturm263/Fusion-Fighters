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

[CreateAssetMenu(menuName = "Quantum/PlayerState/InputState/BlockState", order = Quantum.EditorDefines.AssetMenuPriorityStart + 391)]
public partial class BlockStateAsset : InputStateAsset {
  public Quantum.BlockState Settings_BlockState;

  public override string AssetObjectPropertyPath => nameof(Settings_BlockState);
  
  public override Quantum.AssetObject AssetObject => Settings_BlockState;
  
  public override void Reset() {
    if (Settings_BlockState == null) {
      Settings_BlockState = new Quantum.BlockState();
    }
    base.Reset();
  }
}

public static partial class BlockStateAssetExts {
  public static BlockStateAsset GetUnityAsset(this BlockState data) {
    return data == null ? null : UnityDB.FindAsset<BlockStateAsset>(data);
  }
}
