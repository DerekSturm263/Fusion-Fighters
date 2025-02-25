﻿using Extensions.Miscellaneous;
using Photon.Deterministic;
using Quantum;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDPlayerLink : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _name;
    [SerializeField] private TMPro.TMP_Text _playerNum;
    [SerializeField] private Image _health;
    [SerializeField] private HorizontalLayoutGroup _stocks;
    [SerializeField] private Image _energy;
    [SerializeField] private Material _activateUltimate;
    [SerializeField] private Material _lowHealth;
    [SerializeField] private GameObject _lowHealthObj;
    [SerializeField] private Image _portrait;
    [SerializeField] private TMPro.TMP_Text _ready;
    [SerializeField] private Image _readyFill;
    [SerializeField] private TMPro.TMP_Text _lifeCount;
    [SerializeField] private Image _background;

    [SerializeField] private TMPro.TMP_Text _healthText;
    [SerializeField] private TMPro.TMP_Text _energyText;

    [Header("Settings")]
    [SerializeField] private Color _emptyHealth;
    [SerializeField] private Color _fullHealth;
    [SerializeField] private Color _emptyEnergy;
    [SerializeField] private Color _fullEnergy;
    [SerializeField] private Color _emptyStock;
    [SerializeField] private Color _fullStock;
    [SerializeField] private Color _emptyStockPortrait;

    private float _healthRatio, _energyRatio, _stockRatio;

    private EntityRef _entity;
    public void SetEntity(EntityRef entity) => _entity = entity;

    [SerializeField] private GameObject _displayObj;
    [SerializeField] private DisplayAllBuildInfo _display;

    private unsafe void Update()
    {
        if (!_health || !_energy)
            return;

        _health.fillAmount = _healthRatio;
        _energy.fillAmount = _energyRatio;

        _health.color = Color.Lerp(_emptyHealth, _fullHealth, _health.fillAmount);
        _energy.color = Color.Lerp(_emptyEnergy, _fullEnergy, _energy.fillAmount);

        if (_energyRatio == 1)
        {
            _portrait.material = _activateUltimate;
        }
        else
        {
            if (_healthRatio > 0 && _healthRatio < 0.2f && QuantumRunner.Default.Game.Frames.Verified.Global->IsMatchRunning)
                _portrait.material = _lowHealth;
            else
                _portrait.material = null;

            if (_stockRatio > 0 || !QuantumRunner.Default.Game.Frames.Verified.Global->IsMatchRunning)
                _portrait.color = Color.white;
            else
                _portrait.color = _emptyStockPortrait;
        }

        _lowHealthObj.SetActive(_health.fillAmount <= 0.2f);
    }

    public void SetPlayerNumber(FighterIndex index)
    {
        _playerNum.SetText(index.Type == FighterType.Human ? $"P{index.GlobalNoBots + 1}" : "Bot");

        if (_background)
        {
            _background.color = index.GetDarkColor(QuantumRunner.Default.Game.Frames.Verified).ToColor();
        }
    }

    public void SetPlayerColor(FighterIndex index)
    {
        if (_background)
        {
            _background.color = index.GetDarkColor(QuantumRunner.Default.Game.Frames.Verified).ToColor();
        }
    }

    public void SetPlayerName(string name)
    {
        _name.SetText(name);
    }

    public void SetPlayerIconIndex(FighterIndex index)
    {
        GameObject player = FindFirstObjectByType<EntityViewUpdater>().GetView(FighterIndex.GetPlayerFromIndex(QuantumRunner.Default.Game.Frames.Verified, index)).gameObject;
        CoroutineRunner.Instance.StartCoroutine(TakePicture(player, index.Global));
    }

    IEnumerator TakePicture(GameObject player, int index)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Texture2D portraitTexture = player.GetComponentInChildren<Camera>().RenderToTexture2D(FindFirstObjectByType<PlayerSpawnEventListener>().PlayerIcons[index], TextureFormat.RGBA32, true, player.GetComponent<CustomQuantumAnimator>().Direction == -1);
        Sprite portrait = Sprite.Create(portraitTexture, new(0, 0, portraitTexture.width, portraitTexture.height), Vector2.one);
        
        _portrait.sprite = portrait;
    }

    public void UpdateReadiness(bool isReady)
    {
        if (_ready)
            _ready.SetText(isReady ? "Ready!" : "Not Yet...");
    }

    public void UpdateReadinessValue(float value)
    {
        if (_readyFill)
            _readyFill.fillAmount = value;
    }

    public void ShowReadiness(bool show)
    {
        if (_readyFill)
            _readyFill.gameObject.SetActive(show);
    }

    public void UpdateHealth(FP newHealth, FP maxHealth)
    {
        if (maxHealth != 0)
            _healthRatio = (newHealth / maxHealth).AsFloat;
        else
            _healthRatio = 0;

        _healthText?.SetText($"{newHealth.AsInt}/{maxHealth.AsInt}");
    }

    public void UpdateEnergy(FP newEnergy, FP maxEnergy)
    {
        if (maxEnergy != 0)
            _energyRatio = (newEnergy / maxEnergy).AsFloat;
        else
            _energyRatio = 0;

        _energyText?.SetText($"{newEnergy.AsInt}/{maxEnergy.AsInt}");
    }

    public unsafe void UpdateStocks(int newStocks, int maxStocks)
    {
        bool showLifeAsNumber = maxStocks > 10 || maxStocks == -1;

        if (!_stocks)
            return;

        _stocks.gameObject.SetActive(!showLifeAsNumber);
        
        if (_lifeCount)
            _lifeCount.gameObject.SetActive(showLifeAsNumber);

        _stockRatio = maxStocks == -1 ? 1 : (float)newStocks / maxStocks;

        if (showLifeAsNumber)
        {
            if (maxStocks == -1)
                _lifeCount.SetText($"Lives: \u221E");
            else
                _lifeCount.SetText($"Lives: {newStocks}/{maxStocks}");
        }
        else
        {
            for (int i = 0; i < maxStocks; ++i)
            {
                _stocks.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = maxStocks; i < _stocks.transform.childCount; ++i)
            {
                _stocks.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 0; i < maxStocks; ++i)
                _stocks.transform.GetChild(i).GetComponent<Image>().color = newStocks >= i + 1 ? _fullStock : _emptyStock;
        }
    }

    public void DisplayInfo()
    {
        Build build = default;

        if (QuantumRunner.Default.Game.Frames.Verified.TryGet(_entity, out PlayerStats stats))
        {
            build = stats.Build;
        }

        DisplayBuildInfo(build);
    }

    private void DisplayBuildInfo(Build build)
    {
        EventSystemController.Instance.Enable();

        _displayObj.SetActive(true);
        _display.UpdateDisplay(build);
    }
}
