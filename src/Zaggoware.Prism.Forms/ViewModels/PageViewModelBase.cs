namespace Zaggoware.Prism.Forms.ViewModels
{
    using global::Prism.AppModel;
    using global::Prism.Navigation;
    using global::Prism.Services;
    
    using PropertyChanged;

    [AddINotifyPropertyChangedInterface]
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
        
        protected INavigationService NavigationService { get; }
        
        protected IPageDialogService PageDialogService { get; }
        
        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}