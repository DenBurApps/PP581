using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class OpenSubscriptionView : MonoBehaviour
{
    private const string PriceAddText = "$";
    
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _deleteButton;

    [SerializeField] private GameObject _archiveToggle;
    
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private TMP_Text _smallNameText;
    [SerializeField] private TMP_Text _bigNameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _tariffText;
    [SerializeField] private TMP_Text _startDateText;
    [SerializeField] private TMP_Text _nextDateText;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action DeleteButtonClicked;
    public event Action EditButtonClicked;
    public event Action BackButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
        _editButton.onClick.AddListener(OnEditButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
        _editButton.onClick.RemoveListener(OnEditButtonClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetTypeText(string type)
    {
        _typeText.text = type;
    }

    public void SetName(string name)
    {
        _bigNameText.text = name;
        _smallNameText.text = name;
    }

    public void SetPriceText(string price)
    {
        _priceText.text = PriceAddText + price;
    }

    public void SetTariffText(string tariff)
    {
        _tariffText.text = tariff;
    }

    public void SetStartDate(string startDate)
    {
        _startDateText.text = startDate;
    }

    public void SetNextDate(string nextDate)
    {
        _nextDateText.text = nextDate;
    }

    public void ToggleArchivePlane(bool status)
    {
        _archiveToggle.gameObject.SetActive(status);
    }
    
    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnEditButtonClicked() => EditButtonClicked?.Invoke();
    private void OnDeleteButtonClicked() => DeleteButtonClicked?.Invoke();
}
