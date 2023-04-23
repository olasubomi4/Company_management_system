using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ObaGroupModel.Validation;

public class OneOfEndDateAndAllDay : ValidationAttribute
{
    private readonly string field1Name;
    private readonly string field2Name;

    public OneOfEndDateAndAllDay(string field1Name, string field2Name)
    {
        this.field1Name = field1Name;
        this.field2Name = field2Name;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var field1 = validationContext.ObjectType.GetProperty(field1Name)?.GetValue(validationContext.ObjectInstance, null);
        var field2 = validationContext.ObjectType.GetProperty(field2Name)?.GetValue(validationContext.ObjectInstance, null);

        if ((field1 == null && field2 == null) || (field1 != null && field2.Equals(true)))
        {
            return new ValidationResult($"Either {field1Name} or {field2Name} is required.");
        }
        
        return ValidationResult.Success;
    }
}