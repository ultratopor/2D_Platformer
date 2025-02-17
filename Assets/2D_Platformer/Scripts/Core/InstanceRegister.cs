namespace Core
{
    /// <summary>
    /// Этот класс предоставляет контейнер для создания одноэлементных объектов для любого другого класса,
    /// входящего в состав Simulation. Обычно он используется для хранения моделей симуляции
    /// и классов конфигурации.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class InstanceRegister<T> where T : class, new()
    {
        public static T instance = new T();
    }
}