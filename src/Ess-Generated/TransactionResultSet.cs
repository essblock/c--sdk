          // Automatically generated by xdrgen 
          // DO NOT EDIT or your changes may be overwritten

          namespace Ess.Generated
{


// === xdr source ============================================================
//  struct TransactionResultSet
//  {
//      TransactionResultPair results<>;
//  };
//  ===========================================================================
public class TransactionResultSet {
  public TransactionResultSet () {}
  public TransactionResultPair[] Results { get; set; }
  public static void Encode(IByteWriter stream, TransactionResultSet encodedTransactionResultSet) {
    int resultssize = encodedTransactionResultSet.Results.Length;
    XdrEncoding.EncodeInt32(resultssize, stream);
    for (int i = 0; i < resultssize; i++) {
      TransactionResultPair.Encode(stream, encodedTransactionResultSet.Results[i]);
    }
  }
  public static TransactionResultSet Decode(IByteReader stream) {
    TransactionResultSet decodedTransactionResultSet = new TransactionResultSet();
    int resultssize = XdrEncoding.DecodeInt32(stream);
    decodedTransactionResultSet.Results = new TransactionResultPair[resultssize];
    for (int i = 0; i < resultssize; i++) {
      decodedTransactionResultSet.Results[i] = TransactionResultPair.Decode(stream);
    }
    return decodedTransactionResultSet;
  }
}
}
