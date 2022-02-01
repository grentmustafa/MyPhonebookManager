# Read Me

Develop a reusable library in C# language to manage phone books for users. 
No user interface is required, only an API to create, delete and edit phone book entries. 
Each entry would contain: name (first and last), type (Work, Cellphone or Home) and number. 
Multiple entries under the same name are allowed. 
The persistence format of the phone book library must be a file, (i.e. XML, JSON, Binary etc.) and is not an embedded DB (i.e. SQLite, MySQL, Excel, etc. are not allowed).
In addition to creating/editing/deleting, the library also needs to support iterating over the list in alphabetical order or by the first or last name of each entry.
The library should be ready for other persistence formats even though only the implementation for a file storage is currently required. 
 
Nice-to-do:
· Having unit tests in your project is a plus 
· Thread-safety of the library is a nice-to-have feature.
· XML documentation of the API is welcome, too

- 1 User (identified by first name + last name) -> can have multiple phone numbers of three types.
- 1 phone number -> is owned by one user
- Phone book manager -> a list of users and their respectife numbers

### The source code is orginzed in two subfolder Source and Tests in .NET 6.
1. Under Source Folder the code is structured based on principles of clean architecture with Repository Pattern.
  - PhoneBookManager.Entitiets -> Here are stored the entites of User, PhoneType, and PhoneNumberRecords
  - PhoneBookManager.Reposity ->  Here are defined two interfaces for all neccesary functionalites  for Users & PhoneNumberRecord, and the implementation that works with json files.
  - PhoneBookManager.DTO -> Here are declared all  data transfer objects that will be used on Domain with Automapper
  - PhoneBookManager.Domain -> Here is the core logic of the application
  - PhoneBookManager.WebAPI -> Here a simple Minimal web api that consumes the functionalitites of the Domain
2. Under Tests folder is used to store all projects regarding unit test, integration test, etc
  - PhoneBookManager.UnitTests-> using the prinicples of Dependcy Injection, all services of domain are mocked and used for testing
   
   ![image](https://user-images.githubusercontent.com/41747651/151966817-46543970-eb5b-4ea7-a881-9f6b0bb36604.png)





