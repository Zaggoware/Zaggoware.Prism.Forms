namespace Zaggoware.Prism.Forms.ViewModels
{
    using System.Threading.Tasks;
    using System.Windows.Input;
    
    using global::Prism.Commands;
    using global::Prism.Navigation;
    using global::Prism.Services;
    
    using Xamarin.Forms;

    public abstract class RefreshableApiPageViewModelBase<TData> : ApiPageViewModelBase<TData>
        where TData : class
    {
        protected RefreshableApiPageViewModelBase(
            INavigationService navigationService,
            IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync()).ObservesCanExecute(() => CanRefresh);
        }

        public ICommand RefreshCommand { get; }

        public virtual bool IsRefreshing { get; set; }

        public virtual bool CanRefresh => !IsRefreshing;

        protected override async Task OnLoadAsync()
        {
            IsRefreshing = true;
            await base.OnLoadAsync();
            Device.BeginInvokeOnMainThread(() =>
            {
                IsRefreshing = false;
            });
        }

        protected virtual Task<TData?> OnRefreshAsync()
        {
            return LoadApiDataAsync();
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await OnRefreshAsync();
            Device.BeginInvokeOnMainThread(() =>
            {
                IsRefreshing = false;
            });
        }
    }
}
