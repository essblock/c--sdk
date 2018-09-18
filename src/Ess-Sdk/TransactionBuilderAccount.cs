namespace Ess.Sdk
{
    public interface ITransactionBuilderAccount
    {
        KeyPair KeyPair { get; }

        long SequenceNumber { get; }

        void IncrementSequenceNumber();
    }
}
