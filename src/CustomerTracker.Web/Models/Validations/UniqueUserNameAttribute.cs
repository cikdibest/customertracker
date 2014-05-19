using System.ComponentModel.DataAnnotations;
using System.Linq;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Models.Validations
{
    public class UniqueUserNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string userName = value.ToString();

            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            bool isExistUserName = repositoryUser.Filter(q => q.Username == userName).Any();

            if (isExistUserName)
            {
                return new ValidationResult("Kullanıcı adı sistemde kayıtlı");
            }

            return ValidationResult.Success;

        }
    }
}