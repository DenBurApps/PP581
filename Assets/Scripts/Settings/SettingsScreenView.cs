using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class SettingsScreenView : MonoBehaviour
{
    [SerializeField] private Button _feedbackButton;
    [SerializeField] private Button _privacyPolicyButton;
    [SerializeField] private Button _termsOfUseButton;
    [SerializeField] private Button _versionButton;
    [SerializeField] private Button _contactUsButton;
    [SerializeField] private Button _subscriptionsButton;
    [SerializeField] private Button _archiveButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action FeedbackButtonClicked;
    public event Action PrivacyPolicyButtonClicked;
    public event Action TermsOfUseButtonClicked;
    public event Action VersionButtonClicked;
    public event Action ContactUsButtonClicked;
    public event Action SubscriptionsClicked;
    public event Action ArchiveClicked;
    
    private void OnEnable()
    {
        _feedbackButton.onClick.AddListener(OnProcessFeedbackButtonClicked);
        _privacyPolicyButton.onClick.AddListener(OnProcessPolicyButtonClicked);
        _termsOfUseButton.onClick.AddListener(OnTermsOfUseButtonClicked);
        _versionButton.onClick.AddListener(OnVersionButtonClicked);
        _archiveButton.onClick.AddListener(OnArchiveClicked);
        _subscriptionsButton.onClick.AddListener(OnSubscriptionsClicked);
        _contactUsButton.onClick.AddListener(OnContactUsClicked);
    }

    private void OnDisable()
    {
        _feedbackButton.onClick.RemoveListener(OnProcessFeedbackButtonClicked);
        _privacyPolicyButton.onClick.RemoveListener(OnProcessPolicyButtonClicked);
        _termsOfUseButton.onClick.RemoveListener(OnTermsOfUseButtonClicked);
        _versionButton.onClick.RemoveListener(OnVersionButtonClicked);
        _archiveButton.onClick.RemoveListener(OnArchiveClicked);
        _subscriptionsButton.onClick.RemoveListener(OnSubscriptionsClicked);
       
    }

    private void OnSubscriptionsClicked()
    {
        SubscriptionsClicked?.Invoke();
    }

    private void OnArchiveClicked()
    {
        ArchiveClicked?.Invoke();
    }

    private void OnContactUsClicked()
    {
        ContactUsButtonClicked?.Invoke();
    }
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }
    
    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void OnVersionButtonClicked()
    {
        VersionButtonClicked?.Invoke();
    }

    private void OnTermsOfUseButtonClicked()
    {
        TermsOfUseButtonClicked?.Invoke();
    }

    private void OnProcessPolicyButtonClicked()
    {
        PrivacyPolicyButtonClicked?.Invoke();
    }

    private void OnProcessFeedbackButtonClicked()
    {
        FeedbackButtonClicked?.Invoke();
    }
}
