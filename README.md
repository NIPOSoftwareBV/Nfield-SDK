[![Build Status](https://dev.azure.com/niposoftware/Nfield/_apis/build/status/Nfield.SDK%20package?branchName=master)](https://dev.azure.com/niposoftware/Nfield/_build/latest?definitionId=307&branchName=master)
[![NuGet version](https://badge.fury.io/nu/Nfield.SDK.svg)](https://badge.fury.io/nu/Nfield.SDK)

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
await connection.SignInAsync("testdomain", "user1", "password123");
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
```

## Versioning

There is a file `version.txt` in the root of the repository containing the `major.minor` version.
The build server will append an incrementing third digit.
The number in this file should be increased in a sensible way every time the SDK is changed,
using [semantic versioning](https://semver.org/).

The suffixes that will be appended to the nuget package versions will be the following: 
- no suffix for release versions for builds from release/*
- `-beta` for builds from master
- `-alpha` for builds on other branches

## Releases
 - How to create and publish a new `-alpha` or `-beta` version. These versions can only be published on a DevOps private/internal Nfield Feed:
    - Create a new branch with this pattern: `*/ci-*` or merge a commit into `master`
    - This branch will trigger a new build in DevOps pipelines
    - After the build a `Release pipeline` will allow to Push the new nuget version in the `Nfield` internal feed. These versions __don't create__ `GitHub releases`.
 - How to create and publish a new `release` on the Nuget.org public Feed
    - Create a new release branch Example: `release/2023-P3S5`
    - This branch will trigger a new build in DevOps pipelines
    - After the build a `Release pipeline` will allow to Push the new nuget version in the `Nuget.org` feed and it will create the `GitHub release` repo.

## Feedback
For feedback related to this SDK please visit the
[Nfield website].

[Nfield website]: https://www.nipo.com/

