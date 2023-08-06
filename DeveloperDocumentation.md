# Documentation
## Controllers
The website has two main controllers - UserController and AdminController. As the names suggests one is used for pages the user can access, the other is for the pages admin can access.
Every page in both controllers gets redirected to the login page and prompts the user to log in. See *Login* for more information.
## Models
The main models are: Admin, MultipartAnswer, Question, Team, User, UserToQuestion. Each have their own repository to access database.
### Database
The communication with the database is done using MySql.EntitiyFrameworkCore NuGet by Oracle. 
The main access point is in DatabaseContext where the DbSets mirror the structure of the database.
Every model has its own repository. The repository derives from an abstract generic class DatabaseRepository<T> that itself implements the IRepository<T> interface.
THe IRepository<T> interface contains basic CRUD actions.
## Login
Login for both Admins and Users is done by comparing hashed passwords in the Hasher class. The hashing uses the Pdkdf2 hashing algorithm. 
## Question codes
To prevent users from guessing the correct name/id of a question, the id gets hashed with constant salt with the same algorithm used for login.
This 16 character long code is then used for selecting the correct question from the database.
