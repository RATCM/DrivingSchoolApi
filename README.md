# Driving School API
## Project structure
<img src="https://www.milanjovanovic.tech/blogs/mnw_004/clean_architecture.png" alt="Clean architecture structure">


This project follows a clean architecture structure.

The following layers in the project are:
- [Domain](DrivingSchoolApi.Domain)
  - Contains entities and value objects
  - Classes here are stored in the database
- [Application](DrivingSchoolApi.Application)
  - Contains use-cases
  - Defines interfaces for repositories
- [Infrastructure](DrivingSchoolApi.Infrastructure)
  - Contains external services, in our case PostgreSql
  - Implements interfaces from the application layer
- [Presentation](DrivingSchoolApi)
  - Contains the API endpoints and DTOs

In principle, the presentation layer should not depend on the infrastructure
layer in any regard, but in our case it was necessary in order to set up
the dependency injection for the API controllers.

Because of this, we should keep all classes, either internal or private in the infrastructure layer
except for the [DependencyInjection](DrivingSchoolApi.Infrastructure/DependencyInjection.cs) class which only contains a single extension method


### Domain layer
#### Folder structure
```md
.
├── Entities        # Objects with their identity defined by a single id
├── Exceptions      # Domain Exceptions
├── Keys            # IDs for Entities
├── Primitives      # Abstract classes
└── ValueObjects    # Immutable objects with their identity only defined by their fields
```

### Entities
```mermaid
---
title: Entities class diagram
---
classDiagram
    class DrivingLesson {
        Id
        Route
        Price
    }
    class DrivingSchool {
        Id
        DrivingSchoolName
        SchoolAddress
        PhoneNumber
        WebAddress
        
        +ChangeName(newName)
        +ChangeAddress(newAddress)
        +CangePhoneNumber(newNumber)
        +ChangeWebAddress(newWebAddress)
    }
    class Instructor {
        Id
        InstructorName
        EmailAddress
        HashedPassword
        PhoneNumber
        
        +ChangeSchool(newSchoolId)
        +ChangeName(newName)
        +ChangeEmail(newEmail)
        +ChangePasswordHash(newHash)
        +ChangePhoneNumber(newNumber)
    }
    class Student{ 
        Id
        StudentName
        EmailAddress
        HashedPassword
        PhoneNumber

        +ChangeSchool(newSchoolId)
        +ChangeName(newName)
        +ChangeEmail(newEmail)
        +ChangePasswordHash(newHash)
        +ChangePhoneNumber(newNumber)
    }
    class TheoryLesson{
        Id
        LessonDateTime
        Price
    }

    Student --> DrivingSchool: SchoolId
    Instructor --> DrivingSchool: SchoolId
    
    TheoryLesson "many"-->"many" Student: StudentIds
    TheoryLesson --> Instructor: InstructorId
    TheoryLesson --> DrivingSchool: SchoolId
    
    DrivingLesson --> Student: StudentId
    DrivingLesson --> Instructor: InstructorId
    DrivingLesson --> DrivingSchool: SchoolId
```


## Setup
### Docker compose
In order to make project work properly, you need to create a file named ".env" on the root directory of the project

In this file you need to define the following:
- DB_USER
  - The username for the postgres database
  - e.g. DB_USER=dev
- DB_PASSWD
  - The password for the postgres database
  - e.g. DB_PASSWD=dev
- DB_PORT_HOST
  - The port number on your host machine where you want to receive traffic for the database
  - e.g. DB_PORT_HOST=5430
- DB_PORT_CONTAINER
  - The port number within the container that's listening for connections for the database
  - e.g. DB_PORT_CONTAINER=5431
- API_PORT_HOST
    - The port number on your host machine where you want to receive traffic for the API
    - e.g. API_PORT_HOST=5259
- API_PORT_CONTAINER 
  - The port number within the container that's listening for connections for the API
  - e.g. API_PORT_HOST=5260

#### Example
Using the examples above, the .env file would look something like this
```dotenv
DB_USER=dev
DB_PASSWD=dev
DB_PORT_HOST=5430
DB_PORT_CONTAINER=5431

API_PORT_HOST=5259
API_PORT_CONTAINER=5260
```
