using System;
using System.Collections.Generic;
using System.Threading;
using CifkorTestTask.Application.Pets;
using CifkorTestTask.Domain.Pets.Data;
using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Networking;
using CifkorTestTask.Presentation.Pets.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Pets.Presenter
{
    public class BreedPresenter : IInitializable, IDisposable
    {
        private readonly IBreedController _breedController;
        private readonly IBreedView _view;
        private readonly IInjectionFactory _factory;
        private readonly List<BreedInfoViewPresenter> _infoPresenters = new();

        public BreedPresenter(
            IBreedController breedController,
            IBreedView view,
            IInjectionFactory factory)
        {
            _breedController = breedController;
            _view = view;
            _factory = factory;
        }

        void IInitializable.Initialize()
        {
            _breedController.OnBreedUpdateEnqueue += HandleBreedUpdateEnqueue;
            _breedController.OnBreedUpdate += HandleBreedUpdate;
            _breedController.OnBreedError += HandleBreedError;

            _breedController.StartRequest();

            _view.Show();
            _view.ShowError(string.Empty);
            _view.BreedInfoPrefab.gameObject.SetActive(false);
        }

        void IDisposable.Dispose()
        {
            _breedController.OnBreedUpdateEnqueue -= HandleBreedUpdateEnqueue;
            _breedController.OnBreedUpdate -= HandleBreedUpdate;
            _breedController.OnBreedError -= HandleBreedError;

            _breedController.StopRequest();

            CleanInfoPresenters();

            _view.Hide();
            _view.Dispose();
        }

        private void CleanInfoPresenters()
        {
            foreach (var presenter in _infoPresenters)
            {
                presenter.OnBreedDetailsRequestEnqueue -= HandleBreedDetailsRequestEnqueue;
                presenter.Dispose();
            }

            _infoPresenters.Clear();
        }

        private void HandleBreedUpdateEnqueue()
        {
            _view.SetLoading(true);
        }

        private void HandleBreedUpdate(BreedData[] datas)
        {
            _view.SetLoading(false);
            CleanInfoPresenters();

            foreach (var data in datas)
            {
                var infoPresenter = _factory.Create<BreedInfoViewPresenter, BreedData, BreedInfoView, Transform>
                    (data, _view.BreedInfoPrefab, _view.ScrollPanelContentParent);

                infoPresenter.OnBreedDetailsRequestEnqueue += HandleBreedDetailsRequestEnqueue;
                _infoPresenters.Add(infoPresenter);
            }
        }

        private void HandleBreedError(string error)
        {
            _view.SetLoading(false);
            _view.ShowError("Не удалось загрузить породы");
        }

        private void HandleBreedDetailsRequestEnqueue(string breedId)
        {
            foreach (var presenter in _infoPresenters)
            {
                presenter.TryCancelBreedDetailsFetch(breedId);
            }
        }
    }
}
