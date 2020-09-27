namespace Zaggoware.Prism.Forms.ViewModels
{
    using global::Prism.AppModel;
    using global::Prism.Navigation;
    using global::Prism.Services;
    
    using Xamarin.Essentials;

    public abstract class PageViewModelBase : IInitialize, INavigatedAware, IPageLifecycleAware
    {
        protected PageViewModelBase(
            INavigationService navigationService,
            IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
        }

        public bool CanNavigate { get; set; } = true;

        public bool HasContent { get; set; } = true;

        public bool HasInternetConnection => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public bool IsLoading { get; set; }
        
        protected INavigationService NavigationService { get; }
        
        protected IPageDialogService PageDialogService { get; }
        
        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnAppearing()
        {
            
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        public virtual void OnDisappearing()
        {
            
            Connectivity.ConnectivityChanged -= OnConnectivityChanged;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
        
        protected virtual void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
        }
    }
}