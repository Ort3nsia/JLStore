```
JLS/                       (progetto Web API)
├─ Controllers/
│   └─ CustomerController.cs
│
├─ Domain/
│   ├─ Models/
│   │   └─ Customer.cs
│   ├─ Repositories/
│   │   └─ ICustomerRepository.cs
│   └─ Services/
│       ├─ ICustomerService.cs
│       └─ CustomerService.cs
│
├─ Dto/
│   ├─ CustomerDto.cs
│   ├─ CustomerCreateDto.cs
│   └─ CustomerUpdateDto.cs
│
├─ Infrastructure/
│   ├─ Data/
│   │   ├─ DataContext.cs
│   │   └─ DataSeed.cs
│   └─ Repositories/
│       └─ CustomerRepository.cs
│
├─ Mappings/
│   └─ CustomerProfile.cs
│
├─ Helpers/
│   └─ StringHelpers.cs
│
├─ Migrations/
│   └─ ... (generate da EF Core)
│
├─ appsettings.json
├─ appsettings.Development.json
└─ Program.cs
```
