# Book e-shop
Purpose: **Learn new development practices**  
Goal: Develop a WebAPI book e-shop
## Stack:
- [x] **.NET WebAPI**
- [x] **Postgres+Dapper**
- [x] **Memory cache**
- [x] **xUnit**
  - [x] Integration
  - [x] Unit
- [x] **Clean architecture**
  - [x] CQRS
  - [x] MediatR
  - [x] Layers
- [x] **FluentMigration**
- [x] **FluentValidation**
- [ ] **k8s deploy**
- [x] **Swagger**
- [x] **Serilog**
  - [x] Context based exceptions
  - [x] Enrich log with trace identifier
  - [ ] Sink to ES/Grafana ?
  - [ ] Move setting to configuration file
- [x] **Mapster**
- [x] **Unit of work pattern**
- [ ] **Outbox pattern**
- [ ] **Authentication**

## Setup the solution
- Postgres setup/Change connection string
    - local postgres in docker by command `docker run --name postgres-db -e POSTGRES_PASSWORD=admin -p 5432:5432 -d postgres`
- Restore nugets
- Build `Migrator` project
- Run migrator up (args: `-up`)
- Run Web project
- Api spec is on `/swagger` endpoint