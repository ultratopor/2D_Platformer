using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// Предоставляет коллекцию очередей, которая всегда упорядочена.
    /// </summary>
    /// <typeparam name="T">Всё, что реализует IComparable</typeparam>
    public class HeapQueue<T> where T : IComparable<T>
    {
        private List<T> items;

        public int count => items.Count;

        public bool isEmpty => items.Count == 0;

        public T first => items[0];     // то же что и Peek

        public void Clear() => items.Clear();

        public bool Contains(T item) => items.Contains(item);

        public void Remove(T item) => items.Remove(item);

        public T Peek() => items[0];    // то же что и first

        public HeapQueue()
        {
            items = new List<T>();
        }
         
        /// <summary>
        /// Добавление нового элемента и просеивание вниз массива
        /// </summary>
        /// <param name="item">Добавляемый элемент</param>
        public void Push(T item)
        {
            // добавляем в конец списка - дерева
            items.Add(item);
            // Находим правильное положение элемента просеиванием вниз
            SiftDown(0, items.Count - 1);
        }

        /// <summary>
        /// Удаляет и возвращает элемент из вершины структуры данных (первый или единственный в массиве)
        /// </summary>
        /// <returns>Извлечённый элемент</returns>
        public T Pop()
        {
            // если элементов больше одного, возвращаемый элемент будет первым в дереве.
            // затем добавьте последний элемент в начало дерева, сократите список
            // и найдите правильный индекс в дереве для первого элемента.
            T item;
            // Извлекает последний элемент массива
            var last = items[^1];
            // Удаляет последний элемент из массива
            items.RemoveAt(items.Count - 1);
            // Если массив не пустой
            if (items.Count > 0)
            {
                // кэширует первый элемент
                item = items[0];
                // первому элементу присваивает последний
                items[0] = last;
                // просеивает вверх весь массив
                SiftUp();
            }
            else
            {
                item = last;
            }
            return item;
        }

        /// <summary>
        /// Сравнение a и b
        /// </summary>
        /// <param name="a">Сравниваемый член коллекции</param>
        /// <param name="b">Сравниваемый член коллекции</param>
        /// <returns> Отрицательное число, если a меньше b, положительное число, если a больше b
        /// и 0, если объекты равны.</returns>
        private int Compare(T a, T b) => a.CompareTo(b);
        
        /// <summary>
        /// Просеивание кучи вниз под видом бинарного дерева
        /// </summary>
        /// <param name="startPos">Индекс начального элемента в массиве</param>
        /// <param name="pos">Индекс текущего индекса, который нужно просеять вниз</param>
        private void SiftDown(int startPos, int pos)
        {
            // кэшируем элемент по входящему индексу
            var newItem = items[pos];
            // пока входящий индекс больше стартового
            while (pos > startPos)
            {
                // кэшируем родительский индекс битовым сдвигом (в два раза меньше в меньшую сторону)
                var parentPos = (pos - 1) >> 1;
                var parent = items[parentPos];
                //если новый элемент предшествует родительскому или равен ему, pos — это позиция нового элемента.
                if (Compare(parent, newItem) <= 0)
                    break;
                // в противном случае родителя суём в ячейку по индексу, 
                // а индекс родителя суём в индекс текущий
                items[pos] = parent;
                pos = parentPos;
            }
            items[pos] = newItem;
        }
        /// <summary>
        /// Дважды просеивает массив под видом бинарного дерева после того, как первый элемент заменили последним
        /// </summary>
        void SiftUp()
        {
            var endPos = items.Count;
            var startPos = 0;
            // Кэшируем первый элемент
            var newItem = items[0];
            var childPos = 1;
            var pos = 0;
            // Поиск позиции для вставки
            while (childPos < endPos)
            {
                // Кэшируем правую позицию
                var rightPos = childPos + 1;
                //Если ещё не дошли до конца и правая ветвь меньше левой, то левая сдвигается в право
                if (rightPos < endPos && Compare(items[rightPos], items[childPos]) <= 0)
                    childPos = rightPos;
                // Присваиваем меньшему значению левую позицию, поднимая вверх в дереве
                items[pos] = items[childPos];
                pos = childPos;
                // опускаемся в дерево ниже (нечётные элементы массива) и повторяем
                childPos = 2 * pos + 1;
            }
            // присваиваем новый элемент в дочернюю позицию
            items[pos] = newItem;
            SiftDown(startPos, pos);
        }
    }
}