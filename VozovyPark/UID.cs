using System;
using System.Collections.Generic;

namespace VozovyPark
{
    public static class UID
    {
        private static Dictionary<System.Type, int> ids = new Dictionary<System.Type, int>();

        public static int newUID<T>()
        {
            if (!ids.ContainsKey(typeof(T)))
            {
                ids.Add(typeof(T), 0);
            }
            return ++ids[typeof(T)];
        }
    }
}
