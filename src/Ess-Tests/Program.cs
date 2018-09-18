using Ess.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Examples
{
    using UrlContent = KeyValuePair<string, string>;
    class Program
    {
        //调用地址
        static string EssApi_url = "https://test-api.essblock.com:8000/";
        //网络密码
        static string network_passphrase = "Public Global EnterpriseSourceSystem Network ; September 2015";
        //量子单位
        static long Qunit = 10000000;

        static void Main(string[] args)
        {
            Ess.Sdk.Network.CurrentNetwork = network_passphrase;

            // seed from account on the testnetwork
            var myKeyPair = KeyPair.FromSeed("SDWFJYVBBUV3T2KNGLOCOWIOZ5GJTYLCWCPCKEB3MTLWGYGOMU3F3NGO");
           Account myAccount = new Account(myKeyPair, GetSequence(myKeyPair.Address));
            //创建随机账户
            var randomAccountKeyPair = CreateRandomAccount(myAccount, 10000 * Qunit);
            //创建指定账户
           // var AccountKeyPair = CreateAccount(myAccount, 100 * Qunit, "GB6E6EJGF3P2INHUY73G37KER6FBKLE2CMTMTBPEKEMW52K76A7ZHWQG");

            //向指定账户发送资产
            //   var dest = KeyPair.FromAccountId("GB6E6EJGF3P2INHUY73G37KER6FBKLE2CMTMTBPEKEMW52K76A7ZHWQG");
            //  Payment(myKeyPair, dest,  100* Ess.Sdk.One.Value);

       
            Console.Read();
        }

        static string GetResult(string msg)
        {
            using (var client = new HttpClient())
            {
                string response = client.GetStringAsync(EssApi_url + WebUtility.UrlEncode(msg)).Result;
                return response;
            }
        }

        static HttpResponseMessage PostResult(string tx)
        {
            using (var client = new HttpClient())
            {
                var body = new List<UrlContent>();
                body.Add(new UrlContent("tx", tx));
                var formUrlEncodedContent = new FormUrlEncodedContent(body);
                return client.PostAsync(EssApi_url + "transactions", formUrlEncodedContent).Result;
            }
        }

        private static long GetSequence(string address)
        {
            using (var client = new HttpClient())
            {
                string response = client.GetStringAsync(EssApi_url + "accounts/" + address).Result;
                var json = JObject.Parse(response);
                return (long)json["sequence"];
            }
        }

        //根据公钥地址创建已定义用户
        static KeyPair CreateAccount(Account source, long nativeAmount,string EssAccountID)
        {
            var dest = KeyPair.FromAccountId(EssAccountID) ;

            var operation =
                new CreateAccountOperation.Builder(dest, nativeAmount)
                .SetSourceAccount(source.KeyPair)
                .Build();

            source.IncrementSequenceNumber();

            Ess.Sdk.Transaction transaction =
                new Ess.Sdk.Transaction.Builder(source)
                .AddOperation(operation)
                .Build();

            transaction.Sign(source.KeyPair);

            var tx = transaction.ToEnvelopeXdrBase64();

            var response = PostResult(tx);

            Console.WriteLine("response:" + response.ReasonPhrase);
            Console.WriteLine(dest.Address);
            //    Console.WriteLine(dest.Seed);
          //  Console.WriteLine();

            return dest;
        }

        private static void DecodeTransactionResult(string result)
        {
            var bytes = Convert.FromBase64String(result);
            var reader = new Ess.Generated.ByteReader(bytes);
            var txResult = Ess.Generated.TransactionResult.Decode(reader);

        }

        private static void DecodeTxFee(string result)
        {
            var bytes = Convert.FromBase64String(result);
            var reader = new Ess.Generated.ByteReader(bytes);
            var txResult = Ess.Generated.LedgerEntryChanges.Decode(reader);

        }

        static void Payment(KeyPair from, KeyPair to, long amount)
        {
            Account source = new Account(from, GetSequence(from.Address));

            // load asset
            Asset asset = new Asset();

            var operation =
                new PaymentOperation.Builder(to, asset, amount)
                .SetSourceAccount(from)
                .Build();

            source.IncrementSequenceNumber();

            Ess.Sdk.Transaction transaction =
                new Ess.Sdk.Transaction.Builder(source)
                .AddOperation(operation)
                .Build();

            transaction.Sign(source.KeyPair);

            var tx = transaction.ToEnvelopeXdrBase64();

            var response = PostResult(tx);

            Console.WriteLine(response.ReasonPhrase);
        }

        //创建随机账户
        static KeyPair CreateRandomAccount(Account source, long nativeAmount)
        {
            var dest = KeyPair.Random();

            var operation =
                new CreateAccountOperation.Builder(dest, nativeAmount)
                .SetSourceAccount(source.KeyPair)
                .Build();

            source.IncrementSequenceNumber();

            Transaction transaction =
                new Transaction.Builder(source)
                .AddOperation(operation)
                .Build();

            transaction.Sign(source.KeyPair);

            var tx = transaction.ToEnvelopeXdrBase64();

            var response = PostResult(tx);

            Console.WriteLine("response:" + response.ReasonPhrase);
            Console.WriteLine(dest.Address);
            Console.WriteLine(dest.Seed);
            Random rd = new Random();
            System.IO.StreamWriter sw = new System.IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "\\syslog" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + rd.Next(99999) + ".txt");
            sw.WriteLine("公钥地址:" + dest.Address);
            sw.WriteLine("私钥密码:" + dest.Seed);
            sw.Close();
            return dest;
        }

        //创建随机钥对--本地生成，如需要在ESS生成账户，请参考CreateAccount方法
        static KeyPair CreatRandomKeyPair()
        {
            var dest = KeyPair.Random();
            Random rd = new Random();
            //钥对保存到本地
            System.IO.StreamWriter sw = new System.IO.StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "\\syslog" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + rd.Next(99999) + ".txt");
            sw.WriteLine("公钥地址:"+dest.Address);
            sw.WriteLine("私钥密码:" + dest.Seed);
            sw.Close();
                return dest;
        }
    }
}