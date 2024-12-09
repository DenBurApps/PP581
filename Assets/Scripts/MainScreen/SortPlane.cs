using System;
using UnityEngine;
using UnityEngine.UI;

public class SortPlane : MonoBehaviour
{
    [SerializeField] private Button _sortByDescending;
    [SerializeField] private Button _sortByAscending;
    [SerializeField] private Button _sortByNameAzButton;
    [SerializeField] private Button _sortByNameZaButton;

    public event Action SortByDesceningClicked;
    public event Action SortByAscendingClicked;
    public event Action SortByNameAzClicked;
    public event Action SortByNameZaClicked;
    
    private void OnEnable()
    {
        _sortByDescending.onClick.AddListener(OnSortByDescending);
        _sortByAscending.onClick.AddListener(OnSortByAscending);
        _sortByNameAzButton.onClick.AddListener(OnSortByNameAZ);
        _sortByNameZaButton.onClick.AddListener(OnSortByNameZA);
    }

    private void OnDisable()
    {
        _sortByDescending.onClick.RemoveListener(OnSortByDescending);
        _sortByAscending.onClick.RemoveListener(OnSortByAscending);
        _sortByNameAzButton.onClick.RemoveListener(OnSortByNameAZ);
        _sortByNameZaButton.onClick.RemoveListener(OnSortByNameZA);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    private void OnSortByDescending() => SortByDesceningClicked?.Invoke();
    private void OnSortByAscending() => SortByAscendingClicked?.Invoke();
    private void OnSortByNameAZ() => SortByNameAzClicked?.Invoke();
    private void OnSortByNameZA() => SortByNameZaClicked?.Invoke();
}
