# TestingStudentSystem_Microservice

Project implementing microservices project based on the requirements from *Head First Software Architecture book*, Chapter 12.


## Architecture & Services

The system is composed of the following services:

- **StudentAccountManagement** — handles student and teacher accounts, registration, login, Auth, ApiGetWay.  
- **TestManagement** — handles operations about tests (creating, scheduling(not implemented), taking tests).  
- **GradingManagement** — handles grading logic and computing scores.  
- **SharedLogger** — cross-cutting logging utility shared by multiple services.

- **RabbitMq** — used for communication between the test service and grade service.
- **Serilog and seq** — used to log the messegeses and see it in UI.
- **Yarp** — used as a ApiGetWay, it is installed in **StudentAccountManagement** service.
- **SignalR** — Used so the teacher can observe the student answering the test in real time during the test.
- **Manual Mediator** — Use manual mediator insted of MediatR as now MediatR package is not free.
- **Manual Mapping** — Use manual mapping insted of AutoMapper to gain flexibility.
- **gRPC** — use gRPC to get the students emails from Auth service to TestManagment service.
- **MailKit** — use MailKit package to use gmail service to notify the students with upcomming tests.

- **DDD** — I tried to use Domain Driven Design tactical part to create Entities, Value Objects, events... 
- **Archeticture** — All services are implemented using Onion Archeticture [Domain Layer, Application Layer, Presentation Layer, Infrastracture Layer] 

# How to run 
You need to have docker only to run the project 

### clone the repo:

git clone https://github.com/joseph-tharwat/TestingStudentSystem_Microservice.git
cd TestingStudentSystem_Microservice/

### start the docker swarm: 

docker swarm init 

if can not run add your ip like this: docker swarm init  --advertise-addr "192.168.0.19"

### deploy the stack:
docker stack deploy -c docker-stack.yml system

To make sure that the service is up and running:
docker stack services system
![alt text](Images/outputContainers.png)

### open the logs:
You can open seq to see the logs on: localhost:5341/#

### Use the project: 
you can test the project with the samples in the "Simple Examples for testing Docker.txt" file

