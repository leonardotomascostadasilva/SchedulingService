namespace ServiceSchedule.Infrastructure.Entities
{
    public class Message : Entity
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Data { get; set; }
        public string Topic { get; set; }
    }
}
