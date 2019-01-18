﻿using System.Threading.Tasks;

namespace Volo.Abp.Http.DynamicProxying
{
    public interface IRegularTestController
    {
        int IncrementValue(int value);

        Task<int> IncrementValueAsync(int value);

        Task GetException1Async();

        Task<string> PostValueWithHeaderAndQueryStringAsync(string headerValue, string qsValue);

        Task<string> PostValueWithBodyAsync(string bodyValue);

        Task<Car> PostObjectWithBodyAsync(Car bodyValue);

        Task<Car> PostObjectWithQueryAsync(Car bodyValue);

        Task<Car> GetObjectWithUrlAsync(Car bodyValue);

        Task<Car> GetObjectandIdAsync(int id, Car bodyValue);

        Task<Car> GetObjectAndIdWithQueryAsync(int id, Car bodyValue);

        Task<string> PutValueWithBodyAsync(string bodyValue);

        Task<string> PatchValueWithBodyAsync(string bodyValue);

        Task<string> PutValueWithHeaderAndQueryStringAsync(string headerValue, string qsValue);

        Task<string> PatchValueWithHeaderAndQueryStringAsync(string headerValue, string qsValue);

        Task<int> DeleteByIdAsync(int id);
    }
}
