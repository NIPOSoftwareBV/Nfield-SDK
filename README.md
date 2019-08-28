[![Build status](https://niposoftware.visualstudio.com/_apis/public/build/definitions/15ce0e91-931d-4fbf-9169-8c3dde412b54/176/badge)](https://niposoftware.visualstudio.com/Nfield/_build/index?definitionId=176) [![NuGet version](https://badge.fury.io/nu/Nfield.SDK.svg)](https://badge.fury.io/nu/Nfield.SDK)

# NIPO Software Nfield SDK for Windows
This SDK allows you to build applications that take advantage of the Nfield services.
    
## Requirements
- A compatible framework:
  - .Net Framework 4.6.1 or later
  - .Net Standard 2.0 or later
  - .Net Core 2.0 or later
- To use this SDK to call Nfield services you need an Nfield account.

## Usage
The recommended way to consume this project is to reference the NuGet package. You can install it by executing the following command in the Package Manager Console.

```
PM> Install-Package Nfield-SDK
```

Alternatively get the source code of the SDK by cloning this repository and include the _Library_ project in your solution.

## Code Samples
A comprehensive sample project can be found in the _Examples_ folder.
The basic required steps are shown below.

Create a connection.
```c#
INfieldConnection connection = NfieldConnectionFactory.Create(new Uri("https://api.nfieldmr.com/v1/"));
```

Sign in using your Nfield credentials.
```c#
connection.SignInAsync("testdomain", "user1", "password123").Wait();
```

Get a service.
```c#
INfieldInterviewersService interviewersService = connection.GetService<INfieldInterviewersService>();
```

Then you can perform any operations that you want to perform on the service, for example add an interviewer.
```c#
Interviewer interviewer = new Interviewer
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
}
```

## Versioning

There is a file `version.txt` in the root of the repository containing the `major.minor` version.
The build server will append an incrementing third digit.
The number in this file should be increased in a sensible way every time the SDK is changed,
using [semantic versioning](https://semver.org/).

The suffixes that will be appended to the nuget package versions will be the following: 
- no suffix for release versions
- `-beta` for builds from master
- `-alpha` for builds on other branches

## Feedback
For feedback related to this SDK please visit the
[Nfield website].

[Nfield website]: https://www.nipo.com/
