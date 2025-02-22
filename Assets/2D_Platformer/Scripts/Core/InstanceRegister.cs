namespace Core
{
    /// <summary>
    /// Статический контейнер для симуляции.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class InstanceRegister<T> where T : class, new()
    {
        public static T instance = new T();
    }
}