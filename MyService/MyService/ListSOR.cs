using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyService
{
    /// <summary>
    /// Обобщенный класс наследуемый от ServiceOperationResult испоьзуется чтобы возвращать на клиент помимо результатов операции, типизированный список 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListSOR<T>:ServiceOperationResult
    {
       public List<T> List { get; set; }
    }
}
