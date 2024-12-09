using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilledSubscriptionPlane : MonoBehaviour
{
    private const string PriceAddText = "$";

    [SerializeField] private Sprite _activeSprite;
    [SerializeField] private Sprite _archivedSprite;
    [SerializeField] private Image _logo;

    [SerializeField] private TMP_Text _titlText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private TMP_Text _typeText;
    [SerializeField] private TMP_Text _date;
    [SerializeField] private TMP_Text _paymentDateText;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _archiveButton;

    public event Action<FilledSubscriptionPlane> OpenButtonClicked;
    public event Action<FilledSubscriptionPlane> SetArchived;
    public event Action<FilledSubscriptionPlane> SetSubscribet;

    public SubscriptionData Data { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsArchived { get; private set; }

    private void OnEnable()
    {
        if (IsArchived)
        {
            _archiveButton.onClick.AddListener(OnSubscribtionClicked);
        }
        else
        {
            _archiveButton.onClick.AddListener(OnArchived);
        }

        _openButton.onClick.AddListener(Open);
    }

    private void OnDisable()
    {
        _archiveButton.onClick.RemoveListener(OnArchived);
        _archiveButton.onClick.RemoveListener(OnSubscribtionClicked);
        _openButton.onClick.RemoveListener(Open);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        IsActive = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }

    public void ResetData()
    {
        Data = null;
        _titlText.text = string.Empty;
        _date.text = string.Empty;
        _paymentDateText.text = string.Empty;
        _priceText.text = string.Empty;
        _typeText.text = string.Empty;
        IsArchived = false;
    }

    public void Archive()
    {
        IsArchived = true;
        Data.IsArchived = IsArchived;
        _archiveButton.onClick.RemoveListener(OnArchived);
        _archiveButton.onClick.AddListener(OnSubscribtionClicked);
    }

    public void Subscribe()
    {
        IsArchived = false;
        Data.IsArchived = IsArchived;
        _archiveButton.onClick.RemoveListener(OnSubscribtionClicked);
        _archiveButton.onClick.AddListener(OnArchived);
    }
    
    public void SetData(SubscriptionData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        Data = data;

        SetTariff();
        SetTitle();
        SetDate();
        SetPrice();
        SetNexPaymentDate();
    }

    private void SetTitle()
    {
        _titlText.text = Data.ServiceName;
    }

    private void SetPrice()
    {
        _priceText.text = PriceAddText + Data.Price;
    }

    private void SetDate()
    {
        _date.text = Data.Date;
    }

    private void SetNexPaymentDate()
    {
        _paymentDateText.text = Data.NextPaymentDate;
    }

    private void SetTariff()
    {
        _typeText.text = Data.Tariff;
    }

    private void Open() => OpenButtonClicked?.Invoke(this);

    private void OnArchived()
    {
        Data.IsArchived = IsArchived;
        IsArchived = true;
        SetArchived?.Invoke(this);
    }

    private void OnSubscribtionClicked()
    {
        Data.IsArchived = IsArchived;
        IsArchived = false; ;
        SetSubscribet?.Invoke(this);
    }
}