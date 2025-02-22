namespace Core
{
   /// <summary>
/// Событие — это то, что происходит в определённый момент времени в ходе моделирования. 
/// Метод предварительных условий используется для проверки того, следует ли выполнять событие, 
/// поскольку условия в ходе моделирования могли измениться с момента первоначального планирования события.
/// </summary>
/// <typeparam name="Event"></typeparam>
public abstract class Event : System.IComparable<Event>
{
    internal float tick;
    /// <summary>
    /// Этот метод используется для сравнения двух объектов типа Event. 
    /// Он возвращает результат сравнения значений их свойств tick с помощью метода CompareTo. 
    /// Если значение свойства tick текущего объекта больше, чем у other.tick, возвращается положительное число. 
    /// Если меньше — отрицательное число. В случае равенства значений возвращается 0.
    /// </summary>
    /// <param name="other">Сравниваемый тик у другого события</param>
    /// <returns>Числовой результат сравнения</returns>
    public int CompareTo(Event other)
    {
        return tick.CompareTo(other.tick);
    }

    public abstract void Execute();

    public virtual bool Precondition() => true;

    internal virtual void ExecuteEvent()
    {
        if (Precondition())
            Execute();
    }

    /// <summary>
    /// Этот метод обычно используется для установки ссылок на нулевые значения, когда это необходимо. 
    /// Он автоматически вызывается симуляцией после завершения события.
    /// </summary>
    internal virtual void Cleanup()
    {

    }
}

public abstract class Event<T> : Event where T : Event<T>
{
    public static System.Action<T> OnExecute;

    internal override void ExecuteEvent()
    {
        if (Precondition())
        {
            Execute();
            OnExecute?.Invoke((T)this);
        }
    }
}
}