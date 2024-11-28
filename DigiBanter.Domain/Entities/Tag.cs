namespace DigiBanter.Domain.Entities;

public class Tag:BaseEntity<int>
{
    public ICollection<TagTranslation> Translations { get; set; } = new List<TagTranslation>();
}
