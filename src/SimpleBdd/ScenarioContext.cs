using System.Collections.Concurrent;
using JetBrains.Annotations;

namespace SimpleBdd
{
    /// <remarks>Thread safe</remarks>
    public class ScenarioContext
    {
        public static ScenarioContext Current { get; } = new ScenarioContext();

        private readonly ConcurrentDictionary<string, object> _contextStorage
            = new ConcurrentDictionary<string, object>();


        /// <summary>
        /// Puts instance to scenario context to get it later
        /// </summary>
        /// <typeparam name="T">Any type you want to save</typeparam>
        /// <param name="instance">Instance of a type</param>
        /// <param name="key">Instance key. Default value is typeof(T).FullName. Provide if you want to use some other key.</param>
        /// <param name="instanceIndex">Instance index. Default value is 1</param>
        public void Put<T>(T instance, string key = null, int? instanceIndex = null)
        {
            SetValue(GetStorageKey(GetKeyBase<T>(key), instanceIndex), instance);
        }

        /// <summary>
        /// Puts result instance to scenario context to get it later
        /// </summary>
        /// <typeparam name="T">Any type you want to save</typeparam>
        /// <param name="instance">Instance of a type</param>
        /// <param name="key">Instance key. Default value is typeof(T).FullName. Provide if you want to use some other key.</param>
        /// <param name="instanceIndex">Instance index. Default value is 1</param>
        public void PutResult<T>(T instance, string key = null, int? instanceIndex = null)
        {
            SetValue(GetStorageKey(GetResultKeyBase<T>(key), instanceIndex), instance);
        }

        /// <summary>
        /// Gets saved with <see cref="Put{T}"/> instance
        /// </summary>
        /// <typeparam name="T">Type of saved with <see cref="Put{T}"/> instance</typeparam>
        /// <param name="key">Custom key of saved with <see cref="Put{T}"/> instance if it was provider</param>
        /// <param name="instanceIndex">Index of saved with <see cref="Put{T}"/> instance. If it is not set 1 is taken.</param>
        /// <returns></returns>
        public T Get<T>(string key = null, int? instanceIndex = null)
        {
            return GetValue<T>(GetStorageKey(GetKeyBase<T>(key), instanceIndex));
        }

        /// <summary>
        /// Gets saved with <see cref="PutResult{T}"/> result instance
        /// </summary>
        /// <typeparam name="T">Type of saved with <see cref="PutResult{T}"/> result instance</typeparam>
        /// <param name="key">Custom key of saved with <see cref="PutResult{T}"/> result instance if it was provider</param>
        /// <param name="instanceIndex">Index of saved with <see cref="PutResult{T}"/> result instance. If it is not set 1 is taken.</param>
        /// <returns></returns>
        public T GetResult<T>(string key = null, int? instanceIndex = null)
        {
            return GetValue<T>(GetStorageKey(GetResultKeyBase<T>(key), instanceIndex));
        }

        /// <summary>
        /// Clears all values in current context
        /// </summary>
        public void Clear()
        {
            _contextStorage.Clear();
        }

        private T GetValue<T>([NotNull] string key)
        {
            return (T)_contextStorage[key];
        }

        private void SetValue<T>(string key, T value)
        {
            _contextStorage[key] = value;
        }

        private static string GetKeyBase<T>(string key = null)
        {
            return string.IsNullOrEmpty(key) ? typeof(T).FullName : key;
        }

        private static string GetResultKeyBase<T>(string key = null)
        {
            return "_result" + GetKeyBase<T>(key);
        }

        private static string GetStorageKey(string keyBase, int? instanceIndex = 1)
        {
            return $"{keyBase}{instanceIndex}";
        }
    }
}