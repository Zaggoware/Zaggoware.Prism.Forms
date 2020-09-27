namespace Zaggoware.Prism.Forms.Navigation
{
    using System;
    using System.Threading.Tasks;

    using global::Prism.Navigation;
    
    public interface INavigationStateService
    {
        INavigationParameters? LastNavigationParams { get; }

        string? LastNavigationRoute { get; }

        Uri? LastNavigationUri { get; }
        
        INavigationService? NavigationService { get; }
        
        Task<INavigationResult> NavigateAsync(
            string route,
            INavigationParameters? navigationParameters,
            bool? asModal,
            bool animated);

        Task<INavigationResult> NavigateAsync(
            Uri uri,
            INavigationParameters? navigationParameters,
            bool? asModal,
            bool animated);
    }
}