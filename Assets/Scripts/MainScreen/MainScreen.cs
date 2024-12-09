using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MainScreen : MonoBehaviour
{
    private const string SaveFileName = "SavedData1.json";

    [SerializeField] private SortPlane _sortPlane;
    [SerializeField] private MainScreenView _view;
    [SerializeField] private List<FilledSubscriptionPlane> _filledSubscriptionPlanes;
    [SerializeField] private ScreenStateManager _screenStateManager;
    [SerializeField] private AddSubscription _addSubscriptionScreen;
    [SerializeField] private OpenSubscription _openSubscriptionScreen;
    [SerializeField] private EditSubscription _editSubscription;
    [SerializeField] private ArchiveScreen _archiveScreen;

    private List<int> _availableIndexes = new List<int>();

    public event Action<FilledSubscriptionPlane> OpenFilledPlane;
    public event Action AddSubscription;
    public event Action<FilledSubscriptionPlane> PlaneIsArchived;
    public event Action ArchivedClicked;
    public event Action SettingsClicked;

    private string _saveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    private void Start()
    {
        _view.Enable();
        DisableAllWindows();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledSubscriptionPlanes.Count);
        _sortPlane.Disable();
        LoadFilledWindowsData();
    }

    private void OnEnable()
    {
        _view.SortButtonClicked += OnSortButtonClicked;
        _view.SearchInputed += SearchInputed;
        _view.SearchIsEmpty += EnableAllSubscriptionWindows;
        _view.AddButtonClicked += OnAddButtonClicked;
        _view.ArchiveButtonClicked += OnArchiveButtonClicked;
        _view.SettingsButtonClicked += OnSettingsClicked;

        _screenStateManager.MainScreenOpen += _view.Enable;

        _addSubscriptionScreen.SavedButtonClicked += EnableSubscribtion;

        _openSubscriptionScreen.DeleteActiveSubscriptionButtonClicked += DeleteActiveSubscriptionItemPlane;
        _openSubscriptionScreen.BackButtonClicked += _view.Enable;

        _editSubscription.SavedActiveSubscriptionButtonClicked += _view.Enable;

        _archiveScreen.PlaneSetSubscribet += EnableSubscribtion;
    }

    private void OnDisable()
    {
        _view.SearchInputed -= SearchInputed;
        _view.SortButtonClicked -= OnSortButtonClicked;
        _view.SearchIsEmpty -= EnableAllSubscriptionWindows;
        _view.AddButtonClicked -= OnAddButtonClicked;
        _view.ArchiveButtonClicked -= OnArchiveButtonClicked;
        _view.SettingsButtonClicked -= OnSettingsClicked;

        _sortPlane.SortByAscendingClicked -= SortByAscendingPrice;
        _sortPlane.SortByDesceningClicked -= SortByDecendingPrice;
        _sortPlane.SortByNameAzClicked -= SortByNameAZ;
        _sortPlane.SortByNameZaClicked -= SortByNameZA;

        _screenStateManager.MainScreenOpen -= _view.Enable;

        _addSubscriptionScreen.SavedButtonClicked -= EnableSubscribtion;

        _openSubscriptionScreen.DeleteActiveSubscriptionButtonClicked -= DeleteActiveSubscriptionItemPlane;
        _openSubscriptionScreen.BackButtonClicked -= _view.Enable;

        _editSubscription.SavedActiveSubscriptionButtonClicked -= _view.Enable;

        _archiveScreen.PlaneSetSubscribet -= EnableSubscribtion;
    }

    private void EnableSubscribtion(SubscriptionData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        if (_availableIndexes.Count > 0)
        {
            int availableIndex = _availableIndexes[0];
            _availableIndexes.RemoveAt(0);

            var currentFilledItemPlane = _filledSubscriptionPlanes[availableIndex];

            if (currentFilledItemPlane != null)
            {
                currentFilledItemPlane.Enable();
                currentFilledItemPlane.SetData(data);
                currentFilledItemPlane.Subscribe();
                currentFilledItemPlane.OpenButtonClicked += OnOpenFilledPlane;
                currentFilledItemPlane.SetArchived += OnPlaneArchived;
            }
        }

        SaveFilledWindowsData();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledSubscriptionPlanes.Count);
    }

    private void DeleteActiveSubscriptionItemPlane(FilledSubscriptionPlane plane)
    {
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));

        _view.Enable();

        int index = _filledSubscriptionPlanes.IndexOf(plane);

        if (index >= 0 && !_availableIndexes.Contains(index))
        {
            _availableIndexes.Add(index);
        }

        plane.OpenButtonClicked -= OnOpenFilledPlane;
        plane.SetArchived -= OnPlaneArchived;
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

    private void OnSortButtonClicked()
    {
        if (!_sortPlane.isActiveAndEnabled)
        {
            _sortPlane.Enable();
            _sortPlane.SortByAscendingClicked += SortByAscendingPrice;
            _sortPlane.SortByDesceningClicked += SortByDecendingPrice;
            _sortPlane.SortByNameAzClicked += SortByNameAZ;
            _sortPlane.SortByNameZaClicked += SortByNameZA;
        }
        else
        {
            _sortPlane.Disable();
            _sortPlane.SortByAscendingClicked -= SortByAscendingPrice;
            _sortPlane.SortByDesceningClicked -= SortByDecendingPrice;
            _sortPlane.SortByNameAzClicked -= SortByNameAZ;
            _sortPlane.SortByNameZaClicked -= SortByNameZA;
        }
    }

    private void SortByAscendingPrice()
    {
        if (!AnyPlaneActive())
            return;

        var sortedData = _filledSubscriptionPlanes
            .Where(plane => plane.Data != null)
            .Select(plane => plane.Data)
            .OrderBy(data =>
            {
                int.TryParse(data.Price, out int parsedPrice);
                return parsedPrice;
            })
            .ToList();

        RefillPlanesWithSortedData(sortedData);
    }

    private void SortByDecendingPrice()
    {
        if (!AnyPlaneActive())
            return;

        var sortedData = _filledSubscriptionPlanes
            .Where(plane => plane.Data != null)
            .Select(plane => plane.Data)
            .OrderByDescending(data =>
            {
                int.TryParse(data.Price, out int parsedPrice);
                return parsedPrice;
            })
            .ToList();

        RefillPlanesWithSortedData(sortedData);
    }

    private void SortByNameAZ()
    {
        if (!AnyPlaneActive())
            return;

        var sortedData = _filledSubscriptionPlanes
            .Where(plane => plane.Data != null)
            .Select(plane => plane.Data)
            .OrderBy(data => data.ServiceName)
            .ToList();

        RefillPlanesWithSortedData(sortedData);
    }

    private void SortByNameZA()
    {
        if (!AnyPlaneActive())
            return;

        var sortedData = _filledSubscriptionPlanes
            .Where(plane => plane.Data != null)
            .Select(plane => plane.Data)
            .OrderByDescending(data => data.ServiceName)
            .ToList();

        RefillPlanesWithSortedData(sortedData);
    }

    private void SearchInputed(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            EnableAllSubscriptionWindows();
            return;
        }

        string adaptedSearch = text.ToLower();

        foreach (var plane in _filledSubscriptionPlanes)
        {
            if (!plane.IsActive || !plane.IsArchived)
                continue;

            if (plane.Data.ServiceName.ToLower() == adaptedSearch)
            {
                plane.Enable();
            }
            else
            {
                plane.Disable();
            }
        }
    }

    private bool AnyPlaneActive()
    {
        return _filledSubscriptionPlanes.Any(plane => plane.gameObject.activeSelf);
    }

    private void RefillPlanesWithSortedData(List<SubscriptionData> sortedData)
    {
        DisableAllWindows();

        for (int i = 0; i < sortedData.Count; i++)
        {
            var plane = _filledSubscriptionPlanes[i];
            var data = sortedData[i];

            plane.SetData(data);

            if (_view.ArchiveButtonPressed && plane.IsArchived)
            {
                plane.Enable();
            }
            else if (!_view.ArchiveButtonPressed && !plane.IsArchived)
            {
                plane.Enable();
            }
            else
            {
                plane.Disable();
                _availableIndexes.Add(i);
            }
        }

        SaveFilledWindowsData();
    }

    private void EnableAllSubscriptionWindows()
    {
        foreach (var plane in _filledSubscriptionPlanes)
        {
            if (!plane.IsActive && !plane.IsArchived && plane.Data != null)
            {
                plane.Enable();
            }
            else
            {
                plane.Disable();
            }
        }
    }

    private void OnOpenFilledPlane(FilledSubscriptionPlane plane)
    {
        OpenFilledPlane?.Invoke(plane);
        _view.Disable();
    }

    private void OnPlaneArchived(FilledSubscriptionPlane plane)
    {
        if (plane.IsArchived)
        {
            PlaneIsArchived?.Invoke(plane);
            DeleteActiveSubscriptionItemPlane(plane);
        }
    }

    private void OnAddButtonClicked()
    {
        AddSubscription?.Invoke();
        _view.Disable();
    }

    private void OnArchiveButtonClicked()
    {
        ArchivedClicked?.Invoke();
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
                ActiveSubscriptionsDataList loadedTripDataList =
                    JsonUtility.FromJson<ActiveSubscriptionsDataList>(json);

                int windowIndex = 0;
                foreach (SubscriptionData subscriptionData in loadedTripDataList.Data)
                {
                    if (subscriptionData.IsArchived)
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
                                currentFilledItemPlane.Subscribe();
                                currentFilledItemPlane.OpenButtonClicked += OnOpenFilledPlane;
                                currentFilledItemPlane.SetArchived += OnPlaneArchived;
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

[Serializable]
public class ActiveSubscriptionsDataList
{
    public List<SubscriptionData> Data;

    public ActiveSubscriptionsDataList(List<SubscriptionData> data)
    {
        Data = data;
    }
}