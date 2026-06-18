using System;
using CifkorTestTask.Domain.Pets.Data;

namespace CifkorTestTask.Domain.Pets
{
    public class Breed
    {
        public event Action<BreedData[]> OnBreedUpdate;
        public event Action<string> OnBreedError;

        public BreedData[] RuntimeData { get; private set; }

        public void SetBreedData(BreedData[] data)
        {
            RuntimeData = data;
            OnBreedUpdate?.Invoke(data);
        }

        public void SetError(string error) => OnBreedError?.Invoke(error);
    }
}
