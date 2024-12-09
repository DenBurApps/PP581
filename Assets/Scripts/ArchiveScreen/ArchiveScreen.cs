using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ArchiveScreen : MonoBehaviour
{
    private const string SaveFileName = "SavedData2.json";
    
    [SerializeField] private ArchiveScreenView _view;
    [SerializeField] private List<FilledSubscriptionPlane> _filledSubscriptionPlanes;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private OpenSubscription _openSubscription;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private EditSubscription _editSubscription;

    private List<int> _availableIndexes = new List<int>();

    public event Action<FilledSubscriptionPlane> OpenedArchivedSubscription;
    public event Action<SubscriptionData> PlaneSetSubscribet;
    public event Action MainScreenClicked;
    public event Action SettingsClicked; 

    private string _saveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);
    
    private void Start()
    {
        DisableAllWindows();
        _view.Disable();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledSubscriptionPlanes.Count);
        LoadFilledWindowsData();
    }

    private void OnEnable()
    {
        _mainScreen.PlaneIsArchived += EnableArchiveSubscription;
        _openSubscription.DeleteArchivedSubscriptionButtonClicked += DeleteArchiveSubscription;
        _screenStateManager.ArchiveScreenOpen += _view.Enable;
        _editSubscription.SavedArchivedButtonClicked += _view.Enable;

        _view.SubscriptionButtonClicked += OnSubscriptionsClicked;
        _view.SettingsButtonClicked += OnSettingsClicked;
    }

    private void OnDisable()
    {
        _mainScreen.PlaneIsArchived -= EnableArchiveSubscription;
        _openSubscription.DeleteArchivedSubscriptionButtonClicked -= DeleteArchiveSubscription;
        _screenStateManager.ArchiveScreenOpen -= _view.Enable;
        _editSubscription.SavedArchivedButtonClicked -= _view.Enable;
        
        _view.SubscriptionButtonClicked -= OnSubscriptionsClicked;
        _view.SettingsButtonClicked -= OnSettingsClicked;
    }

    private void EnableArchiveSubscription(FilledSubscriptionPlane filledSubscriptionPlane)
    {
        if (filledSubscriptionPlane == null)
            throw new ArgumentNullException(nameof(filledSubscriptionPlane));

        if (filledSubscriptionPlane.Data != null && filledSubscriptionPlane.IsArchived)
        {
            if (_availableIndexes.Count > 0)
            {
                int availableIndex = _availableIndexes[0];
                _availableIndexes.RemoveAt(0);

                var currentPlane = _filledSubscriptionPlanes[availableIndex];

                if (currentPlane != null)
                {
                    currentPlane.Enable();
                    currentPlane.SetData(filledSubscriptionPlane.Data);
                    currentPlane.Archive();
                    currentPlane.OpenButtonClicked += OnOpenArchiveSubscription;
                    currentPlane.SetSubscribet += OnPlaneSetSubscriptedActive;
                }
            }
        }
        
        SaveFilledWindowsData();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledSubscriptionPlanes.Count);
    }

    private void DeleteArchiveSubscription(FilledSubscriptionPlane plane)
    {
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));

        _view.Enable();

        int index = _filledSubscriptionPlanes.IndexOf(plane);

        if (index >= 0 && !_availableIndexes.Contains(index))
        {
            _availableIndexes.Add(index);
        }

        plane.OpenButtonClicked -= OnOpenArchiveSubscription;
        plane.SetSubscribet -= OnPlaneSetSubscriptedActive;
        plane.Disable();

        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledSubscriptionPlanes.Count);

        SaveFilledWindowsData();
    }

    private void DisableAllWindows()
    {
        for (int i = 0; i < _filledSubscriptionPlanes.Count; i++)
        {
            _filledSubscriptionPlanes[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void OnOpenArchiveSubscription(FilledSubscriptionPlane filledSubscriptionPlane)
    {
        OpenedArchivedSubscription?.Invoke(filledSubscriptionPlane);
        _view.Disable();
    }

    private void OnPlaneSetSubscriptedActive(FilledSubscriptionPlane filledSubscriptionPlane)
    {
        if (!filledSubscriptionPlane.IsArchived)
        {
            PlaneSetSubscribet?.Invoke(filledSubscriptionPlane.Data);
            DeleteArchiveSubscription(filledSubscriptionPlane);
        }
    }

    private void OnSubscriptionsClicked()
    {
        MainScreenClicked?.Invoke();
        _view.Disable();
    }

    private void OnSettingsClicked()
    {
        SettingsClicked?.Invoke();
        _view.Disable();
    }
    
    private void SaveFilledWindowsData()
    {
        List<SubscriptionData> itemsToSave = new List<SubscriptionData>();

        foreach (var window in _filledSubscriptionPlanes)
        {
            if (window.Data != null)
            {
                itemsToSave.Add(window.Data);
            }
        }

        ActiveSubscriptionsDataList itemDataList = new ActiveSubscriptionsDataList(itemsToSave);
        string json = JsonUtility.ToJson(itemDataList, true);

        try
        {
            File.WriteAllText(_saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save trip data: " + e.Message);
        }
    }

    private void LoadFilledWindowsData()
    {
        if (File.Exists(_saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(_saveFilePath);
                ActiveSubscriptionsDataList loadedTripDataList = JsonUtility.FromJson<ActiveSubscriptionsDataList>(json);

                int windowIndex = 0;
                foreach (SubscriptionData subscriptionData in loadedTripDataList.Data)
                {
                    if (!subscriptionData.IsArchived)
                        continue;
                    
                    if (windowIndex < _filledSubscriptionPlanes.Count)
                    {
                        if (_availableIndexes.Count > 0)
                        {
                            int availableIndex = _availableIndexes[0];
                            var currentFilledItemPlane = _filledSubscriptionPlanes[availableIndex];
                            _availableIndexes.RemoveAt(0);

                            if (!currentFilledItemPlane.IsActive)
                            {
                                currentFilledItemPlane.Enable();
                                currentFilledItemPlane.SetData(subscriptionData);
                                currentFilledItemPlane.Archive();
                                currentFilledItemPlane.OpenButtonClicked += OnOpenArchiveSubscription;
                                currentFilledItemPlane.SetSubscribet += OnPlaneSetSubscriptedActive;
                            }
                        }
                    }
                }
                
                _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledSubscriptionPlanes.Count);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load trip data: " + e.Message);
            }
        }
    }
}