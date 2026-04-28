namespace FlowerShopOnlineOrderSystem.Services
{
    public class EmailOptions
    {
        public const string SectionName = "EmailSettings";

        public string FromAddress { get; set; } = "orders@flowershop.local";

        public string FromName { get; set; } = "Flower Shop";

        public bool EnableSmtp { get; set; }

        public string? Host { get; set; }

        public int Port { get; set; } = 587;

        public bool EnableSsl { get; set; } = true;

        public string? UserName { get; set; }

        public string? Password { get; set; }
    }
}
