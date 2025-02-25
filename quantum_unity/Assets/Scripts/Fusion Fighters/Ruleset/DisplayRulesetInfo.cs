using Extensions.Components.UI;
using UnityEngine;
using UnityEngine.Events;

using Type = SerializableWrapper<Quantum.Ruleset>;

public class DisplayRulesetInfo : Display<Type, Extensions.Types.Tuple<UnityEvent<string>[], UnityEvent<Sprite>>>
{
    [SerializeField] protected string _format = "{0}";

    protected (string[], Sprite) GetInfo(Type item) => (new string[] { item.Name, item.Description }, item.Preview);
    public override void UpdateDisplay(Type item)
    {
        var info = GetInfo(item);

        _component.Item1[0].Invoke(string.Format(_format, info.Item1[0]));
        _component.Item1[1].Invoke(string.Format(_format, info.Item1[1]));
    }

    protected override Type GetValue() => RulesetController.Instance.CurrentRuleset;
}
