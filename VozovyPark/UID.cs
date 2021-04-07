using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VozovyPark
{
    [DataContract]
    public class UID
    {
        public static UID instance;

        [DataMember(Name = "ids")]
        private Dictionary<string, int> ids = new Dictionary<string, int>();

        public UID () 
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        public static int newUID<T>()
        {
            string type = typeof(T).ToString();
            if (!instance.ids.ContainsKey(type))
            {
                instance.ids.Add(type, 0);
            }
            return ++instance.ids[type];
        }
    }
}
