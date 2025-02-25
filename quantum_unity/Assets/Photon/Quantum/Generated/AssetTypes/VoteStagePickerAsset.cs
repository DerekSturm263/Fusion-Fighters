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

[CreateAssetMenu(menuName = "Quantum/InfoAsset/StagePicker/VoteStagePicker", order = Quantum.EditorDefines.AssetMenuPriorityStart + 229)]
public partial class VoteStagePickerAsset : StagePickerAsset {
  public Quantum.VoteStagePicker Settings_VoteStagePicker;

  public override string AssetObjectPropertyPath => nameof(Settings_VoteStagePicker);
  
  public override Quantum.AssetObject AssetObject => Settings_VoteStagePicker;
  
  public override void Reset() {
    if (Settings_VoteStagePicker == null) {
      Settings_VoteStagePicker = new Quantum.VoteStagePicker();
    }
    base.Reset();
  }
}

public static partial class VoteStagePickerAssetExts {
  public static VoteStagePickerAsset GetUnityAsset(this VoteStagePicker data) {
    return data == null ? null : UnityDB.FindAsset<VoteStagePickerAsset>(data);
  }
}
