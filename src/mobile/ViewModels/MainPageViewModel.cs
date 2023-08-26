using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiChat.Helpers;
using MauiChat.Models;
using MauiChat.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace MauiChat.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        [ObservableProperty]
        string query;

        private Color _fromStatus = Color.FromHex("#C50A07");
        public Color fromStatus
        {
            get
            {
                return _fromStatus;
            }
            set
            {
                SetProperty(ref _fromStatus, value);
            }
        }

        [ObservableProperty]
        bool isAnimationVisible = true;

        [ObservableProperty]
        ObservableCollection<Message> messages = new();

        [ObservableProperty]
        CollectionView collectionView;

        [ObservableProperty]
        ContentPage conversationView;

        [ObservableProperty]
        double opacityModeMessage = 1;

        [ObservableProperty]
        double opacityModeImage = 0.5;

        [ObservableProperty]
        private Message theMessage = new();

        SignalRService signalR = new SignalRService();
        IDispatcher _dispatcher;

        private AsyncRelayCommand _currentCommand;

        public AsyncRelayCommand CurrentCommand
        {
            get
            {
                return _currentCommand ??= new AsyncRelayCommand(AddMessage);
            }
            set
            {
                SetProperty(ref _currentCommand, value);
            }
        }



        public MainPageViewModel(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;

            signalR.ConnectAsync();

            signalR.hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var messageObj = JsonSerializer.Deserialize<Message>(message);

                if (string.IsNullOrWhiteSpace(messageObj.Text)) return;

                bool IsUserMessage = user == CurrentUser.user.FirstName ? true : false;

                Messages.Add(new Message
                {
                    Text = messageObj.Text,
                    IsUserMessage = IsUserMessage,
                    IsTextActive = messageObj.IsTextActive,
                    IsImageActive = messageObj.IsImageActive,
                    MessageTime = messageObj.MessageTime
                });

                Task.Delay(150).ContinueWith(t =>
                {
                    _dispatcher.Dispatch(() =>
                    {
                        CollectionView.ScrollTo
                        (
                            item: Messages.Last(),
                            position: ScrollToPosition.End,
                            animate: true
                        );
                    });
                });
            });
        }

        private async Task AddMessage()
        {
            TheMessage.IsImageActive = true;
            IsBusy = true;

            if (Messages.Count <= 0) IsAnimationVisible = false;

            await signalR.SendMessageAsync(CurrentUser.user.FirstName, Query);

            Query = string.Empty;

            IsBusy = false;
            TheMessage.IsImageActive = false;
        }
    }
}
