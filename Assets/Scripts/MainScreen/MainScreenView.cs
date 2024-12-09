using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class MainScreenView : MonoBehaviour
{
    [SerializeField] private Button _addButton;
    [SerializeField] private TMP_InputField _search;
    [SerializeField] private Button _sortButton;
    [SerializeField] private GameObject _emptyPlane;
    [SerializeField] private Button _archiveButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _subscribtionButton;
    [SerializeField] private Button _searchClearedButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action AddButtonClicked;
    public event Action SortButtonClicked;
    public event Action ArchiveButtonClicked;
    public event Action SubscribtionButtonClicked;
    public event Action SettingsButtonClicked;
    public event Action<string> SearchInputed;
    public event Action SearchIsEmpty;

    public bool ArchiveButtonPressed { get; private set; }

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _addButton.onClick.AddListener(OnAddButtonClicked);
        _sortButton.onClick.AddListener(OnSortButtonClicked);
        _archiveButton.onClick.AddListener(OnArchiveButtonClicked);
        _subscribtionButton.onClick.AddListener(OnSubscribtionButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _search.onValueChanged.AddListener(OnSearchInputed);

        DisableClearButton();
    }

    private void OnDisable()
    {
        _addButton.onClick.RemoveListener(OnAddButtonClicked);
        _sortButton.onClick.RemoveListener(OnSortButtonClicked);
        _archiveButton.onClick.RemoveListener(OnArchiveButtonClicked);
        _subscribtionButton.onClick.RemoveListener(OnSubscribtionButtonClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        _searchClearedButton.onClick.RemoveListener(OnSearchClearedClicked);
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

    private void OnAddButtonClicked() => AddButtonClicked?.Invoke();
    private void OnSortButtonClicked() => SortButtonClicked?.Invoke();

    private void OnArchiveButtonClicked()
    {
        ArchiveButtonPressed = true;
        ArchiveButtonClicked?.Invoke();
    }

    private void OnSubscribtionButtonClicked()
    {
        ArchiveButtonPressed = false;
        SubscribtionButtonClicked?.Invoke();
    }

    private void OnSettingsButtonClicked() => SettingsButtonClicked?.Invoke();

    private void OnSearchInputed(string text)
    {
        SearchInputed?.Invoke(text);

        if (string.IsNullOrEmpty(_search.text))
        {
            SearchIsEmpty?.Invoke();
            DisableClearButton();
        }
        else
        {
            EnableClearButton();
        }
    }

    private void OnSearchClearedClicked()
    {
        _search.text = string.Empty;
        SearchIsEmpty?.Invoke();
    }

    private void EnableClearButton()
    {
        _searchClearedButton.gameObject.SetActive(true);
        _searchClearedButton.onClick.AddListener(OnSearchClearedClicked);
    }

    private void DisableClearButton()
    {
        _searchClearedButton.gameObject.SetActive(false);
        _searchClearedButton.onClick.RemoveListener(OnSearchClearedClicked);
    }
}