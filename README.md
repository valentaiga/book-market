# Book e-shop
Goal: **Learn new development practices**  
Purpose: Develop a WebAPI book e-shop
## Stack:
- [x] **.NET WebAPI**
- [x] **Postgres**
- [ ] **xUnit**
  - [ ] Module
  - [ ] Integration
  - [ ] Unit
- [x] **Clean architecture**
  - [x] SQRS
  - [x] MediatR
  - [x] Layers
- [x] **Dapper**
- [x] **FluentMigration**
- [x] **FluentValidation**
- [ ] **k8s deploy**
- [x] **Swagger**
- [x] **Serilog**
  - [x] Context based exceptions
  - [x] Enrich log with trace identifier
  - [ ] Sink to ES/Grafana ?
  - [ ] Move setting to configuration file
- [ ] **Mapster**
- [x] **Unit of work pattern**
- [ ] **Outbox pattern**
- [ ] **Authentication**

## Setup the solution
- Postgres setup/Change connection string
    - local postgres in docker by command `docker run --name postgres-db -e POSTGRES_PASSWORD=admin -p 5432:5432 -d postgres`
- Restore nugets
- Build project
- Run migration UP (args: `-migrate up`)
- Run Web project
- Swagger is on `/swagger` endpoint