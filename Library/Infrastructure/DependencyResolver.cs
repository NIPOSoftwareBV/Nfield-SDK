//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Exposes the implementation of the IoC container through the <see cref="Current"/> property.
    /// To register your own implementation use one of the 'Register' overloads to register your favorite IoC container.
    /// </summary>
    public class DependencyResolver
    {
        private static readonly DependencyResolver Instance = new DependencyResolver();

        private IDependencyResolver _current = new DefaultDependencyResolver();

        /// <summary>
        /// The current <see cref="IDependencyResolver"/>
        /// </summary>
        public static IDependencyResolver Current
        {
            get { return Instance._current; }
        }

        /// <summary>
        /// Register a class that implements <see cref="IDependencyResolver"/> to be the current implementation.
        /// </summary>
        public static void Register(IDependencyResolver dependencyResolver)
        {
            Instance.RegisterDependencyResolver(dependencyResolver);
        }

        /// <summary>
        /// Register a class that implements the Common Service Locator (http://commonservicelocator.codeplex.com/"/>)
        /// IServiceLocator to be the current implementation. The actual implementation will be a wrapper that maps to
        /// the service locator methods.
        /// </summary>
        public static void Register(object commonServiceLocator)
        {
            Instance.RegisterCommonServiceLocator(commonServiceLocator);
        }

        /// <summary>
        /// Register methods that should be used to resolve dependencies. The actual implementation will be a wrapper 
        /// that maps to these methods.
        /// </summary>
        public static void Register(Func<Type, object> resolveMethod, Func<Type, IEnumerable<object>> resolveAllMethod)
        {
            Instance.RegisterDelegates(resolveMethod, resolveAllMethod);
        }

        private void RegisterDependencyResolver(IDependencyResolver dependencyResolver)
        {
            if (dependencyResolver == null)
            {
                throw new ArgumentNullException("dependencyResolver");
            }
            _current = dependencyResolver;
        }

        private void RegisterCommonServiceLocator(object commonServiceLocator)
        {
            if (commonServiceLocator == null)
            {
                throw new ArgumentNullException("commonServiceLocator");
            }
            var commonServiceLocatorType = commonServiceLocator.GetType();
            var getInstanceMethod = commonServiceLocatorType.GetMethod("GetInstance", new[] {typeof (Type)});
            var getAllInstancesMethod = commonServiceLocatorType.GetMethod("GetAllInstances", new[] {typeof (Type)});

            if (getInstanceMethod == null || getAllInstancesMethod == null || getInstanceMethod.ReturnType != typeof (object) ||
                getAllInstancesMethod.ReturnType != typeof (IEnumerable<object>))
            {
                throw new ArgumentException(
                    String.Format("The type {0} does not appear to implement IServiceLocator.", commonServiceLocatorType),
                    "commonServiceLocator");
            }
            var resolveMethod = (Func<Type, object>)Delegate.CreateDelegate(typeof(Func<Type, object>), commonServiceLocator, getInstanceMethod);
            var resolveAllMethod = (Func<Type, IEnumerable<object>>)Delegate.CreateDelegate(typeof(Func<Type, IEnumerable<object>>), commonServiceLocator, getAllInstancesMethod);

            RegisterDelegates(resolveMethod, resolveAllMethod);
        }

        private void RegisterDelegates(Func<Type, object> resolveMethod, Func<Type, IEnumerable<object>> resolveAllMethod)
        {
            if (resolveMethod == null)
            {
                throw new ArgumentNullException("resolveMethod");
            }
            if (resolveAllMethod == null)
            {
                throw new ArgumentNullException("resolveAllMethod");
            }
            RegisterDependencyResolver(new DelegateBasedDependencyResolver(resolveMethod, resolveAllMethod));
        }

        private class DefaultDependencyResolver : IDependencyResolver
        {
            public object Resolve(Type typeToResolve)
            {
                if (typeToResolve.IsInterface || typeToResolve.IsAbstract)
                {
                    return null;
                }

                try
                {
                    return Activator.CreateInstance(typeToResolve);
                }
                catch
                {
                    return null;
                }
            }

            public IEnumerable<object> ResolveAll(Type typeToResolve)
            {
                return Enumerable.Empty<object>();
            }
        }

        private class DelegateBasedDependencyResolver : IDependencyResolver
        {

            private readonly Func<Type, object> _resolveMethod;

            private readonly Func<Type, IEnumerable<object>> _resolveAllMethod;

            public DelegateBasedDependencyResolver(
                Func<Type, object> resolveMethod, Func<Type, IEnumerable<object>> resolveAllMethod)
            {
                _resolveMethod = resolveMethod;
                _resolveAllMethod = resolveAllMethod;
            }

            #region Implementation of IDependencyResolver

            /// <summary>
            /// Resolves a single registered type.
            /// </summary>
            /// <param name="typeToResolve"></param>
            /// <returns></returns>
            public object Resolve(Type typeToResolve)
            {
                return _resolveMethod(typeToResolve);
            }

            /// <summary>
            /// Resolves multiple registered instances.
            /// </summary>
            /// <param name="typeToResolve">The type to resolve</param>
            /// <returns></returns>
            public IEnumerable<object> ResolveAll(Type typeToResolve)
            {
                return _resolveAllMethod(typeToResolve);
            }

            #endregion
        }

    }
}
