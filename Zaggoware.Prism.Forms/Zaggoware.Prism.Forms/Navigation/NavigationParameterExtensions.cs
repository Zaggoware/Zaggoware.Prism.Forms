namespace Zaggoware.Prism.Forms.Navigation
{
    using System.Collections.Generic;
    
    using global::Prism.Navigation;
    
    public static class NavigationParameterExtensions
    {
        public static void AddRange(
            this INavigationParameters? parameters,
            IEnumerable<KeyValuePair<string, object>> paramsToAdd)
        {
            parameters ??= new NavigationParameters();
            foreach (var (key, value) in paramsToAdd)
            {
                parameters.Add(key, value);
            }
        }
        
        public static T GetId<T>(this INavigationParameters parameters)
        {
            return parameters.GetValue<T>(NavigationBuilder.ParamId);
        }
    }
}