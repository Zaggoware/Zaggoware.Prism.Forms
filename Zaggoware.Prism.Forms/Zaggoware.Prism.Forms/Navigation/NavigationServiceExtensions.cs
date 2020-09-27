namespace Zaggoware.Prism.Forms.Navigation
{
    using global::Prism.Navigation;
    
    using Xamarin.Forms;

    public static class NavigationServiceExtensions
    {
        public static INavigationBuilder FromRoot(this INavigationService navigationService)
        {
            return new NavigationBuilder(navigationService).FromRoot();
        }

        public static INavigationBuilder ToPage<TPage>(this INavigationService navigationService)
            where TPage : Page
        {
            return new NavigationBuilder(navigationService).ToPage<TPage>();
        }

        public static INavigationStateService State(this INavigationService navigationService)
        {
            return NavigationStateService.Instance.WithNavigationService(navigationService);
        }
    }
}