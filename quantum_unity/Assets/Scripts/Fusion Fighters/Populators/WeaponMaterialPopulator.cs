using Extensions.Miscellaneous;
using UnityEngine;

public class WeaponMaterialPopulator : PopulateAsset<WeaponMaterialAsset>
{
    protected override string FilePath() => "DB/Assets/Build/Equipment/Weapons/Weapons/Materials";

    protected override void Decorate(GameObject buttonObj, WeaponMaterialAsset item)
    {
        base.Decorate(buttonObj, item);

        int countNum = InventoryController.Instance.GetItemCount(item);

        TMPro.TMP_Text count = buttonObj.FindChildWithTag("Count", false)?.GetComponent<TMPro.TMP_Text>();
        if (count)
            count.SetText(countNum.ToString());

        if (countNum == 0)
            count.color = Color.red;
    }

    protected override bool GiveEvents(WeaponMaterialAsset item) => InventoryController.Instance.GetItemCount(item) > 0;

    protected override bool IsEquipped(WeaponMaterialAsset item) => false;
}
