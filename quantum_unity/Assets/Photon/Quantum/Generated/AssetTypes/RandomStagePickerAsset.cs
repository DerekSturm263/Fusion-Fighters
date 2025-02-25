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

[CreateAssetMenu(menuName = "Quantum/InfoAsset/StagePicker/RandomStagePicker", order = Quantum.EditorDefines.AssetMenuPriorityStart + 225)]
public partial class RandomStagePickerAsset : StagePickerAsset {
  public Quantum.RandomStagePicker Settings_RandomStagePicker;

  public override string AssetObjectPropertyPath => nameof(Settings_RandomStagePicker);
  
  public override Quantum.AssetObject AssetObject => Settings_RandomStagePicker;
  
  public override void Reset() {
    if (Settings_RandomStagePicker == null) {
      Settings_RandomStagePicker = new Quantum.RandomStagePicker();
    }
    base.Reset();
  }
}

public static partial class RandomStagePickerAssetExts {
  public static RandomStagePickerAsset GetUnityAsset(this RandomStagePicker data) {
    return data == null ? null : UnityDB.FindAsset<RandomStagePickerAsset>(data);
  }
}
