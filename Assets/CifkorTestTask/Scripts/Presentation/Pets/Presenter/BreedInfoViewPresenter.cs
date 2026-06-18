using System;
using System.Threading;
using CifkorTestTask.Domain.Pets.Data;
using CifkorTestTask.Infrastructure.Injection;
using CifkorTestTask.Infrastructure.Networking;
using CifkorTestTask.Infrastructure.Pooling;
using CifkorTestTask.Presentation.Pets.Networking;
using CifkorTestTask.Presentation.Pets.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CifkorTestTask.Presentation.Pets.Presenter
{
    public class BreedInfoViewPresenter : IInitializable, IDisposable
    {
        private readonly IRequestQueueService _queue;
        private readonly IPoolableFactory _poolableFactory;
        private readonly IInjectionFactory _injectionFactory;
        private readonly BreedInfoView _viewPrefab;
        private readonly Transform _viewParent;
        private readonly BreedData _breedData;

        private CancellationTokenSource _lifetimeCts;
        private CancellationTokenSource _breedDetailsCts;

        private BreedInfoView _view;

        public event Action<string> OnBreedDetailsRequestEnqueue;

        public BreedInfoViewPresenter(
            BreedData breedData,
            BreedInfoView viewPrefab,
            Transform viewParent,
            IRequestQueueService queue,
            IPoolableFactory poolableFactory,
            IInjectionFactory injectionFactory)
        {
            _breedData = breedData;
            _viewPrefab = viewPrefab;
            _viewParent = viewParent;
            _queue = queue;
            _poolableFactory = poolableFactory;
            _injectionFactory = injectionFactory;
        }

        public void Initialize()
        {
            _lifetimeCts = new CancellationTokenSource();

            _view = _poolableFactory.Spawn(_viewPrefab, _viewParent);
            _view.SetText(_breedData.Index, _breedData.Name);
            _view.SetLoading(false);

            _view.OnClick += HandleViewClicked;
        }

        public void Dispose()
        {
            _lifetimeCts?.Cancel();
            _lifetimeCts?.Dispose();
            CancelBreedDetailsFetch();

            _view.OnClick -= HandleViewClicked;
            _poolableFactory.Despawn(_view);
        }

        public void TryCancelBreedDetailsFetch(string breedId)
        {
            if (breedId == _breedData.Id)
                return;

            CancelBreedDetailsFetch();
        }

        private void CancelBreedDetailsFetch()
        {
            _queue.CancelWhere(r => r is BaseTaggedRequest<BreedData> { Tag: GetBreedDetailsRequest.RequestTag });

            _breedDetailsCts?.Cancel();
            _breedDetailsCts?.Dispose();
            _breedDetailsCts = null;
        }

        private void EnqueueBreedDetailsFetch()
        {
            CancelBreedDetailsFetch();
            _breedDetailsCts = CancellationTokenSource.CreateLinkedTokenSource(_lifetimeCts.Token);
            EnqueueBreedDetailsFetchAsync(_breedDetailsCts.Token).Forget();

            async UniTaskVoid EnqueueBreedDetailsFetchAsync(CancellationToken ct)
            {
                var request = _injectionFactory.Create<GetBreedDetailsRequest, string>(_breedData.Id);
                try
                {
                    OnBreedDetailsRequestEnqueue?.Invoke(_breedData.Id);
                    _view.SetLoading(true);
                    var breedDetailsData = await _queue.Enqueue(request);

                    if (ct.IsCancellationRequested)
                        return;

                    _view.SetLoading(false);

                    OpenBreedDetailsPopup(breedDetailsData);
                }
                catch (OperationCanceledException)
                {

                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    _view.SetLoading(false);
                }
            }
        }

        private void OpenBreedDetailsPopup(BreedData breedDetailsData)
        {
            _injectionFactory.Create<BreedDetailsPresenter, BreedData>(breedDetailsData);
        }

        private void HandleViewClicked()
        {
            EnqueueBreedDetailsFetch();
        }
    }
}
