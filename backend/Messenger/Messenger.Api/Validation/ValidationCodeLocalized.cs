using System.Globalization;
using FluentValidation;
using FluentValidation.Resources;
using Messenger.Core.Constants;

namespace Messenger.Api.Validation;

public class ValidationCodeLocalized : ILanguageManager
{
    public static void SetRepetValidationErrorCodes()
    {
        ValidatorOptions.Global.LanguageManager = new ValidationCodeLocalized();
    }

    public string GetString(string key, CultureInfo culture = null)
    {
        return ValidationErrorCodes.ResourceManager.GetString(key, culture) ?? key;
    }

    public bool Enabled { get; set; }
    public CultureInfo Culture { get; set; }
}
