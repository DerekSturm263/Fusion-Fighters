﻿using Quantum;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Extensions.Components.UI
{
    [AddComponentMenu("UI/Timer", 79)]
    [RequireComponent(typeof(TMPro.TMP_Text))]
    [DisallowMultipleComponent]
    public class TimerDisplay : UIBehaviour
    {
        private System.TimeSpan _time;

        [SerializeField] private string _format = "hh':'mm':'ss";
        [SerializeField] private bool _trimStart;
        [SerializeField] private bool _trimEnd;
        [SerializeField] private bool _useBeginningCountdown;
        [SerializeField] private bool _showDuringEditMode;

        [SerializeField] private Types.Dictionary<int, UnityEvent<int>> _tickEvents;
        [SerializeField] private UnityEvent<string> _onTick;

        public void InvokeTickEvent(int time)
        {
            if (isActiveAndEnabled && _tickEvents.TryGetValue(time, out UnityEvent<int> unityEvent))
                unityEvent.Invoke(time);
        }

        protected override unsafe void Awake()
        {
            if (_useBeginningCountdown)
                QuantumEvent.Subscribe<EventOnBeginningCountdown>(listener: this, handler: e => UpdateTimer(e.Time, true, e.InvokeEvents));
            else
                QuantumEvent.Subscribe<EventOnTimerTick>(listener: this, handler: e => UpdateTimer(e.Time, true, e.InvokeEvents));

            if (_showDuringEditMode)
                UpdateTimer(QuantumRunner.Default.Game.Frames.Verified.Global->CurrentMatch.Ruleset.Match.Time, true, false);
        }

        private void UpdateTimer(int time, bool invokeTickEvent, bool invokeSpecialEvents)
        {
            _time = System.TimeSpan.FromSeconds(time);

            string text = new(_time.ToString(_format));
            if (_trimStart)
                text = text.TrimStart('0');
            if (_trimEnd)
                text = text.TrimEnd('0');

            if (invokeTickEvent)
                _onTick.Invoke(text);

            if (invokeSpecialEvents)
                InvokeTickEvent(time);
        }
    }
}
