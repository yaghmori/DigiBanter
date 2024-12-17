using System.ComponentModel.DataAnnotations;

namespace ParsLinks.Domain.Enums;

public enum FileTypeEnum
{
    [Display(Description = "None", Order = 0, AutoGenerateField = false)]
    None,
    [Display(Description = "other", ShortName = "file")]
    other,
    [Display(Description = "Image", ShortName = "file-image")]
    Image,
    [Display(Description = "PDF", ShortName = "file-pdf")]
    PDF,
    [Display(Description = "Text", ShortName = "file-lines")]
    Text,
    [Display(Description = "Document", ShortName = "file-doc")]
    Document,
    [Display(Description = "Sheet", ShortName = "file-spreadsheet")]
    Sheet,
    [Display(Description = "Zip", ShortName = "file-zip")]
    Zip,
    [Display(Description = "Audio", ShortName = "file-music")]
    Audio,
    [Display(Description = "Video", ShortName = "file-video")]
    Video,
}
