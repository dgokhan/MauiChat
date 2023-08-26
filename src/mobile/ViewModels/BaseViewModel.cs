using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiChat.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        bool isBusy;
    }
}
