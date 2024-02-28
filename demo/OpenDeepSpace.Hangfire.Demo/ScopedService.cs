namespace OpenDeepSpace.Hangfire.Demo
{
    public class ScopedService : IScopedService
    {

        public Guid Id { get; set; }

        public ScopedService()
        {
                Id=Guid.NewGuid();
        }

        public void op()
        {
            
        }
    }
}
