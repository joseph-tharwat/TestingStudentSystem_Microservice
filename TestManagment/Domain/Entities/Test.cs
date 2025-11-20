using System.Collections.ObjectModel;
using TestManagment.Domain.ValueObjects.Test;

namespace TestManagment.Domain.Entities
{
    public class Test
    {
        public int Id { get; private set; }
        public TestTitle Title { get; set; }
        public TestPublicationStatus PublicationStatus { get; private set; }
        public ICollection<TestQuestion> TestQuestions { get; private set; } = [];
        public Collection<TestsScheduling> Schedulings { get; set; }

        public Test()
        {
            this.PublicationStatus = new TestPublicationStatus();
        }
        public Test(TestTitle testTitle)
        {
            this.Title = testTitle;
            this.PublicationStatus = new TestPublicationStatus();
        }

        public void AddQuestion(int questionId)
        {
            if(PublicationStatus)
            {
                throw new InvalidOperationException("The test has been published, You can not add any questions now.");
            }

            if(TestQuestions.Any(q=>q.QuestionId == questionId))
            {
                throw new InvalidOperationException("The question is already added before");
            }
            TestQuestions.Add(new TestQuestion(Id, questionId));
        }

        public void RemoveQuestion(int questionId)
        {
            if (PublicationStatus)
            {
                throw new InvalidOperationException("The test has been published, You can not remove any questions now.");
            }

            var testQuestion = TestQuestions.FirstOrDefault(q => q.QuestionId == questionId);
            if (testQuestion == null)
            {
                throw new InvalidOperationException("The question is not in the test");
            }
            TestQuestions.Remove(testQuestion);
        }

        public void Publish()
        {
            if (PublicationStatus)
            {
                throw new InvalidOperationException("This test has been published before");
            }
            PublicationStatus = PublicationStatus.Publish();
        }

        public void UnPublish()
        {
            if (!PublicationStatus)
            {
                throw new InvalidOperationException("This test has not published before");
            }
            PublicationStatus.unPublish();
        }

    }
}
