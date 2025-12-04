using TestManagment.Domain.DomainErrors;
using TestManagment.Shared.Result;

namespace TestManagment.Domain.ValueObjects.Test
{
    public class TestTitle
    {
        public string Value { get; }
        private TestTitle(string title)
        {
            this.Value = title;
        }
        public static TestTitle GetObject(string title)
        {
            return new TestTitle(title);
        }
        public static Result<TestTitle> Create(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return Result<TestTitle>.Failure(TestErrors.EmptyTestTitle);
            }
            return Result<TestTitle>.Success(new TestTitle(title), null);
        }
        public static implicit operator TestTitle(string title) => new TestTitle(title);
        public static implicit operator string(TestTitle self) => self.Value;
    }
}
