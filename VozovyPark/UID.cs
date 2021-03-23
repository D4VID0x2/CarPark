using System;
using System.Collections.Generic;

namespace VozovyPark
{
    public static class UID
    {
        private static Dictionary<System.Type, int> ids = new Dictionary<System.Type, int>();

        public static int newUID<T>()
        {
            return ++ids[typeof(T)];
        }
    }
}
