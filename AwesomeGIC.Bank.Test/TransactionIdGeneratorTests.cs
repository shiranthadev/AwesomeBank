using AwesomeGIC.Bank.Service.Handlers;

namespace AwesomeGIC.Bank.Test
{
    public class TransactionIdGeneratorTests
    {
        [Theory]
        [InlineData(2024,1,23, 1, "20240123-02")]
        [InlineData(2024,1,23, 9, "20240123-10")]
        [InlineData(2024,1,23, 63, "20240123-64")]
        [InlineData(2024,1, 23, 99, "20240123-100")]
        public void WhenSequencePassed_SequenceId_ShouldBeGenerated(int year, int month, int day, int count, string expectedSequenceId)
        {
            var sequenceId = TransactionIdGenerator.GetNextTxnId(new DateOnly(year, month, day), count);
            Assert.Equal(expectedSequenceId, sequenceId);
        }
    }
}
