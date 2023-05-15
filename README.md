# Animal_Adoption_Management_System_Backend# Animal Adoption Management System

#### *Please note that this is an ongoing pet project of mine, continuously growing and evolving and may not yet have all features properly implemented*
 
## About this API
This is the backend of a complex system that aims to unify and centralise the management of animals living in shelters and looking for a possible adopter providing them a new home. 
Unlike many others, this system not only stores data of a particular shelter. It was designed to store unrelated shelters from anywhere around the world, also different kinds of animals and different users, who are simply looking for adoptable animals or who work at the shelters that are stored in the system. Besides storing different entities, multiple endpoints for creating, updating and querying these entities are also provided.
The main functionality for possible adopters is the opportunity to create so-called AdoptionApplications, that can be later evaluated and either rejected or approved by the employee of the shelter at which the animal is currently residing. The approval of an AdoptionApplication results in an AdoptionContract, that means the animal has been successfully adopted and is no longer residing at the shelter. 


## Main features and technologies
- ASP .NET backend with different controllers and endpoints, using service interfaces for managing different entities (e.g Shelter, Animal, AdoptionApplication)
- Entity Framework as an ORM tool
- PostgreSQL database for storing data, entities with different kinds of relationships (often many-to-many) between them
- IdentityCore used for user authentication and authorisation
- JWT token stored in cookie for managing the authentication and authorisation process
- Image uploading and storing
- Serilog, configured for logging useful information into text files (only used in some authentication-related parts of the application for demo purposes) 
- AutoMapper
- Seeded data with custom DataInititaliser class
- Exception middleware

 
## Images
##### 1. UML diagram of the model classes
<img src="./Images/Animal_Adoption_Management_System_class_diagram.jpeg">


<!-- ##### 2. Controllers and their endpoints
*Image will be added later* -->

 
## Disclaimer
The idea of this project and the particular details are completely mine, any resemblance to an existing API or software is purely coincidental.

