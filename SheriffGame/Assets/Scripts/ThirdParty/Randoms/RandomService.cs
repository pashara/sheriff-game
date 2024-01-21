using System;

namespace ThirdParty.Randoms
{
    public interface IRandomService
    {
        int InitialSeed { get; } 
        float Value { get; }
        
        /// <returns>A random int within [minInclusive..maxExclusive)</returns>
        int Random(int minInclusive, int maxExclusive);

        /// <returns>A random float within [minInclusive..maxInclusive]</returns>
        float Random(float minInclusive, float maxInclusive);
        void UpdateSeed(int seed);

        IRandomService CreateSubService();
    }

    public class RandomService : IRandomService
    {
        private Random _random;

        public float Value
        {
            get
            {
                var value = (float) _random.NextDouble();
                return value;
            }
        }
        
        public int InitialSeed { get; private set; }
        
        
        public RandomService()
        {
            InitialSeed = new Random().Next(int.MinValue + 1, int.MaxValue - 1);
            _random = new Random(InitialSeed);
        }

        public RandomService(int seed)
        {
            InitialSeed = seed;
            _random = new Random(InitialSeed);
        }

        public int Random(int @from, int to)
        {
            var value = _random.Next(from, to);
            return value;
        }

        public float Random(float @from, float to)
        {
            var value = (float)_random.NextDouble() * (to - from) + from;
            return value;
        }

        public void UpdateSeed(int seed)
        {
            _random = new Random(seed);
        }

        public IRandomService CreateSubService()
        {
            return new RandomService(_random.Next(int.MinValue + 1, int.MaxValue - 1));
        }
    }
}