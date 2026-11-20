using System.Collections.Generic;
using Xunit;
using IndependentWork8;

namespace IndependentWork8.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_And_Count_ShouldWorkCorrectly()
        {
            var cache = new Cache<string, int>(3);
            cache.Add("A", 1);
            cache.Add("B", 2);

            Assert.Equal(2, cache.Count);
        }

        [Fact]
        public void Get_ExistingKey_ShouldReturnCorrectValue()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("Key1", "Value1");

            string result = cache.Get("Key1");
            Assert.Equal("Value1", result);
        }

        [Fact]
        public void Get_NonExistingKey_ShouldThrowKeyNotFoundException()
        {
            var cache = new Cache<int, string>(2);

            Assert.Throws<KeyNotFoundException>(() => cache.Get(99));
        }

        [Fact]
        public void Add_ExceedingCapacity_ShouldEvictOldestElementFIFO()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "First");
            cache.Add(2, "Second");
            cache.Add(3, "Third");

            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
            Assert.Equal(2, cache.Count);
        }

        [Fact]
        public void ContainsKey_ShouldReturnTrueForExistingAndFalseForMissing()
        {
            var cache = new Cache<string, int>(2);
            cache.Add("Test", 100);

            Assert.True(cache.ContainsKey("Test"));
            Assert.False(cache.ContainsKey("Missing"));
        }
    }
}
