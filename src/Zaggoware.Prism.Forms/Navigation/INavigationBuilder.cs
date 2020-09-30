namespace Zaggoware.Prism.Forms.Navigation
{
    using System;
    using System.Threading.Tasks;

    using global::Prism.Navigation;

    using Xamarin.Forms;
    
    public interface INavigationBuilder
    {
        bool? NavigateAsModal { get; }
        
        bool NavigateFromRoot { get; }
        
        NavigationParameters? NavigationParameters { get; }
        
        INavigationService NavigationService { get; }

        string[] Pages { get; }

        bool ShouldAnimate { get; }
        
        Uri? Uri { get; }
        
        INavigationBuilder Animate(bool animate = true);

        INavigationBuilder AsModal(bool asModal = true);

        INavigationBuilder EnsureSuccess();
        
        INavigationBuilder FromRoot();

        Task<INavigationResult> NavigateAsync();
        
        INavigationBuilder ToPage<TPage>()
            where TPage : Page;

        INavigationBuilder ToUri(Uri uri);
        
        INavigationBuilder WithId<T>(T value);

        INavigationBuilder WithParam<T>(string name, T value);
    }
}