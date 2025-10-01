```
JLS
├── Program.cs
├── ... (File di configurazione ecc...)
├── FrameworkAndDrivers/
│   ├── Externals/
│   │   └── CustomersDbContext.cs
│   └── Repository/
│       └── CustomersRepository.cs
├── InterfaceAdapters/
│   ├── Interfaces/
│   │   └── ICustomersController.cs
│   └── Controllers/
│       └── CustomersController.cs
├── UseCases/
│   ├── Interfaces/
│   │   └── ICustomersService.cs
│   ├── DTOs/
│   │   ├── CustomersCreateDTO.cs
│   │   ├── CustomersUpdateDTO.cs
│   │   └── CustomersReadDTO.cs
│   └── Services/
│       └── CustomersServices.cs
└── Entities/
    ├── Interfaces/
    │   └── ICustomersRepository.cs
    └── Domain/
        └── Customers.cs
```
