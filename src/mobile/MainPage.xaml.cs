using MauiChat.ViewModels;

namespace MauiChat;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;

        vm.CollectionView = myCollectionView;
        vm.ConversationView = mainPage;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        EntryQuery.IsEnabled = false;
        EntryQuery.IsEnabled = true;
    }
}

