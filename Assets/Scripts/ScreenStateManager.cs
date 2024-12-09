using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenStateManager : MonoBehaviour
{
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private AddSubscription _addSubscriptionScreen;
    [SerializeField] private ArchiveScreen _archiveScreen;
    [SerializeField] private SettingsScreen _settingsScreen;

    public event Action MainScreenOpen;
    public event Action AddSubscriptionOpen;
    public event Action ArchiveScreenOpen;
    public event Action SettingScreenClicked;

    private void OnEnable()
    {
        _mainScreen.AddSubscription += OnAddSubscriptionOpen;
        _mainScreen.ArchivedClicked += OnArchiveScreenOpen;
        _mainScreen.SettingsClicked += OnSettingsClicked;
        
        _addSubscriptionScreen.BackButtonClicked += OnMainScreenOpen;

        _archiveScreen.MainScreenClicked += OnMainScreenOpen;
        _archiveScreen.SettingsClicked += OnSettingsClicked;

        _settingsScreen.ArchiveClicked += OnArchiveScreenOpen;
        _settingsScreen.SubscriptionsClicked += OnMainScreenOpen;
    }

    private void OnDisable()
    {
        _mainScreen.AddSubscription -= OnAddSubscriptionOpen;
        _mainScreen.ArchivedClicked -= OnArchiveScreenOpen;
        _mainScreen.SettingsClicked -= OnSettingsClicked;
        
        _addSubscriptionScreen.BackButtonClicked -= OnMainScreenOpen;
        
        _archiveScreen.MainScreenClicked -= OnMainScreenOpen;
        _archiveScreen.SettingsClicked -= OnSettingsClicked;
        
        _settingsScreen.ArchiveClicked -= OnArchiveScreenOpen;
        _settingsScreen.SubscriptionsClicked -= OnMainScreenOpen;
    }

    private void OnMainScreenOpen() => MainScreenOpen?.Invoke();
    private void OnAddSubscriptionOpen() => AddSubscriptionOpen?.Invoke();
    private void OnArchiveScreenOpen() => ArchiveScreenOpen?.Invoke();
    private void OnSettingsClicked() => SettingScreenClicked?.Invoke();
}
