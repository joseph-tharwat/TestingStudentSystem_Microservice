namespace TestManagment.Domain.Entities
{
    public class TestsScheduling
    {
        public int TestId { get; set; }
        public DateTime DateTime { get; set; }
        public Test test { get; set; }

        public TestsScheduling(int testId, DateTime time)
        {
            this.TestId = testId;   
            this.DateTime = time;
        }
        public TestsScheduling()
        {
            
        }
    }
}
