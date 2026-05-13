# Coworking Booking System Diagram

This diagram documents the custom Coworking domain used by the project.

- One-to-many examples: `Branch -> Room`, `Branch -> Desk`, `Room/Desk -> Booking`
- Many-to-many example: `Branch <-> Amenity` through `BranchAmenity`
- User authentication and roles are provided by the separate `Users.API` microservice.

Open `CoworkingBookingSystem_Diagram.svg` to view the diagram.
