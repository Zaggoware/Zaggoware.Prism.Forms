namespace Zaggoware.Prism.Forms.ViewModels
{
    using System.Threading.Tasks;

    using global::Prism.Navigation;
    using global::Prism.Services;

    using Xamarin.Essentials;
    using Xamarin.Forms;
    
    public abstract class PageViewModelBase<TData> : PageViewModelBase
        where TData : class
    {
        public PageViewModelBase(
            INavigationService navigationService,
            IPageDialogService pageDialogService) 
            : base(navigationService, pageDialogService)
        {
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();
            await OnLoadAsync();
        }

        protected async Task<TData?> LoadApiDataAsync()
        {
            var data = await Task.Run(async () => await OnLoadApiDataAsync()).ConfigureAwait(false);
            Device.BeginInvokeOnMainThread(() => UpdateContent(data));
            return data;
        }

        protected TData? LoadCachedData()
        {
            var data = OnLoadCachedData();
            UpdateContent(data);
            return data;
        }

        protected override async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs args)
        {
            if (args.NetworkAccess == NetworkAccess.Internet)
            {
                await LoadApiDataAsync();
            }
            else
            {
                LoadCachedData();
            }
        }

        protected abstract Task<TData?> OnLoadApiDataAsync();

        protected virtual async Task OnLoadAsync()
        {
            IsLoading = true;
            LoadCachedData();
            
            if (!HasInternetConnection)
            {
                IsLoading = false;
                return;
            }

            await LoadApiDataAsync();
            IsLoading = false;
        }

        protected abstract TData? OnLoadCachedData();

        protected abstract void OnUpdateContent(TData data);
        
        private void UpdateContent(TData? data)
        {
            HasContent = data != null;
            
            if (data != null)
            {
                OnUpdateContent(data);
            }
        }
    }
}