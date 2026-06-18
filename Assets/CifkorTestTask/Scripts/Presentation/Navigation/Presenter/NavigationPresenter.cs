using System;
using System.Collections.Generic;
using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Presentation.Climate.Presenter;
using CifkorTestTask.Presentation.Cookie.Presenter;
using CifkorTestTask.Presentation.Navigation.View;
using CifkorTestTask.Presentation.Pets.Presenter;
using CifkorTestTask.Presentation.Screens;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Navigation.Presenter
{
    public class NavigationPresenter : IInitializable, IDisposable
    {
        private readonly List<IViewBase> _views;
        private readonly INavigationView _navigationView;
        private readonly IInjectionFactory _factory;

        private readonly Type[] _buttonRouteTypes = { typeof(ClickerPresenter), typeof(WeatherPresenter), typeof(BreedPresenter)};
        private KeyValuePair<Type, object>? _activeRoutePresenter;

        public NavigationPresenter(
            List<IViewBase> views,
            INavigationView navigationView,
            IInjectionFactory factory)
        {
            _views = views;
            _navigationView = navigationView;
            _factory = factory;
        }

        void IInitializable.Initialize()
        {
            foreach (var view in _views)
            {
                view.Hide(false);
            }

            _navigationView.OnButtonClicked += HandleButtonClicked;
            _navigationView.Show();

            HandleButtonClicked(0);
        }

        void IDisposable.Dispose()
        {
            _navigationView.OnButtonClicked -= HandleButtonClicked;
        }

        private void HandleButtonClicked(int buttonIndex)
        {
            if (buttonIndex < 0)
                throw new ArgumentOutOfRangeException("buttonIndex", "Cannot be negative");

            if (buttonIndex > _buttonRouteTypes.Length - 1)
                throw new ArgumentOutOfRangeException("buttonIndex", $"Cannot be greater than {_buttonRouteTypes.Length - 1}");

            var clickedRouteType = _buttonRouteTypes[buttonIndex];
            if (_activeRoutePresenter.HasValue)
            {
                if (_activeRoutePresenter.Value.Key == clickedRouteType)
                {
                    Debug.LogWarning($"{clickedRouteType.Name} is already active. Nothing to do.");
                    return;
                }

                if (_activeRoutePresenter.Value.Value is IDisposable disposablePresenter)
                    disposablePresenter.Dispose();
            }

            _activeRoutePresenter = new KeyValuePair<Type, object>(clickedRouteType, _factory.Create(clickedRouteType));
            _navigationView.SetButtonActive(buttonIndex);
        }
    }
}
