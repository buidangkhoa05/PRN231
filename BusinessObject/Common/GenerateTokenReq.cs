using BusinessObject.Common.Enums;

namespace BusinessObject.Common
{
    public class GenerateTokenReq
    {
        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Fullname { get; set; } = null!;
        public Role Role { get; set; }
        public int ExpireHours { get; set; }

    }
}