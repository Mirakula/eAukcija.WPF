using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eAukcija.ViewModel
{
    public class Mediator
    {
        private static readonly Mediator instance = new Mediator();

        public static Mediator Instance
        {
            get
            {
                return instance;
            }
        }

        private Mediator() { }

        private static Dictionary<string, Action<object>> subscriber = new Dictionary<string, Action<object>>();

        public void Register (string message, Action<object> action)
        {
            subscriber.Add(message, action);
        }

        public void Notify(string message, Object param)
        {
            foreach (var item in subscriber)
            {
                if (item.Key.Equals(message))
                {
                    Action<object> method = (Action<object>)item.Value;
                    method?.Invoke(param);
                }
            }
        }
    }
}
