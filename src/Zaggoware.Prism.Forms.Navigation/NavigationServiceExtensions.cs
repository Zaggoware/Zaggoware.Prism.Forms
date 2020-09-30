namespace Zaggoware.Prism.Forms.Navigation
{
    using System;
    using System.Collections.Generic;
    
    using global::Prism.Navigation;

    public static class NavigationServiceExtensions
    {
        public static INavigationBuilder Builder(this INavigationService navigationService)
        {
            return new NavigationBuilder(navigationService);
        }
        
        public static INavigationBuilder Builder<TBuilder>(this INavigationService navigationService, params object[] args)
            where TBuilder : INavigationBuilder
        {
            var instanceArgs = new List<object>(args.Length + 1) { navigationService };
            instanceArgs.AddRange(args);

            var builder = (INavigationBuilder)Activator.CreateInstance(typeof(TBuilder), instanceArgs.ToArray());
            return builder;
        }

        public static INavigationStateService WithState(this INavigationService navigationService)
        {
            return NavigationStateService.Instance.WithNavigationService(navigationService);
        }
    }
}