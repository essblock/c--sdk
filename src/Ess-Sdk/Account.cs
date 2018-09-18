using static Ess.Sdk.Preconditions;

namespace Ess.Sdk
{
    public class Account : ITransactionBuilderAccount
    {
        public KeyPair KeyPair
        {
            get;
            private set;
        }

        public long SequenceNumber
        {
            get;
            private set;
        }

        public Account(KeyPair keyPair, long sequenceNumber)
        {
            this.KeyPair = CheckNotNull(keyPair, "密钥对对象不能为空");
            this.SequenceNumber = sequenceNumber;
        }

        public void IncrementSequenceNumber()
        {
            SequenceNumber++;
        }
    }
}
