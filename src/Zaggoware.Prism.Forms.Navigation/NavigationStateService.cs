namespace Zaggoware.Prism.Forms.Navigation
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    
    using global::Prism.Navigation;

    public class NavigationStateService : INavigationStateService
    {
        private static NavigationStateService? _instance;

        public static NavigationStateService Instance
        {
            get { return _instance ??= new NavigationStateService(); }
            set => _instance = value;
        }

        public static string Delimiter { get; set; } = "/";
        
        public static string ParamLastNavigationParams { get; set; } = "LastNavigationParams";
        
        public static string ParamLastNavigationRoute { get; set; } = "LastNavigationRoute";
        
        public static string ParamLastNavigationUri { get; set; } = "LastNavigationUri";

        public INavigationParameters? LastNavigationParams { get; protected set; }

        public string? LastNavigationRoute { get; protected set; }

        public Uri? LastNavigationUri { get; protected set; }
        
        public INavigationService? NavigationService { get; protected set; }

        public virtual async Task<INavigationResult> NavigateAsync(
            string route,
            INavigationParameters? navigationParameters,
            bool? asModal,
            bool animated)
        {
            if (NavigationService == null)
            {
                throw new InvalidOperationException(Exceptions.NavigationServiceMissing);
            }
            
            var stateParams = GetStateParameters();
            if (stateParams != null && stateParams.Any())
            {
                navigationParameters.AddRange(stateParams);
            }

            var result = await NavigationService.NavigateAsync(route, navigationParameters, asModal, animated);
            if (result.Success)
            {
                LastNavigationRoute = route.StartsWith(Delimiter)
                    ? route
                    : LastNavigationRoute + Delimiter + route;
                LastNavigationUri = null;
                LastNavigationParams = navigationParameters;
            }

            return result;
        }

        public virtual async Task<INavigationResult> NavigateAsync(
            Uri uri,
            INavigationParameters? navigationParameters,
            bool? asModal,
            bool animated)
        {
            if (NavigationService == null)
            {
                throw new InvalidOperationException(Exceptions.NavigationServiceMissing);
            }
            
            var stateParams = GetStateParameters();
            if (stateParams != null && stateParams.Any())
            {
                navigationParameters.AddRange(stateParams);
            }

            var result = await NavigationService.NavigateAsync(uri, navigationParameters, asModal, animated);
            if (result.Success)
            {
                LastNavigationRoute = null;
                LastNavigationUri = uri;
                LastNavigationParams = navigationParameters;
            }

            return result;
        }

        protected virtual INavigationParameters? GetStateParameters()
        {
            return new NavigationParameters
            {
                { ParamLastNavigationRoute, Instance.LastNavigationRoute },
                { ParamLastNavigationUri, Instance.LastNavigationUri },
                { ParamLastNavigationParams, Instance.LastNavigationParams }
            };
        }
        
        protected internal virtual NavigationStateService WithNavigationService(INavigationService navigationService)
        {
            NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            return this;
        }
    }
}
