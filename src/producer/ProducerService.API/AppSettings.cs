namespace ProducerService.API;
public class EventBusSettings
{
    public string SubscriptionClientName { get; set; } = null!;
    public int EventBusRetryCount { get; set; }
    public string EventBusUserName { get; set; } = null!;
    public string EventBusPassword { get; set; } = null!;
    public string EventBusConnection { get; set; } = null!;
}

