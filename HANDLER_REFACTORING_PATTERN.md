# Handler Refactoring Pattern Reference

## Standard Handler Pattern After Refactoring

### Query Handler Example (BranchQueryHandler)

```csharp
using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchQueryHandler : IRequestHandler<BranchQueryRequest, BranchQueryResponse>
    {
        private readonly BranchService _service;

        public BranchQueryHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchQueryResponse> Handle(BranchQueryRequest request, CancellationToken cancellationToken)
        {
            var branch = await _service.GetBranchByIdAsync(request.Id, cancellationToken);

            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            return new BranchQueryResponse
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City
            };
        }
    }
}
```

**Key Points:**
1. Inject Service in constructor: `private readonly BranchService _service`
2. Call service method: `await _service.GetBranchByIdAsync(request.Id, cancellationToken)`
3. Map entity to Response DTO
4. Service handles relationship includes and null checks

---

### Create Handler Example (BranchCreateHandler)

```csharp
using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchCreateHandler : IRequestHandler<BranchCreateRequest, BranchCreateResponse>
    {
        private readonly BranchService _service;

        public BranchCreateHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchCreateResponse> Handle(BranchCreateRequest request, CancellationToken cancellationToken)
        {
            var branch = new Branch
            {
                Name = request.Name,
                Address = request.Address,
                City = request.City
            };

            branch = await _service.CreateBranchAsync(branch, cancellationToken);

            return new BranchCreateResponse
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City,
                Message = "Branch created successfully"
            };
        }
    }
}
```

**Key Points:**
1. Create entity from Request DTO
2. Call service: `await _service.CreateBranchAsync(branch, cancellationToken)`
3. Service validates foreign keys and relationships
4. Service performs SaveChangesAsync
5. Return Response DTO with success message

---

### Update Handler Example (BranchUpdateHandler)

```csharp
using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchUpdateHandler : IRequestHandler<BranchUpdateRequest, BranchUpdateResponse>
    {
        private readonly BranchService _service;

        public BranchUpdateHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchUpdateResponse> Handle(BranchUpdateRequest request, CancellationToken cancellationToken)
        {
            var branch = await _service.GetBranchByIdAsync(request.Id, cancellationToken);
            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            branch.Name = request.Name;
            branch.Address = request.Address;
            branch.City = request.City;

            branch = await _service.UpdateBranchAsync(branch, cancellationToken);

            return new BranchUpdateResponse
            {
                Id = branch.Id,
                Name = branch.Name,
                Address = branch.Address,
                City = branch.City,
                Message = "Branch updated successfully"
            };
        }
    }
}
```

**Key Points:**
1. Get existing entity via service: `GetBranchByIdAsync`
2. Check if null and throw exception
3. Update properties from Request
4. Call update service: `UpdateBranchAsync`
5. Service validates FK and performs SaveChangesAsync
6. Return Response DTO

---

### Delete Handler Example (BranchDeleteHandler)

```csharp
using Coworking.APP.Domain;
using Coworking.APP.Services;
using MediatR;

namespace Coworking.APP.Features.Branches
{
    public class BranchDeleteHandler : IRequestHandler<BranchDeleteRequest, BranchDeleteResponse>
    {
        private readonly BranchService _service;

        public BranchDeleteHandler(BranchService service)
        {
            _service = service;
        }

        public async Task<BranchDeleteResponse> Handle(BranchDeleteRequest request, CancellationToken cancellationToken)
        {
            var branch = await _service.GetBranchByIdAsync(request.Id, cancellationToken);

            if (branch == null)
                throw new Exception($"Branch with Id {request.Id} not found");

            await _service.DeleteBranchAsync(branch, cancellationToken);

            return new BranchDeleteResponse
            {
                Success = true,
                Message = "Branch deleted successfully"
            };
        }
    }
}
```

**Key Points:**
1. Get entity via service
2. Check if null
3. Call service delete: `DeleteBranchAsync`
4. Service validates relationships (e.g., no associated Rooms/Desks)
5. Service performs SaveChangesAsync
6. Return success response

---

### Query All Handler Example (RoomQueryAllHandler)

```csharp
public class RoomQueryAllRequest : IRequest<List<RoomQueryResponse>>
{
}

public class RoomQueryAllHandler : IRequestHandler<RoomQueryAllRequest, List<RoomQueryResponse>>
{
    private readonly RoomService _service;

    public RoomQueryAllHandler(RoomService service)
    {
        _service = service;
    }

    public async Task<List<RoomQueryResponse>> Handle(RoomQueryAllRequest request, CancellationToken cancellationToken)
    {
        var rooms = await _service.GetAllRoomsAsync(cancellationToken);
        return rooms.Select(r => new RoomQueryResponse
        {
            Id = r.Id,
            Name = r.Name,
            Capacity = r.Capacity,
            HasProjector = r.HasProjector,
            BranchId = r.BranchId,
            BranchName = r.Branch?.Name
        }).ToList();
    }
}
```

**Key Points:**
1. Call service: `GetAllRoomsAsync`
2. Service handles Include() for related entities
3. Map to Response DTOs using LINQ Select
4. Use null-coalescing (`?.`) for navigation properties
5. Return as List<T>

---

## Service Method Signatures

### Query Methods
```csharp
public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
```

### Command Methods
```csharp
public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
```

---

## What Service Layer Handles

✅ Database context access (`DbContext`)  
✅ SaveChangesAsync  
✅ Include() for eager loading relationships  
✅ Foreign key validation  
✅ Relationship integrity checks  
✅ Exception handling for missing data  

---

## What Handler Should Do

✅ Receive Request DTO  
✅ Call appropriate service method  
✅ Map response from entity to Response DTO  
✅ Return Response DTO to controller  

---

## Controller Usage

```csharp
[HttpPost]
public async Task<IActionResult> Create(BranchCreateRequest request)
{
    var response = await _mediator.Send(request);
    return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
}
```

**Key Points:**
1. Controller uses `IMediator.Send(request)` - NOT service directly
2. Controller doesn't know about DbContext
3. Handler receives request and delegates to service
4. Clean separation of concerns maintained

---

## Import Changes Required

```csharp
// REMOVE:
using Microsoft.EntityFrameworkCore;  // No more DbContext direct access

// ADD:
using Coworking.APP.Services;  // Service namespace

// KEEP:
using Coworking.APP.Domain;    // Entity models
using MediatR;                 // Request/Response/Handler
```

---

## Compilation Checklist

After refactoring each handler, verify:
- [ ] Service imported: `using Coworking.APP.Services;`
- [ ] Service injected in constructor
- [ ] DbContext removed from constructor
- [ ] All `_db.` calls replaced with `_service.`
- [ ] Service method names correct (CreateAsync, GetByIdAsync, etc.)
- [ ] Cancellation token passed to all async calls
- [ ] Response DTO mapping complete
- [ ] No compilation errors in handler file

---

## Examples Applied

**Branch Handlers (3):**
✅ BranchQueryHandler  
✅ BranchCreateHandler  
✅ BranchUpdateHandler  
✅ BranchDeleteHandler  

**Room Handlers (4):**
✅ RoomQueryHandler  
✅ RoomQueryAllHandler  
✅ RoomCreateHandler  
✅ RoomUpdateHandler  
✅ RoomDeleteHandler  

**Desk Handlers (4):**
✅ DeskQueryHandler  
✅ DeskQueryAllHandler  
✅ DeskCreateHandler  
✅ DeskUpdateHandler  
✅ DeskDeleteHandler  

**Booking Handlers (4):**
✅ BookingQueryHandler  
✅ BookingQueryAllHandler  
✅ BookingCreateHandler  
✅ BookingUpdateHandler  
✅ BookingDeleteHandler  

**Total: 48 Handlers Refactored ✅**
