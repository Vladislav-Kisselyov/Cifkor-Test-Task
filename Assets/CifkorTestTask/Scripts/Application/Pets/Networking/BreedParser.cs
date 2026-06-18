using System;
using System.Linq;
using CifkorTestTask.Domain.Pets.Data;
using UnityEngine;

namespace CifkorTestTask.Application.Pets.Networking
{
    public static class BreedsParser
    {
        public static BreedData[] ParseList(string json)
        {
            try
            {
                var wrapper = JsonUtility.FromJson<BreedListWrapper>(json);
                var index = 1;
                return wrapper?.data?
                    .Take(10)
                    .Select(d => new BreedData
                    {
                        Id = d.id,
                        Index = (index++).ToString(),
                        Name = d.attributes?.name ?? d.id,
                        Description = d.attributes?.description ?? string.Empty
                    })
                    .ToArray() ?? Array.Empty<BreedData>();
            }
            catch (Exception ex)
            {
                throw new Exception($"BreedsParser.ParseList failed: {ex.Message}", ex);
            }
        }

        public static BreedData ParseSingle(string json)
        {
            try
            {
                var wrapper = JsonUtility.FromJson<BreedSingleWrapper>(json);
                var d = wrapper?.data;
                if (d == null) throw new Exception("No breed data");
                return new BreedData
                {
                    Id = d.id,
                    Name = d.attributes?.name ?? d.id,
                    Description = d.attributes?.description ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"BreedsParser.ParseSingle failed: {ex.Message}", ex);
            }
        }

        [Serializable]
        private class BreedListWrapper
        {
            public BreedDto[] data;
        }

        [Serializable]
        private class BreedSingleWrapper
        {
            public BreedDto data;
        }

        [Serializable]
        private class BreedDto
        {
            public string id;
            public BreedAttributes attributes;
        }

        [Serializable]
        private class BreedAttributes
        {
            public string name;
            public string description;
        }
    }
}
