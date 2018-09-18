using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Ess.Sdk.Preconditions;

namespace Ess.Sdk
{
    public class ChangeTrustOperation : Operation
    {
        public Asset Asset { get; private set; }
        public long Limit { get; private set; }

        private ChangeTrustOperation(Asset asset, long limit)
        {
            Asset = CheckNotNull(asset, "资产不能为空");
            Limit = CheckNotNull(limit, "资产上限不能为空");
        }

        public static new ChangeTrustOperation FromXDR(Generated.Operation xdr)
        {
            return (ChangeTrustOperation)Operation.FromXDR(xdr);
        }

        public override Generated.Operation.OperationBody ToOperationBody()
        {
            var op = new Generated.ChangeTrustOp
            {
                Line = Asset.ToXDR(),
                Limit = new Generated.Int64(Limit)
            };

            var body = new Generated.Operation.OperationBody
            {
                ChangeTrustOp = op,
                Discriminant = Generated.OperationType.Create(Generated.OperationType.OperationTypeEnum.CHANGE_TRUST)
            };

            return body;
        }

        public class Builder
        {
            public KeyPair SourceAccount { get; private set; }
            public Asset Asset { get; private set; }
            public long Limit { get; private set; }

            public Builder(Generated.ChangeTrustOp op)
            {
                Asset = Ess.Sdk.Asset.FromXDR(op.Line);
                Limit = op.Limit.InnerValue;
            }

            public Builder(Asset asset, long limit)
            {
                Asset = CheckNotNull(asset, "资产不能为空");
                if(limit < 0)
                {
                    throw new ArgumentException("资产上限的值必须是非负的");
                }
                Limit = CheckNotNull(limit, "资产上限不能为空");
            }

            public Builder SetSourceAccount(KeyPair sourceAccount)
            {
                SourceAccount = CheckNotNull(sourceAccount, "源账号不能为空");
                return this;
            }

            public ChangeTrustOperation Build()
            {
                ChangeTrustOperation operation =
                    new ChangeTrustOperation(Asset, Limit);
                if (SourceAccount != null)
                {
                    operation.SourceAccount = SourceAccount;
                }
                return operation;
            }
        }
    }
}
