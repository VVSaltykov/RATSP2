using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RATSP.Common.Definitions.Enums;

public enum Participation
{
    [Display(Name = "Да")]
    Yes,
    [Display(Name = "Нет")]
    No
}