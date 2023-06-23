## Distributed transactions api

In order to scaffold context use:

#### Master
```bash
dotnet ef dbcontext scaffold "Data Source=10.6.0.5,9000;Database=bank;User Id=sa;Password=Passw0rd;Encrypt=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -f --context BankMasterContext --context-dir Data --output-dir Data/Models
```

#### Leaf
```bash
dotnet ef dbcontext scaffold "Data Source=10.6.0.5,9001;Database=bank;User Id=sa;Password=Passw0rd;Encrypt=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -f --context BankLeafContext --context-dir Data --output-dir Data/Models
```


## ToDo:

* Handle FK for users transactions