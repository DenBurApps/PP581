using System;
using Bitsplash.DatePicker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddSubscriptionView : MonoBehaviour
{
    [SerializeField] private TMP_InputField _serviceNameInput;
    [SerializeField] private TMP_InputField _priceInput;
    [SerializeField] private TMP_InputField _tariffInput;

    [SerializeField] private TMP_Text _paymentStartText;
    [SerializeField] private TMP_Text _nextPaymentText;

    [SerializeField] private Button _paymentStartDateButton;
    [SerializeField] private Button _paymentNextDateButton;

    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private DatePickerSettings _datePicker;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private Button _currentButton;

    public event Action BackButtonClicked;
    public event Action SaveButtonClicked;
    public event Action PaymentStartButtonClicked;
    public event Action PaymentEndButtonClicked;
    public event Action<string> NameInputed;
    public event Action<string> TariffInputed;
    public event Action<string> PriceInputed;
    public event Action<string> StartPaymentChanged;
    public event Action<string> NextPaymentChanged;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _serviceNameInput.onValueChanged.AddListener(OnNameInputed);
        _priceInput.onValueChanged.AddListener(OnPriceInputed);
        _tariffInput.onValueChanged.AddListener(OnTariffInputed);
        _paymentStartDateButton.onClick.AddListener((() => OpenCalendar(_paymentStartDateButton)));
        _paymentNextDateButton.onClick.AddListener((() => OpenCalendar(_paymentNextDateButton)));
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _serviceNameInput.onValueChanged.RemoveListener(OnNameInputed);
        _priceInput.onValueChanged.RemoveListener(OnPriceInputed);
        _tariffInput.onValueChanged.RemoveListener(OnTariffInputed);
        _paymentStartDateButton.onClick.RemoveListener((() => OpenCalendar(_paymentStartDateButton)));
        _paymentNextDateButton.onClick.RemoveListener((() => OpenCalendar(_paymentNextDateButton)));
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetName(string name)
    {
        _serviceNameInput.text = name;
    }

    public void SetPrice(string price)
    {
        _priceInput.text = price;
    }

    public void SetTariff(string tariff)
    {
        _tariffInput.text = tariff;
    }

    public void SetPaymentStartText(string text)
    {
        _paymentStartText.text = text;
    }

    public void SetNextPaymentText(string text)
    {
        _nextPaymentText.text = text;
    }

    public void SetCurrentDate()
    {
        string currentDate = DateTime.Now.ToString("dd.MM.yyyy");

        _paymentStartText.text = currentDate;
        _nextPaymentText.text = currentDate;
        StartPaymentChanged?.Invoke(_paymentStartText.text);
        NextPaymentChanged?.Invoke(_nextPaymentText.text);
    }

    public void SetSaveButtonInteractable(bool isInteractable)
    {
        _saveButton.interactable = isInteractable;
    }

    private void SetDate(TMP_Text textToSet, Action<string> eventToInvoke)
    {
        string text = "";
        var selection = _datePicker.Content.Selection;
        for (int i = 0; i < selection.Count; i++)
        {
            var date = selection.GetItem(i);
            text += date.ToString(format: "dd.MM.yyyy");
        }

        textToSet.text = text;
        eventToInvoke?.Invoke(textToSet.text);
    }

    private void OpenCalendar(Button dateButton)
    {
        _currentButton = dateButton;
        _datePicker.gameObject.SetActive(true);

        if (_currentButton == _paymentStartDateButton)
        {
            _datePicker.Content.OnSelectionChanged.RemoveAllListeners();
        
            _paymentStartDateButton.onClick.RemoveListener(() => OpenCalendar(_paymentStartDateButton));
            _paymentStartDateButton.onClick.AddListener(() => CloseCalendar(_paymentStartDateButton));
            
            _datePicker.Content.OnSelectionChanged.AddListener(() => SetDate(_paymentStartText, StartPaymentChanged));
        }
        else
        {
            _datePicker.Content.OnSelectionChanged.RemoveAllListeners();
        
            _paymentNextDateButton.onClick.RemoveListener(() => OpenCalendar(_paymentNextDateButton));
            _paymentNextDateButton.onClick.AddListener(() => CloseCalendar(_paymentNextDateButton));
            
            _datePicker.Content.OnSelectionChanged.AddListener(() => SetDate(_nextPaymentText, NextPaymentChanged));
        }
    }

    public void CloseCalendar(Button dateButton)
    {
        _datePicker.gameObject.SetActive(false);
        
        if (dateButton == _paymentStartDateButton)
        {
            _paymentStartDateButton.onClick.RemoveListener(() => CloseCalendar(_paymentStartDateButton));
            _paymentStartDateButton.onClick.AddListener(() => OpenCalendar(_paymentStartDateButton));
        
            _datePicker.Content.OnSelectionChanged.RemoveListener(() => SetDate(_paymentStartText, StartPaymentChanged));
        }
        else
        {
            _paymentNextDateButton.onClick.RemoveListener(() => CloseCalendar(_paymentNextDateButton));
            _paymentNextDateButton.onClick.AddListener(() => OpenCalendar(_paymentNextDateButton));
   
            _datePicker.Content.OnSelectionChanged.RemoveListener(() => SetDate(_nextPaymentText, NextPaymentChanged));
        }
    }

    public void CloseCalendar()
    {
        if (_currentButton == null)
        {
            _datePicker.gameObject.SetActive(false);
            return;
        }

        if (_currentButton == _paymentStartDateButton)
        {
            CloseCalendar(_paymentStartDateButton);
        }
        else
        {
            CloseCalendar(_paymentNextDateButton);
        }
    }

    private void OnNameInputed(string name) => NameInputed?.Invoke(name);
    private void OnPriceInputed(string price) => PriceInputed?.Invoke(price);
    private void OnTariffInputed(string tariff) => TariffInputed?.Invoke(tariff);
    private void OnBackButtonClicked() => BackButtonClicked?.Invoke();
    private void OnSaveButtonClicked() => SaveButtonClicked?.Invoke();
}