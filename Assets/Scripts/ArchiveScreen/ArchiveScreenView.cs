using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ArchiveScreenView : MonoBehaviour
{
    [SerializeField] private Button _subscriptionButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private GameObject _emptyPlane;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action SubscriptionButtonClicked;
    public event Action SettingsButtonClicked;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _subscriptionButton.onClick.AddListener(OnSubscriptionsButtonClicked);
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        _subscriptionButton.onClick.RemoveListener(OnSubscriptionsButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void ToggleEmptyPlane(bool status)
    {
        _emptyPlane.gameObject.SetActive(status);
    }

    private void OnSettingsButtonClicked() => SettingsButtonClicked?.Invoke();
    private void OnSubscriptionsButtonClicked() => SubscriptionButtonClicked?.Invoke();
}
