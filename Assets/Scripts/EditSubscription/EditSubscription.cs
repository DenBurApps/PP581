using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSubscription : MonoBehaviour
{
    [SerializeField] private EditSubscriptionView _view;
    [SerializeField] private OpenSubscription _openScreen;

    private string _newName;
    private string _newStartDate;
    private string _newNextDate;
    private string _newPrice;
    private string _newTariff;

    private FilledSubscriptionPlane _filledSubscriptionPlane;

    public event Action BackButtonClicked;
    public event Action SavedActiveSubscriptionButtonClicked;
    public event Action SavedArchivedButtonClicked;

    private void Start()
    {
        _view.Disable();
        ResetData();
    }

    private void OnEnable()
    {
        _openScreen.EditButtonClicked += OpenScreen;
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
        _openScreen.EditButtonClicked -= OpenScreen;
        _view.SaveButtonClicked -= SaveButtonClicked;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.NameInputed -= OnNameInputed;
        _view.PriceInputed -= OnPriceInputed;
        _view.TariffInputed -= OnTariffInputed;
        _view.StartPaymentChanged -= OnStartDateInputed;
        _view.NextPaymentChanged -= OnNextPaymentDateInputed;
    }

    private void OpenScreen(FilledSubscriptionPlane filledSubscriptionPlane)
    {
        if (filledSubscriptionPlane == null)
            throw new ArgumentNullException(nameof(filledSubscriptionPlane));

        _filledSubscriptionPlane = filledSubscriptionPlane;

        _view.SetName(_filledSubscriptionPlane.Data.ServiceName);
        _view.SetPaymentStartText(_filledSubscriptionPlane.Data.Date);
        _view.SetNextPaymentText(_filledSubscriptionPlane.Data.NextPaymentDate);
        _view.SetPrice(_filledSubscriptionPlane.Data.Price);
        _view.SetTariff(_filledSubscriptionPlane.Data.Tariff);
        _view.CloseCalendar();

        _view.Enable();
    }

    private void OnNameInputed(string name)
    {
        _newName = name;
        ValidateInput();
    }

    private void OnPriceInputed(string price)
    {
        _newPrice = price;
        ValidateInput();
    }

    private void OnTariffInputed(string tariff)
    {
        _newTariff = tariff;
        ValidateInput();
    }

    private void OnStartDateInputed(string startDate)
    {
        _newStartDate = startDate;
        ValidateInput();
    }

    private void OnNextPaymentDateInputed(string nextDate)
    {
        _newNextDate = nextDate;
        ValidateInput();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_newName) || !string.IsNullOrEmpty(_newPrice) ||
                       !string.IsNullOrEmpty(_newStartDate) ||
                       !string.IsNullOrEmpty(_newNextDate) || !string.IsNullOrEmpty(_newTariff);

        _view.SetSaveButtonInteractable(isValid);
    }

    private void ResetData()
    {
        _newName = string.Empty;
        _newStartDate = string.Empty;
        _newNextDate = string.Empty;
        _newPrice = string.Empty;
        _newTariff = string.Empty;

        _view.SetName(_newName);
        _view.SetTariff(_newTariff);
        _view.SetPrice(_newPrice);
        _view.SetCurrentDate();
        _view.CloseCalendar();
    }

    private void SaveButtonClicked()
    {
        SubscriptionData subscriptionData =
            new SubscriptionData(_newName, _newStartDate, _newNextDate, _newPrice, _newTariff);
        _filledSubscriptionPlane.SetData(subscriptionData);

        if (!_filledSubscriptionPlane.IsArchived)
        {
            SavedActiveSubscriptionButtonClicked?.Invoke();
        }
        else
        {
            SavedArchivedButtonClicked?.Invoke();
        }

        ResetData();
    }

    private void OnBackButtonClicked()
    {
        ResetData();
        BackButtonClicked?.Invoke();
        _view.Disable();
    }
}