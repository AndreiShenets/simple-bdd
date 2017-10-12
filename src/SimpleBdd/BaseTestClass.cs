namespace SimpleBdd
{
    public class BaseTestClass
    {
        /// <summary>
        /// Puts instance to scenario context to get it later
        /// </summary>
        /// <typeparam name="T">Any type you want to save</typeparam>
        /// <param name="instance">Instance of a type</param>
        /// <param name="key">Instance key. Default value is typeof(T).FullName. Provide if you want to use some other key.</param>
        /// <param name="instanceIndex">Instance index. Default value is 1</param>
        public static void Put<T>(T instance, string key = null, int? instanceIndex = null)
        {
            ScenarioContext.Current.Put(instance, key, instanceIndex);
        }

        /// <summary>
        /// Puts result instance to scenario context to get it later
        /// </summary>
        /// <typeparam name="T">Any type you want to save</typeparam>
        /// <param name="instance">Instance of a type</param>
        /// <param name="key">Instance key. Default value is typeof(T).FullName. Provide if you want to use some other key.</param>
        /// <param name="instanceIndex">Instance index. Default value is 1</param>
        public static void PutResult<T>(T instance, string key = null, int? instanceIndex = null)
        {
            ScenarioContext.Current.PutResult(instance, key, instanceIndex);
        }

        /// <summary>
        /// Gets saved with <see cref="Put{T}"/> instance
        /// </summary>
        /// <typeparam name="T">Type of saved with <see cref="Put{T}"/> instance</typeparam>
        /// <param name="key">Custom key of saved with <see cref="Put{T}"/> instance if it was provider</param>
        /// <param name="instanceIndex">Index of saved with <see cref="Put{T}"/> instance. If it is not set 1 is taken.</param>
        /// <returns></returns>
        public static T Get<T>(string key = null, int? instanceIndex = null)
        {
            return ScenarioContext.Current.Get<T>(key, instanceIndex);
        }

        /// <summary>
        /// Gets saved with <see cref="PutResult{T}"/> result instance
        /// </summary>
        /// <typeparam name="T">Type of saved with <see cref="PutResult{T}"/> result instance</typeparam>
        /// <param name="key">Custom key of saved with <see cref="PutResult{T}"/> result instance if it was provider</param>
        /// <param name="instanceIndex">Index of saved with <see cref="PutResult{T}"/> result instance. If it is not set 1 is taken.</param>
        /// <returns></returns>
        public static T GetResult<T>(string key = null, int? instanceIndex = null)
        {
            return ScenarioContext.Current.GetResult<T>(key, instanceIndex);
        }
    }
}