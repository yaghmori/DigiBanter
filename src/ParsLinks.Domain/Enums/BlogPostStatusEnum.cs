using ParsLinks.Localization;
using System.ComponentModel.DataAnnotations;

namespace ParsLinks.Domain.Enums;

public enum BlogPostStatusEnum : int
{
    [Display(Description = nameof(EnumResources.BlogPostStatusEnum_Archived), ShortName = "fa-file-lock", Prompt = "#ff0000", ResourceType = typeof(EnumResources))]
    Archived = 0,
    [Display(Description = nameof(EnumResources.BlogPostStatusEnum_Draft), ShortName = "fa-file-pen", Prompt = "#ddc800", ResourceType = typeof(EnumResources))]
    Draft = 1,
    [Display(Description = nameof(EnumResources.BlogPostStatusEnum_Published), ShortName = "fa-file-circle-check", Prompt = "#dedede", ResourceType = typeof(EnumResources))]
    Published = 2,


}

// Draft, Published, Archived