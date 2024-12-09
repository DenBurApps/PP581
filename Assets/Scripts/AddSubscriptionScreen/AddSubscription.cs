using System;
using UnityEngine;

public class AddSubscription : MonoBehaviour
{
    [SerializeField] private AddSubscriptionView _view;
    [SerializeField] private ScreenStateManager _screenStateManager;

    private string _name;
    private string _startDate;
    private string _nextDate;
    private string _price;
    private string _tariff;

    public event Action BackButtonClicked;
    public event Action<SubscriptionData> SavedButtonClicked; 
    
    private void Start()
    {
        _view.Disable();
        ResetData();
    }

    private void OnEnable()
    {
        _screenStateManager.AddSubscriptionOpen += OpenScreen;
        _view.SaveButtonClicked += SaveButtonClicked;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.NameInputed += OnNameInputed;
        _view.PriceInputed += OnPriceInputed;
        _view.TariffInputed += OnTariffInputed;
        _view.StartPaymentChanged += OnStartDateInputed;
        _view.NextPaymentChanged += OnNextPaymentDateInputed;

    }

    private void OnDisable()
    {
        _screenStateManager.AddSubscriptionOpen -= OpenScreen;
        _view.SaveButtonClicked -= SaveButtonClicked;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.NameInputed -= OnNameInputed;
        _view.PriceInputed -= OnPriceInputed;
        _view.TariffInputed -= OnTariffInputed;
        _view.StartPaymentChanged -= OnStartDateInputed;
        _view.NextPaymentChanged -= OnNextPaymentDateInputed;
    }

    private void OpenScreen()
    {
        _view.Enable();
    }

    private void OnNameInputed(string name)
    {
        _name = name;
        ValidateInput();
    }

    private void OnPriceInputed(string price)
    {
        _price = price;
        ValidateInput();
    }

    private void OnTariffInputed(string tariff)
    {
        _tariff = tariff;
        ValidateInput();
    }

    private void OnStartDateInputed(string startDate)
    {
        _startDate = startDate;
        ValidateInput();
    }

    private void OnNextPaymentDateInputed(string nextDate)
    {
        _nextDate = nextDate;
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_price) &&
                       !string.IsNullOrEmpty(_startDate) &&
                       !string.IsNullOrEmpty(_nextDate) && !string.IsNullOrEmpty(_tariff);
        
        _view.SetSaveButtonInteractable(isValid);
    }

    private void ResetData()
    {
        _name = string.Empty;
        _startDate = string.Empty;
        _nextDate = string.Empty;
        _price = string.Empty;
        _tariff = string.Empty;
        
        _view.SetName(_name);
        _view.SetTariff(_tariff);
        _view.SetPrice(_price);
        _view.SetCurrentDate();
        _view.CloseCalendar();
    }

    private void SaveButtonClicked()
    {
        SubscriptionData subscriptionData = new SubscriptionData(_name, _startDate, _nextDate, _price, _tariff);
        
        SavedButtonClicked?.Invoke(subscriptionData);
        OnBackButtonClicked();
    }

    private void OnBackButtonClicked()
    {
        ResetData();
        BackButtonClicked?.Invoke();
        _view.Disable();
    }
}