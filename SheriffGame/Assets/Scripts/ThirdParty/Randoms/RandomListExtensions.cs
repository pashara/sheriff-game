using System;
using System.Collections.Generic;
using System.Linq;

namespace ThirdParty.Randoms
{
    public interface IWeightable
    {
        float Weight { get; }
    }

    public static class RandomListExtensions
    {
        public static T Random<T>(this List<T> list) => 
            list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];

        public static T Random<T>(this List<T> list, IRandomService randomService) =>
            list.Count == 0 ? default(T) : list[randomService.Random(0, list.Count)];
        
        public static T Random<T>(this IList<T> list) => 
            list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];

        public static T Random<T>(this IList<T> list, IRandomService randomService) =>
            list.Count == 0 ? default(T) : list[randomService.Random(0, list.Count)];
        
        public static T Random<T>(this IReadOnlyList<T> list) => 
            list.Count == 0 ? default(T) : list[UnityEngine.Random.Range(0, list.Count)];

        public static T Random<T>(this IReadOnlyList<T> list, IRandomService randomService) =>
            list.Count == 0 ? default(T) : list[randomService.Random(0, list.Count)];

        public static T RandomByWeight<T>(this IEnumerable<T> elements, IRandomService randomService) where T : IWeightable
        {
            var weightables = elements as T[] ?? elements.ToArray();
            var totalWeight = weightables.Sum(element => element.Weight);

            var currentWeight = 0f;
            var neededWeight = randomService.Value * totalWeight;

            foreach (var element in weightables)
            {
                currentWeight += element.Weight;
                if (currentWeight >= neededWeight)
                {
                    return element;
                }
            }

            return weightables.LastOrDefault();
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static void Shuffle<T>(this IList<T> list, IRandomService randomService)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = randomService.Random(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}