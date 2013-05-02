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
using System.Threading.Tasks;
using Nfield.Infrastructure;
using Nfield.Services;
using Ninject;

namespace Nfield.SDK.Samples
{

    /// <summary>
    /// Sample application for demonstrating Nfield SDK usage.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Example of using the Nfield SDK with a user defined IoC container.
            // In most cases the IoC container is created and managed through the application. 
            using(IKernel kernel = new StandardKernel())
            {
                InitializeNfield(kernel);

                const string serverUrl = "http://localhost:82/v1";

                // First step is to get an INfieldConnection which provides services used for data access and manipulation. 
                INfieldConnection connection = NfieldConnectionFactory.Create(new Uri(serverUrl));

                // User must sign in to the Nfield server with the appropriate credentials prior to using any of the services.
                connection.SignInAsync("testdomain", "user1", "password123").Wait();

                // Request the Interviewers service to manage interviewers.
                INfieldInterviewersService interviewersService = connection.GetService<INfieldInterviewersService>();

                // Create a new manager to perform the operations on the service.
                NfieldInterviewersManagement interviewersManager = new NfieldInterviewersManagement(interviewersService);

                // This sample shows various ways of performing synchronous and asynchronous operations on Interviewers.
                var t1 = interviewersManager.AddInterviewerAsync();
                var interviewer2 = interviewersManager.AddInterviewer();
                interviewer2.FirstName = "Harry";
                var t2 = interviewersManager.UpdateInterviewerAsync(interviewer2);

                Task.WaitAll(t1, t2);

                interviewer2 = t2.Result;
                var interviewer1 = t1.Result;
                interviewer1.EmailAddress = interviewer1.EmailAddress + "changed";
                interviewer1.FirstName = "Bob";

                interviewer1 = interviewersManager.UpdateInterviewer(interviewer1);

                var t3 = interviewersManager.ChangePasswordAsync(interviewer2, "ab12345");
                interviewersManager.ChangePassword(interviewer1, "12345ab");

                t3.Wait();

                interviewersManager.QueryForInterviewers();
                interviewersManager.QueryForInterviewersAsync();
                
                interviewersManager.RemoveInterviewerAsync(interviewer1).Wait();
                interviewersManager.RemoveInterviewer(interviewer2); 
            }
        }

        /// <summary>
        /// Example of initializing the SDK with Ninject as the IoC container.
        /// </summary>
        private static void InitializeNfield(IKernel kernel)
        {
            DependencyResolver.Register(type => kernel.Get(type), type => kernel.GetAll(type));

            NfieldSdkInitializer.Initialize((bind, resolve) => kernel.Bind(bind).To(resolve).InTransientScope(),
                                            (bind, resolve) => kernel.Bind(bind).To(resolve).InSingletonScope(),
                                            (bind, resolve) => kernel.Bind(bind).ToConstant(resolve));
        }
    }
}