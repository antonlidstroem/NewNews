using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.Input;

namespace NewNews.MAUI.ViewModels
{
    public partial class MainViewModel
    {
        //[RelayCommand]
        //private void PreviousSavedSearch()
        //{
        //    if (SavedKeywords.Count == 0) return;

        //    _currentSavedIndex--;
        //    if (_currentSavedIndex < 0)
        //        _currentSavedIndex = SavedKeywords.Count - 1;

        //    SelectSavedSearchByIndex(_currentSavedIndex);
        //}

        //private void SelectSavedSearchByIndex(int index)
        //{
        //    if (index < 0 || index >= SavedKeywords.Count) return;

        //    var saved = SavedKeywords[index];
        //    SelectedSavedSearch = saved;
        //}
        //private int _currentSavedIndex = 0;

        //[RelayCommand]
        //private void NextSavedSearch()
        //{
        //    if (SavedKeywords.Count == 0) return;

        //    _currentSavedIndex++;
        //    if (_currentSavedIndex >= SavedKeywords.Count)
        //        _currentSavedIndex = 0;

        //    SelectSavedSearchByIndex(_currentSavedIndex);
        //}
    }
}
