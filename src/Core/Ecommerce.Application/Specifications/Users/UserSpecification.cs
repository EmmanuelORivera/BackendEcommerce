using Ecommerce.Domain;

namespace Ecommerce.Application.Specifications.Users;

public class UserSpecification : BaseSpecification<User>
{
    public UserSpecification(UserSpecificationParams userParams) : base(
        x =>
        (
        string.IsNullOrEmpty(userParams.Search)
        || x.Name!.Contains(userParams.Search)
        || x.LastName!.Contains(userParams.Search)
        || x.Email!.Contains(userParams.Search)
        )
    )
    {
        ApplyPaging(userParams.PageSize * (userParams.PageIndex - 1), userParams.PageSize);

        if (!string.IsNullOrEmpty(userParams.Sort))
        {
            switch (userParams.Sort)
            {
                case "nameASC":
                    AddOrderBy(p => p.Name!);
                    break;
                case "nameDESC":
                    AddOrderByDecending(p => p.Name!);
                    break;
                case "lastNameASC":
                    AddOrderBy(p => p.LastName!);
                    break;
                case "lastNameDESC":
                    AddOrderByDecending(p => p.LastName!);
                    break;

                default:
                    AddOrderBy(p => p.Name!);
                    break;
            }
        }
        else
        {
            AddOrderByDecending(p => p.Name!);
        }


    }
}