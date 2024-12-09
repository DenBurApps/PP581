using System;
using UnityEngine;

public class OpenSubscription : MonoBehaviour
{
    private const string ArchivedText = "Archive";
    private const string SubscriptionText = "Subscriptions";

    [SerializeField] private OpenSubscriptionView _view;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private EditSubscription _editSubscription;
    [SerializeField] private ArchiveScreen _archiveScreen;

    private FilledSubscriptionPlane _filledSubscriptionPlane;

    public event Action<FilledSubscriptionPlane> DeleteActiveSubscriptionButtonClicked;
    public event Action<FilledSubscriptionPlane> DeleteArchivedSubscriptionButtonClicked; 
    public event Action<FilledSubscriptionPlane> EditButtonClicked;
    public event Action BackButtonClicked;

    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _mainScreen.OpenFilledPlane += OpenScreen;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.DeleteButtonClicked += OnDeleteButtonClicked;
        _view.EditButtonClicked += OnEditButtonClicked;

        _archiveScreen.OpenedArchivedSubscription += OpenScreen;

        _editSubscription.BackButtonClicked += _view.Enable;
    }

    private void OnDisable()
    {
        _mainScreen.OpenFilledPlane -= OpenScreen;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.DeleteButtonClicked -= OnDeleteButtonClicked;
        _view.EditButtonClicked -= OnEditButtonClicked;
        
        _archiveScreen.OpenedArchivedSubscription -= OpenScreen;

        _editSubscription.BackButtonClicked -= _view.Enable;
    }

    private void OpenScreen(FilledSubscriptionPlane filledSubscriptionPlane)
    {
        if (filledSubscriptionPlane == null)
            throw new ArgumentNullException(nameof(filledSubscriptionPlane));

        _filledSubscriptionPlane = filledSubscriptionPlane;

        _view.SetName(_filledSubscriptionPlane.Data.ServiceName);
        _view.SetNextDate(_filledSubscriptionPlane.Data.NextPaymentDate);
        _view.SetStartDate(_filledSubscriptionPlane.Data.Date);
        _view.SetPriceText(_filledSubscriptionPlane.Data.Price);
        _view.SetTariffText(_filledSubscriptionPlane.Data.Tariff);

        if (_filledSubscriptionPlane.IsArchived)
        {
            _view.SetTypeText(ArchivedText);
            _view.ToggleArchivePlane(true);
        }
        else
        {
            _view.SetTypeText(SubscriptionText);
            _view.ToggleArchivePlane(false);
        }

        _view.Enable();
    }

    private void OnDeleteButtonClicked()
    {
        if (!_filledSubscriptionPlane.IsArchived)
        {
            DeleteActiveSubscriptionButtonClicked?.Invoke(_filledSubscriptionPlane);
        }
        else
        {
            DeleteArchivedSubscriptionButtonClicked?.Invoke(_filledSubscriptionPlane);
        }

        _view.Disable();
    }

    private void OnEditButtonClicked()
    {
        EditButtonClicked?.Invoke(_filledSubscriptionPlane);
        _view.Disable();
    }

    private void OnBackButtonClicked()
    {
        BackButtonClicked?.Invoke();
        _view.Disable();
    }
}