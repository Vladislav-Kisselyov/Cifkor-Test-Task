using System;
using CifkorTestTask.Domain.Pets.Data;

namespace CifkorTestTask.Application.Pets
{
    public interface IBreedController
    {
        public event Action OnBreedUpdateEnqueue;
        public event Action<BreedData[]> OnBreedUpdate;
        public event Action<string> OnBreedError;

        public void StartRequest();
        public void StopRequest();
    }
}
