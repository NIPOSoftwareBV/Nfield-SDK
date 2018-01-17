## Postman

Postman is a tool for working with APIs. It has a lot of nice features: 

* Collections: group API requests together so you find them quickly (instead of scrolling through the History list)
* Environments: easily switch between production, live and development environments
* Variables: store the AuthenticationToken from SignIn request and store it in a variable so you can use it in other API requests. This saves you a lot of annoying copy-pasting. 
* Tests: check status codes and verify responses (e.g. check if all surveys have a name)
* Automate: run a collection for repetitive tasks (sign in, create survey, upload script, publish survey, start fieldwork)
* Generate code: postman can even generate code based on a request

![Postman with Nfield API Collection](Postman.PNG)
*Figure 1. Postman with Nfield API Collections*


## Install and import
You can download the free Postman App at https://www.getpostman.com/. 

After installing the Postman app, you can import the collections via the import button in the upper left corner. You can directly import them via a link or you can download them from this GitHub folder and then import them into Postman. 
The environment files should be imported via the manage environment section in the upper right corner of Postman (you have to download them first from this GitHub folder). If you create your own environment, make sure to add a key value pair for origin, domain, username and password. 

After importing the files, you should fill in the origin, domain, username and password in the environment section. The origin is the url of the api, this is already filled in for the production and beta environment. After this, the Postman App with the Nfield API Collection is ready for use. 

