namespace BlueBox.Delivery.Orders.Domain.Domain
{
    public enum OrderStatus : int
    {
        NEW = 1,
        SENT = 2,
        NOTIFIED = 3,
        RECEIVED = 4
    }
}
