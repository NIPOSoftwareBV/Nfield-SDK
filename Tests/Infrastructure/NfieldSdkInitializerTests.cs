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
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Moq;
using Nfield.Models;
using Xunit;

namespace Nfield.Infrastructure
{
    /// <summary>
    /// Tests for <see cref="NfieldSdkInitializer"/>
    /// </summary>
    public class NfieldSdkInitializerTests
    {
        private readonly List<Type> _interfaceList = new List<Type>();

        [Fact]
        public void TestSignInAsync_CredentialsAreIncorrect_ReturnsFalse()
        {
            NfieldSdkInitializer.Initialize(RegisterTransient, RegisterSingleton, RegisterObject);

            // Add interfaces that not need to be registered
            _interfaceList.Add(typeof(IInterviewer));
            _interfaceList.Add(typeof(IDependencyResolver));
            _interfaceList.Add(typeof(INfieldConnection));
            _interfaceList.Add(typeof(INfieldConnectionClient));
            _interfaceList.Add(typeof(INfieldConnectionClientObject));

            foreach (var assemblyType in typeof(NfieldSdkInitializer).Assembly.GetTypes())
            {
                if (assemblyType.IsInterface)
                    Assert.Contains(assemblyType, _interfaceList);
            }
        }

        private void RegisterTransient(Type interfaceType, Type classType)
        {
            Assert.True(interfaceType.IsAssignableFrom(classType));
            _interfaceList.Add(interfaceType);
        }

        private void RegisterSingleton(Type interfaceType, Type classType)
        {
            Assert.True(interfaceType.IsAssignableFrom(classType));
            _interfaceList.Add(interfaceType);
        }

        private void RegisterObject(Type interfaceType, Object objectReg)
        {
            Assert.True(interfaceType.IsInstanceOfType(objectReg));
            _interfaceList.Add(interfaceType);
        }
    }
}
