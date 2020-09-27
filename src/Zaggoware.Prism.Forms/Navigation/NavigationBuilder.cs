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

        public static Action<Exception>? NavigationExceptionHandler { get; set; } = HandleNavigationException;
        
        public static string ParamId { get; set; } = "Id";
        
        public bool? IsModal { get; protected set; }
        
        public bool NavigateFromRoot { get; protected set; }
        
        public NavigationParameters NavigationParameters { get; protected set; } = new NavigationParameters();
        
        public INavigationService NavigationService { get; protected set; }

        public string[] Pages => _pages.ToArray();
        
        public bool ShouldAnimate { get; protected set; } = true;
        
        public Uri? Uri { get; protected set; }

        public virtual INavigationBuilder Animate(bool animate)
        {
            ShouldAnimate = animate;
            return this;
        }

        public virtual INavigationBuilder AsModal()
        {
            IsModal = true;
            return this;
        }

        public virtual INavigationBuilder FromRoot()
        {
            NavigateFromRoot = true;
            return this;
        }
        
        public virtual async Task<INavigationResult> NavigateAsync()
        {
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
                result = await NavigationService.State().NavigateAsync(route, NavigationParameters, IsModal, ShouldAnimate);
            }
            else
            {
                result = await NavigationService.State().NavigateAsync(Uri, NavigationParameters, IsModal, ShouldAnimate);
            }

            if (!result.Success)
            {
                OnNavigationFailed(result);
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
            NavigationParameters.Add(ParamId, value);
            return this;
        }

        public virtual INavigationBuilder WithParam<T>(string name, T value)
        {
            NavigationParameters.Add(name, value);
            return this;
        }

        protected virtual void OnNavigationFailed(INavigationResult result)
        {
            NavigationExceptionHandler?.Invoke(result.Exception);
        }

        private static void HandleNavigationException(Exception exception)
        {
#if DEBUG
            throw exception;
#endif
        }
    }
}
