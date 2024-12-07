
using Newtonsoft.Json;

namespace DigiBanter.Shared.Dto.Response
{
    public class UserResponse
    {

        public string? Id { get; set; }
        public string? PublicId { get; set; }
        public string? Email { get; set; } = default!;
        public string? PhoneNumber { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public int? ClaimsCount { get; set; }
        public int? RoleCount { get; set; }
        public int? SessionsCount { get; set; }
        public bool IsOnline { get; set; } = false;
        public int PointBalance { get; set; }
        public string? ImageBase64 { get; set; }




        [JsonIgnore]
        public string? FullName => string.Join(" ", new[] { FirstName, MiddleName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x)));

        [JsonIgnore]
        public string? DisplayName => FullName ?? Email;


    }
}
