# Zaggoware.Prism.Forms
Repository with some additional classes and extensions for the Prism Library for Xamarin Forms.

[![Nuget version](https://img.shields.io/nuget/v/Zaggoware.Prism.Forms)](https://www.nuget.org/packages/Zaggoware.Prism.Forms/)

## Navigation

To make navigation easier without the use of hardcoded strings a navigation builder has been added.
Some extension methods have been added to Prism's `NavigationService` within the `Zaggoware.Prism.Forms.Navigation` namespace:
```csharp
INavigationBuilder FromRoot();
INavigationBuilder ToPage<TPage>() where TPage : Xamarin.Forms.Page
```
These methods will create a new `NavigationBuilder` for the current `NavigationService`.

### Basic usage:
```csharp
await NavigationService
    .FromRoot()
    .ToPage<MainPage>()
    .NavigateAsync()
// This results in: "/MainPage"
```
`.FromRoot()` can be useful when the user is not allowed to navigate back (e.g. when logging out).

Going to a (sub-)page with navigation history:
```csharp
await NavigationService
    .ToPage<NavigationPage>()
    .ToPage<DetailsPage>()
    .WithId(123)
    .NavigateAsync();
// This results in: "NavigationPage/DetailsPage?Id=123"
```

### Navigation parameters
To add navigation parameters:
```csharp
await NavigationService
    .ToPage<NavigationPage>()
    .ToPage<AnotherPage>()
    .WithParam("Foo", "Bar") // Tip: ParamNames.Foo (see tip below) or extension method: WithFoo(...)
    .WithParam("Bar", "Foo") // Tip: Paramnames.Bar (see tip below) or extension method: WithBar(...)
    .NavigateAsync();
// this results in: "NavigationPage/AnotherPage?Foo=Bar&Bar=Foo"
```
*Tip*: To prevent hardcoding the param names, create a class named `ParamNames` and add constant strings to it.
When retrieving the parameters, you can then simply call `parameters.GetValue<string>(ParamNames.Foo)`. Or even better, create an extensions class so you can call `parameters.GetFoo()`.

### Animation
By default, page navigation has an animation. To disable this, you can add `Animate(false)` to the builder:
```csharp
await NavigationService
    .ToPage<NavigationPage>()
    .ToPage<AnotherPage>()
    .Animate(false)
    .NavigateAsync();
```

### Modal windows
For modal windows, you can simply add `AsModal()` to the builder:
```csharp
await NavigationService
    .ToPage<MyModalPage>()
    .AsModal()
    .NavigateAsync();
```

### State service
For your convience, a state service has been built-in which will keep track of the last navigated page, including its navigation parameters.
Every time `NavigateAsync()` is called on the `NavigationBuilder`, this state will be updated. This can come in handy when other dependencies need to know the current page and/or its parameters. The state service is a singleton and can be accessed by its static instance property: `NavigationStateService.Instance`.

The state service contains three properties:
* `LastNavigationParams` containing the navigation parameters of the last navigation.
* `LastNavigationUri` containing the URI if available. This can be set with the builder by using `WithUri(Uri uri)`, replacing the `ToPage<TPage>()` routing.
* `LastNavigationRoute` containg the route of the last navigation (e.g. "/MainPage/NavigationPage/DetailsPage")

Known issue: When navigating back (`NavigationService.GoBackAsync()`) the state is not automatically updated.
