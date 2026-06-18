using System;
using CifkorTestTask.Domain.Pets.Data;
using CifkorTestTask.Presentation.Pets.View;
using Zenject;

namespace CifkorTestTask.Presentation.Pets.Presenter
{
    public class BreedDetailsPresenter : IInitializable, IDisposable
    {
        private readonly BreedData _breedDetailsData;
        private readonly IBreedDetailsView _detailsView;

        public BreedDetailsPresenter(
            BreedData breedDetailsData,
            IBreedDetailsView detailsView)
        {
            _breedDetailsData = breedDetailsData;
            _detailsView = detailsView;
        }

        public void Initialize()
        {
            _detailsView.SetText(_breedDetailsData.Name, _breedDetailsData.Description);
            _detailsView.OnExitButtonClick += HandleExitButtonClick;

            _detailsView.Show();
        }

        public void Dispose()
        {
            _detailsView.OnExitButtonClick -= HandleExitButtonClick;
            _detailsView.Hide();
        }

        private void HandleExitButtonClick()
        {
            Dispose();
        }
    }
}
