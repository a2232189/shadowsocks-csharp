﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Shadowsocks.Obfs
{
    public static class ObfsFactory
    {
        private static Dictionary<string, Type> _registeredObfs;

        private static Type[] _constructorTypes = new Type[] { typeof(string) };

        static ObfsFactory()
        {
            _registeredObfs = new Dictionary<string, Type>();
            foreach (string method in Plain.SupportedObfs())
            {
                _registeredObfs.Add(method, typeof(Plain));
            }
            foreach (string method in HttpSimpleObfs.SupportedObfs())
            {
                _registeredObfs.Add(method, typeof(HttpSimpleObfs));
            }
        }

        public static IObfs GetObfs(string method)
        {
            if (string.IsNullOrEmpty(method))
            {
                method = "plain";
            }
            method = method.ToLowerInvariant();
            Type t = _registeredObfs[method];
            ConstructorInfo c = t.GetConstructor(_constructorTypes);
            IObfs result = (IObfs)c.Invoke(new object[] { method });
            return result;
        }
    }
}
