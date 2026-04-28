namespace FlowerShopOnlineOrderSystem.Data
{
    public static class RoleNames
    {
        public const string Administrator = "Administrator";
        public const string Customer = "Customer";
        public const string Florist = "Florist";
        public const string Staff = Administrator + "," + Florist;
        public const string AllRoles = Administrator + "," + Customer + "," + Florist;
    }
}
