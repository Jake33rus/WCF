using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    /// <summary>
    /// Обобщенный класс наследуемый от ServiceOperationResult испоьзуется чтобы возвращать на клиент помимо результатов операции, объект сущности
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EssenceSOR<T> : ServiceOperationResult
    {
        public T Essence { get; set; }
    }
}
