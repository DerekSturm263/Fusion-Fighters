using Extensions.Components.UI;
using Photon.Deterministic;
using Quantum;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DisplayBattleStats : Display<BattleStats, List<UnityEvent<string>>>
{
    [SerializeField] private int _globalIndex;
    public int GlobalIndex => _globalIndex;

    public override void UpdateDisplay(BattleStats item)
    {
        _component[0].Invoke(item.Kills.ToString());
        _component[1].Invoke(item.Deaths.ToString());
        _component[2].Invoke(item.TimeSurvived.AsInt.ToString());
        _component[3].Invoke(item.TotalDamageDealt.AsInt.ToString());
        _component[4].Invoke(item.TotalDamageTaken.AsInt.ToString());
        _component[5].Invoke(item.TotalDamageHealed.AsInt.ToString());
        _component[6].Invoke(item.TotalEnergyGenerated.AsInt.ToString());
        _component[7].Invoke(item.TotalEnergyConsumed.AsInt.ToString());
        _component[8].Invoke(item.SubUses.ToString());
        _component[9].Invoke(item.UltimateUses.ToString());
        _component[10].Invoke(item.ItemUses.ToString());
    }

    protected override BattleStats GetValue()
    {
        EntityRef entity = FighterIndex.GetFirstEntity(QuantumRunner.Default.Game.Frames.Verified, item => item.Global == _globalIndex);

        if (QuantumRunner.Default.Game.Frames.Verified.TryGet(entity, out PlayerStats stats))
            return stats.Stats;

        return default;
    }
}
