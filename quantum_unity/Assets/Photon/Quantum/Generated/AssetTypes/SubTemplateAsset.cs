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

[CreateAssetMenu(menuName = "Quantum/InfoAsset/SubTemplate", order = Quantum.EditorDefines.AssetMenuPriorityStart + 226)]
public partial class SubTemplateAsset : InfoAssetAsset {
  public Quantum.SubTemplate Settings_SubTemplate;

  public override string AssetObjectPropertyPath => nameof(Settings_SubTemplate);
  
  public override Quantum.AssetObject AssetObject => Settings_SubTemplate;
  
  public override void Reset() {
    if (Settings_SubTemplate == null) {
      Settings_SubTemplate = new Quantum.SubTemplate();
    }
    base.Reset();
  }
}

public static partial class SubTemplateAssetExts {
  public static SubTemplateAsset GetUnityAsset(this SubTemplate data) {
    return data == null ? null : UnityDB.FindAsset<SubTemplateAsset>(data);
  }
}
