using System;
using System.Collections.Generic;
using UnityEngine;


namespace Core
{
    /// <summary>
    /// Класс Simulation реализует шаблон проектирования «симулятор дискретных событий». 
    /// События объединяются в пул с вместимостью по умолчанию 4 экземпляра.
    /// </summary>
    public static class Simulation
    {

        public static HeapQueue<Event> EventQueue = new HeapQueue<Event>();
        public static Dictionary<System.Type, Stack<Event>> EventPools = new Dictionary<System.Type, Stack<Event>>();

        /// <summary>
        /// Создайте новое событие типа T и верните его, но не планируйте его.
        /// </summary>
        /// <typeparam name="T">Новое событие</typeparam>
        /// <returns>Новое незапланированное событие</returns>
        static public T New<T>() where T : Event, new()
        {
            // создаём новый стэк событий
            Stack<Event> pool;
            // если в словаре такого стека нет
            if (!EventPools.TryGetValue(typeof(T), out pool))
            {
                // то создаём новый стек из четырёх ячеек
                pool = new Stack<Event>(4);
                // всовываем новое событие туда
                pool.Push(new T());
                // всовываем в словарь стэк по ключу события
                EventPools[typeof(T)] = pool;
            }
            if (pool.Count > 0) // если в стэке есть что-нибудь, то выдёргиеваем первый
                return (T)pool.Pop();
            else// если ничего нет, то новое событие возвращаем
                return new T();
        }

        /// <summary>
        /// Снимите все ожидающие события и сбросьте галочку на 0.
        /// </summary>
        public static void Clear()
        {
            EventQueue.Clear();
        }

        /// <summary>
        /// Запланируйте событие на будущий тик и верните его.
        /// </summary>
        /// <returns>Событие.</returns>
        /// <param name="tick">Тик.</param>
        /// <typeparam name="T">Параметр типа события.</typeparam>
        static public T Schedule<T>(float tick = 0) where T : Event, new()
        {
            var ev = New<T>();
            ev.tick = Time.time + tick;
            EventQueue.Push(ev);
            return ev;
        }

        /// <summary>
        /// Перенесите существующее событие на следующий тик и верните его.
        /// </summary>
        /// <returns>Событие.</returns>
        /// <param name="tick">Тик.</param>
        /// <typeparam name="T">Параметр типа события.</typeparam>
        static public T Reschedule<T>(T ev, float tick) where T : Event, new()
        {
            ev.tick = Time.time + tick;
            EventQueue.Push(ev);
            return ev;
        }

        /// <summary>
        /// Возвращает экземпляр симуляционной модели для класса.
        /// </summary>
        /// <typeparam name="T">Новый класс</typeparam>
        static public T GetModel<T>() where T : class, new()
        {
            return InstanceRegister<T>.instance;
        }

        /// <summary>
        /// Устанавливает экземпляр симуляционной модели для класса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static public void SetModel<T>(T instance) where T : class, new()
        {
            InstanceRegister<T>.instance = instance;
        }

        /// <summary>
        /// Уничтожает экземпляр симуляционной модели для класса.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static public void DestroyModel<T>() where T : class, new()
        {
            InstanceRegister<T>.instance = null;
        }

        /// <summary>
        /// Тик симуляции. Возвращает количество оставшихся событий.
        /// Если количество оставшихся событий равно нулю, симуляция завершена, 
        /// если только события не добавляются из внешней системы с помощью вызова Schedule().
        /// </summary>
        /// <returns></returns>
        static public int Tick()
        {
            // кэшириуем время
            var time = Time.time;
            // кэшируем порядковый номер исполяемого события
            var executedEventCount = 0;
            // куча событий не пуста и тик самого младшего в ней меньше кэшированного времени
            while (EventQueue.count > 0 && EventQueue.Peek().tick <= time)
            {
                // кэшируем первый элемент в куче событий, выдёргивая его оттуда
                var ev = EventQueue.Pop();
                // кэшируем его тик
                var tick = ev.tick;
                // если событие подготовлено, то выполняем его и активируем событие внутри
                ev.ExecuteEvent();

                if (ev.tick > tick)
                {
                    // мероприятие было перенесено, поэтому не возвращайте его в пул.
                }
                else
                {
                    Debug.Log($"<color=green>{ev.tick} {ev.GetType().Name}</color>");
                    ev.Cleanup();
                    try
                    {
                        EventPools[ev.GetType()].Push(ev);
                    }
                    catch (KeyNotFoundException)
                    {
                        // Это действительно никогда не должно происходить внутри производственной сборки.
                        Debug.LogError($"No Pool for: {ev.GetType()}");
                    }
                }
                executedEventCount++;
            }
            return EventQueue.count;
        }
    }
}