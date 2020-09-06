# UserManagerAPI
Basic auth logic using JWT, chat using SignalR and CRUD operations with users are implemented with the help of ASP.NET Core Web API.

You can download Postman collection with endpoints here https://www.getpostman.com/collections/cad3a51c49e0ce4dcdba.

Needed credentials for every endpoint are filled in Body tab in Postman collection. You must specify a valid auth token for each request, which returns 401, in Authorization tab, type - Bearer token.

If you wish to use the app, follow these steps:
1. Clone this repo. 
2. Run **Update-Database** in **Package Manager Console**. 
*Note:* Database will be seeded with 3 users when you run the app for the first time.

The app consists of **3** modules:

### Authorization
1. To log in use **Auth/Login** endpoint. *You will need to be logged in to use all endpoints regarding users unless stated otherwise.*

### Users (CRUD)
1. To view list of all users use **Users/Get all** endpoint. You do not have to be authorized for this.
2. To view user details use **Users/Get user details** endpoint.
3. To create new user use **Users/Create user** endpoint.
4. To update existing user use **Users/Update user details** endpoint.
5. To delete existing user use **Users/Delete user** endpoint.

### Chat (Using SignalR)
1. To send message in chat use **Chat/Send Message** endpoint. A JS file to test chat is available on request.
