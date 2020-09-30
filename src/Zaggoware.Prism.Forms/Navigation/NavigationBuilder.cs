namespace Zaggoware.Prism.Forms.Navigation
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    
    using global::Prism.Navigation;
    
    using Xamarin.Forms;

    public class NavigationBuilder : INavigationBuilder
    {
        private readonly List<string> _pages = new List<string>();

        public NavigationBuilder(INavigationService navigationService)
        {
            NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public static string Delimiter { get; set; } = "/";

        public static Action<INavigationResult>? NavigationFailedHandler { get; set; }
        
        public static string ParamId { get; set; } = "Id";

        public static bool ShouldAnimateByDefault { get; set; } = true;
        
        public bool? NavigateAsModal { get; protected set; }
        
        public bool NavigateFromRoot { get; protected set; }
        
        public NavigationParameters? NavigationParameters { get; protected set; }
        
        public INavigationService NavigationService { get; protected set; }

        public string[] Pages => _pages.ToArray();
        
        public bool ShouldAnimate { get; protected set; } = ShouldAnimateByDefault;

        public bool ShouldEnsureSuccess { get; protected set; }
        
        public Uri? Uri { get; protected set; }

        public virtual INavigationBuilder Animate(bool animate = true)
        {
            ShouldAnimate = animate;
            return this;
        }

        public virtual INavigationBuilder AsModal(bool asModal = true)
        {
            NavigateAsModal = asModal;
            return this;
        }

        public virtual INavigationBuilder EnsureSuccess()
        {
            ShouldEnsureSuccess = true;
            return this;
        }

        public virtual INavigationBuilder FromRoot()
        {
            NavigateFromRoot = true;
            return this;
        }
        
        public virtual async Task<INavigationResult> NavigateAsync()
        {
            var stateService = NavigationService.WithState();
            INavigationResult result;
            if (Uri == null)
            {
                var stringBuilder = new StringBuilder();
                for (var i = 0; i < _pages.Count; i++)
                {
                    if (NavigateFromRoot || i > 0)
                    {
                        stringBuilder.Append(Delimiter);
                    }

                    stringBuilder.Append(Pages[i]);
                }

                var route = stringBuilder.ToString();
                result = await stateService.NavigateAsync(route, NavigationParameters, NavigateAsModal, ShouldAnimate);
            }
            else
            {
                result = await stateService.NavigateAsync(Uri, NavigationParameters, NavigateAsModal, ShouldAnimate);
            }

            if (result.Success)
            {
                return result;
            }
            
            OnNavigationFailed(result);
                
            if (ShouldEnsureSuccess)
            {
                throw result.Exception;
            }

            return result;
        }
        
        public virtual INavigationBuilder ToPage<TPage>()
            where TPage : Page
        {
            var pageName = typeof(TPage).Name;
            _pages.Add(pageName);
            return this;
        }

        public virtual INavigationBuilder ToUri(Uri uri)
        {
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            return this;
        }

        public virtual INavigationBuilder WithId<T>(T value)
        {
            return WithParam(ParamId, value);
        }

        public virtual INavigationBuilder WithParam<T>(string name, T value)
        {
            NavigationParameters ??= new NavigationParameters();
            NavigationParameters.Add(name, value);
            return this;
        }

        protected virtual void OnNavigationFailed(INavigationResult result)
        {
            NavigationFailedHandler?.Invoke(result);
        }
    }
}
