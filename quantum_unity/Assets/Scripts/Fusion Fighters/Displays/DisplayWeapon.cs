using Extensions.Components.UI;
using Extensions.Types;
using Quantum;
using UnityEngine;

using Type = SerializableWrapper<Quantum.Weapon>;

public class DisplayWeapon : DisplayTextAndImage<Type>
{
    [SerializeField] private int _weaponType;

    protected override Tuple<string, Sprite> GetInfo(Type item)
    {
        if (item.IsValid)
            return new(string.Format(_format, item.Name, item.Description), item.Icon);
        else
            return new(string.Format(_format, "None", ""), null);
    }

    protected override Type GetValue()
    {
        AssetGuid id = _weaponType switch
        {
            0 => BuildController.Instance.CurrentBuild.value.Gear.MainWeapon.FileGuid,
            1 => BuildController.Instance.CurrentBuild.value.Gear.AltWeapon.FileGuid,
            _ => default
        };

        if (id.IsValid)
            return Type.LoadAs(WeaponController.GetPath(), id);
        else
            return default;
    }

    public void Clear()
    {
        _component.Item1.Invoke(string.Format(_format, "None", ""));
        _component.Item2.Invoke(null);
    }

    public void DisplayEmpty()
    {
        _component.Item1.Invoke(string.Format(_format, "", ""));
        _component.Item2.Invoke(null);
    }
}
