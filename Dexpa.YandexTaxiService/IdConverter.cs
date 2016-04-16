namespace Dexpa.OrdersGateway
{
    static class IdConverter
    {
        public static long OuterIdToInner(string id)
        {
            return long.Parse(id);
        }

        public static string InnerIdToOuter(long id)
        {
            return id.ToString();
        }
    }
}
