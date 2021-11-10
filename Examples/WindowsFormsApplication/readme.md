# Windows Forms sample

This sample demonstrates using SSO through AAD in a Windows Forms application.
In order to use this sample you will have to perform a number of steps.

## Setup

To support SSO a new application needs to be added to the tenant that will be used to authenticate the user.

### _Login to the [Azure portal](https://portal.azure.com) and go to `App registrations`_

![App registrations](./images/AppRegistrations.png)

### _Register a new application_

![New app registration](./images/NewRegistration.png)
![Register an application](./images/RegisterApplication.png)

### _Take note of the appropriate settings_  

If you now look at the application overview a number of details are shown that will be used later.
Store the values of the application ID and the tenant ID.

![Initial app details](./images/InitialAppDetails.png)

### _Add a platform for our Windows Forms application_

In order for our app to use this application registration you will have to tell AAD through which means it will authenticate.

![Add a platform](./images/AddPlatform.png)

### _Add support for `Mobile and desktop applications`_
![Mobile and desktop applications](./images/AddMobileDesktopApplication.png)

### _Setup `Redirect URIs`_

In order to support the ability to show the browser to select the account to use for logging in to Nfield the following URIs should be configured. The `http://localhost` URI is needed when using .Net Core, it will allow the application to capture the result of the login from the browser.

![Redirect URIs](./images/RedirectUris.png)

### _Check authentication details_

The authentication details should now look as following:
![Authentication details](./images/AuthenticationAfterAddPlatform.png)

### _Now give the new application access to the Nfield API_

Now give the application permission to perform actions on the Nfield API on behalf of the user

![Add permissions](./images/AddPermission.png)

### _Select the Nfield Public API_

Look up the Nfield Public API in the `APIs my organizations uses` and store the its Application ID.

![Select Nfield Public API](./images/SelectNfieldPublicAPI.png)

### _Add all Nfield permissions_

![Add all permissions](./images/AddAllNfieldPublicAPIPermissions.png)

Our application is good to go and we should have the following information stored to use in the sample application:
- Client ID: this is the ID of the application we just registered
- Tenant ID: the ID of the tenant that we are signing into
- Nfield Public API Application ID: The ID of the Nfield Public API

These values will have to be added to `appsettings.json`.
Besides that the name of the domain should be added to this config as that should be added as a header to each API call. Luckily the Nfield SDK will take care of that for us.