# AAD Authentication

We recently added support for AAD (Office 365) authentication to the Nfield API. To
use this from within NIPO, you can follow these steps.

## Download the ADTokenHelper tool

This tool can be found
[here](https://ktglbuc.sharepoint.com/sites/niposoftware/Shared%20Documents/Forms/AllItems.aspx?id=%2Fsites%2Fniposoftware%2FShared%20Documents%2FTools%20and%20Installers%2FADTokenHelper).
You need to download all the files, or the program won't work. You can
double-click the executable to get a token. This token will be copied to your
clipboard. Note that tokens expire at some point. When that happens you can
just double-click the executable again to get a new one.

## Using Postman

The collections in this folder use AAD Authentication. Each request have On the `Authorization` tab of each request, the authorization type is set to `Inherent auth from parent`. In the collection folder, click edit to edit the collection. On the `Authorization` tab, the option `Bearer Token` is selected. In the `Token` textbox, you should paste the token you got from the ADTokenHelper tool. You can now update and close the collection. 

## Access

On the `Headers` tab of the request, you should add a new header with the key `X-Nfield-Domain` and the value `NIPO Beta` or whatever domain you want to use. 

For now, everyone in NIPO has access to the following domains using AAD:

|Region|Domain|
|---|---|
|AM|NIPO Software AM|
|EU|NIPO Software EU|
|AP|NIPO Software APAC|
|Beta|NIPO Beta|

If you want or need access to more domains, you can request this of course.
Ops needs to add a tag to this domain to enable assignments. Then a developer
(for now) needs to perform the assignment itself. Good candidates are demo
domains, but in theory this will work for *any* domain.
