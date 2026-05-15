using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Users.APP.Domain;
using Users.APP.Features.Groups;
using Users.APP.Features.Roles;

namespace Users.APP.Features.Users
{
    public class UserQueryRequest : Request, IRequest<IQueryable<UserQueryResponse>>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Genders? Gender { get; set; }
        public DateTime? BirthDateStart { get; set; }
        public DateTime? BirthDateEnd { get; set; }
        public decimal? ScoreStart { get; set; }
        public decimal? ScoreEnd { get; set; }
        public bool? IsActive { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? GroupId { get; set; }
        public List<int> RoleIds { get; set; } = new List<int>();
    }

    public class UserQueryResponse : Response
    {
        // entity properties
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Genders Gender { get; set; } // 1: Woman, 2: Man

        public DateTime? BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public decimal Score { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public int? CountryId { get; set; } 

        public int? CityId { get; set; } 

        public int? GroupId { get; set; }

        public List<int> RoleIds { get; set; }

        // custom properties
        public string FullName { get; set; }

        public string GenderF { get; set; } // "Woman", "Man"

        public string BirthDateF { get; set; } // "03/12/2026"

        public string RegistrationDateF { get; set; }

        public string ScoreF { get; set; }

        public string IsActiveF { get; set; } // "Active", "Inactive"

        public string GroupF { get; set; } // "CTIS", "Bilkent"

        public List<string> RolesF { get; set; } // "Admin", "User"

        public GroupQueryResponse Group { get; set; }

        public List<RoleQueryResponse> Roles { get; set; }
    }

    public class UserQueryHandler : Service<User>, IRequestHandler<UserQueryRequest, IQueryable<UserQueryResponse>>
    {
        public UserQueryHandler(DbContext db) : base(db)
        {
            // if the culture of the application is needed to be changed
            //CultureInfo = new CultureInfo("tr-TR");
        }

        // base query
        // select * from Users
        // overridden query
        // select * from Users inner join Groups on Users.GroupId = Groups.Id order by IsActive desc, RegistrationDate desc, UserName 
        protected override IQueryable<User> DbSet()
        {
            return base.DbSet().Include(userEntity => userEntity.Group)
                .Include(userEntity => userEntity.UserRoles).ThenInclude(userRoleEntity => userRoleEntity.Role)
                .OrderByDescending(userEntity => userEntity.IsActive).ThenByDescending(userEntity => userEntity.RegistrationDate).ThenBy(userEntity => userEntity.UserName);
        }

        public Task<IQueryable<UserQueryResponse>> Handle(UserQueryRequest request, CancellationToken cancellationToken)
        {
            var entityQuery = DbSet().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.UserName))
                entityQuery = entityQuery.Where(userEntity => userEntity.UserName == request.UserName.Trim());

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                entityQuery = entityQuery.Where(userEntity => userEntity.FirstName != null && userEntity.FirstName.Contains(request.FirstName.Trim()));

            if (!string.IsNullOrWhiteSpace(request.LastName))
                entityQuery = entityQuery.Where(userEntity => userEntity.LastName != null && userEntity.LastName.Contains(request.LastName.Trim()));

            if (request.Gender.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.Gender == request.Gender.Value);

            if (request.BirthDateStart.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.BirthDate.HasValue && userEntity.BirthDate.Value.Date >= request.BirthDateStart.Value.Date);

            if (request.BirthDateEnd.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.BirthDate.HasValue && userEntity.BirthDate.Value.Date <= request.BirthDateEnd.Value.Date);

            if (request.ScoreStart.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.Score >= request.ScoreStart.Value);

            if (request.ScoreEnd.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.Score <= request.ScoreEnd.Value);

            if (request.IsActive.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.IsActive == request.IsActive.Value);

            if (request.CountryId.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.CountryId == request.CountryId.Value);

            if (request.CityId.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.CityId == request.CityId.Value);

            if (request.GroupId.HasValue)
                entityQuery = entityQuery.Where(userEntity => userEntity.GroupId == request.GroupId.Value);

            if (request.RoleIds is not null && request.RoleIds.Any())
                entityQuery = entityQuery.Where(userEntity => userEntity.UserRoles.Any(userRoleEntity => request.RoleIds.Contains(userRoleEntity.RoleId)));

            var query = entityQuery.Select(userEntity => new UserQueryResponse
            {
                // entity data
                Address = userEntity.Address,
                BirthDate = userEntity.BirthDate,
                CityId = userEntity.CityId,
                CountryId = userEntity.CountryId,
                FirstName = userEntity.FirstName,
                Gender = userEntity.Gender,
                GroupId = userEntity.GroupId,
                Id = userEntity.Id,
                IsActive = userEntity.IsActive,
                LastName = userEntity.LastName,
                Password = userEntity.Password,
                RegistrationDate = userEntity.RegistrationDate,
                RoleIds = userEntity.UserRoles.Select(userRoleEntity => userRoleEntity.RoleId).ToList(),
                Score = userEntity.Score,
                UserName = userEntity.UserName,
                
                // custom data
                FullName = userEntity.FirstName + " " + userEntity.LastName,
                IsActiveF = userEntity.IsActive ? "Active" : "Inactive",
                ScoreF = userEntity.Score.ToString("N1"), // 3.33333333 -> 3.3
                GenderF = userEntity.Gender.ToString(), // "Man", "Woman"
                RegistrationDateF = userEntity.RegistrationDate.ToString("MM/dd/yyyy HH:mm:ss"),
                BirthDateF = userEntity.BirthDate.HasValue ? userEntity.BirthDate.Value.ToString("MM/dd/yyyy") : string.Empty,
                
                // Way 1:
                GroupF = userEntity.Group != null ? userEntity.Group.Title : null,
                // Way 2:
                Group = userEntity.Group == null ? null : new GroupQueryResponse
                {
                    Id = userEntity.Group.Id,
                    Title = userEntity.Group.Title
                },

                // Way 1:
                RolesF = userEntity.UserRoles.Select(userRoleEntity => userRoleEntity.Role.Name).ToList(),
                // Way 2:
                Roles = userEntity.UserRoles.Select(userRoleEntity => new RoleQueryResponse
                {
                    Id = userRoleEntity.Role.Id,
                    Name = userRoleEntity.Role.Name,
                }).ToList()
            });

            return Task.FromResult(query);
        }
    }
}
