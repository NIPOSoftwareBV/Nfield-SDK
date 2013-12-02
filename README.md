<h1>NIPO Software Nfield SDK for Windows</h1>
<p>This SDK allows you to build applications that take advantage of the Nfield services.</p>
    
<h1>Requirements</h1>
<ul>
    <li>.NET Framework 4.0 or later</li>
    <li>To use this SDK to call Nfield services you need an Nfield account.</li>
</ul>

<h1>Usage</h1>
<p>To get the source code of the SDK clone this repository and include the <code>Library</code> project in your solution.</p>

<h1>Code Samples</h1>
<p>A comprehensive sample project can be found in the <code>Examples</code> folder.</p>
<p>The basic required steps are shown below.</p>
<p>First we need to use and register a dependency resolver.
In this example we're using
<a href="http://www.ninject.org/">Ninject</a>.</p>
<pre>using(IKernel kernel = new StandardKernel()) {
    DependencyResolver.Register(type => kernel.Get(type), type => kernel.GetAll(type));
    NfieldSdkInitializer.Initialize((bind, resolve) => kernel.Bind(bind).To(resolve).InTransientScope(),
                                    (bind, resolve) => kernel.Bind(bind).To(resolve).InSingletonScope(),
                                    (bind, resolve) => kernel.Bind(bind).ToConstant(resolve));
</pre>
<p>Create a connection.</p>
<pre>    INfieldConnection connection = NfieldConnectionFactory.Create(new Uri("https://api.nfieldmr.com/"));</pre>
<p>Sign in using your Nfield credentials.</p>
<pre>    connection.SignInAsync("testdomain", "user1", "password123").Wait();</pre>
<p>Get a service.</p>
<pre>    INfieldInterviewersService interviewersService = connection.GetService<INfieldInterviewersService>();</pre>
<p>Then you can perform any operations that you want to perform on the service, for example add an interviewer.</p>
<pre>    Interviewer interviewer = new Interviewer
            {
                ClientInterviewerId = "sales123",
                FirstName = "Sales",
                LastName = "Team",
                EmailAddress = "sales@niposoftware.com",
                TelephoneNumber = "+31 20 5225989",
                UserName = "sales",
                Password = "password12"
            };
    await _interviewersService.AddAsync(interviewer);
}</pre>

<h1>Feedback</h1>
<p>For feedback related to this SDK please visit the
<a href="http://www.nfieldmr.com/contact.aspx">Nfield website</a>.</p>
