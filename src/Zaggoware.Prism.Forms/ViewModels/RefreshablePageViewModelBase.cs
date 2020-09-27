namespace Zaggoware.Prism.Forms.ViewModels
{
    using System.Windows.Input;
    
    using global::Prism.Commands;
    using global::Prism.Navigation;
    using global::Prism.Services;

    public abstract class RefreshablePageViewModelBase : PageViewModelBase
    {
        protected RefreshablePageViewModelBase(
            INavigationService navigationService,
            IPageDialogService pageDialogService)
            : base(navigationService, pageDialogService)
        {
            RefreshCommand = new DelegateCommand(Refresh).ObservesCanExecute(() => CanRefresh);
        }

        public ICommand RefreshCommand { get; }

        public virtual bool IsRefreshing { get; set; }

        public virtual bool CanRefresh => !IsRefreshing;

        public override void OnAppearing()
        {
            IsRefreshing = true;
            base.OnAppearing();
            IsRefreshing = false;
        }

        protected abstract void OnRefresh();

        private void Refresh()
        {
            IsRefreshing = true;
            OnRefresh();
            IsRefreshing = false;
        }
    }
}
