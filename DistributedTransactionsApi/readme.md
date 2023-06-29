## Distributed transactions api

In order to scaffold context use:

#### Leaf

```bash
dotnet ef dbcontext scaffold "Name=Leaf_1"  Microsoft.EntityFrameworkCore.SqlServer -f --no-onconfiguring --context BankLeafContext --context-dir Data --output-dir Data/Models/Leaf/
```

#### Master

```bash
dotnet ef dbcontext scaffold "Name=Master" Microsoft.EntityFrameworkCore.SqlServer -f --no-onconfiguring --context BankMasterContext --context-dir Data --output-dir Data/Models/Master/
```

## ToDo:

* Handle FK for users transactions