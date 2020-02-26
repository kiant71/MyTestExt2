using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng
{
    //public class SM2
    //{
    //    private static String[] ecc_param =
    //    {
    //        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFF",
    //        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF00000000FFFFFFFFFFFFFFFC",
    //        "28E9FA9E9D9F5E344D5A9E4BCF6509A7F39789F515AB8F92DDBCBD414D940E93",
    //        "FFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFF7203DF6B21C6052B53BBF40939D54123",
    //        "32C4AE2C1F1981195F9904466A39C9948FE30BBFF2660BE1715A4589334C74C7",
    //        "BC3736A2F4F6779C59BDCEE36B692153D0A9877CC62A474002DF32E52139F0A0"
    //    };

    //    private readonly BigInteger ecc_p;

    //    public static SM2 Instance()
    //    {
    //        return new SM2();
    //    }


    //    private BigInteger ecc_a;

    //    private readonly BigInteger ecc_b;
    //    private readonly BigInteger ecc_n;
    //    private readonly BigInteger ecc_gx;
    //    private readonly BigInteger ecc_gy;
    //    public readonly ECCurve ecc_curve;
    //    private readonly ECPoint ecc_point_g;
    //    private readonly ECDomainParameters ecc_bc_spec;
    //    public readonly ECKeyPairGenerator ecc_key_pair_generator;
    //    private readonly ECFieldElement ecc_gx_fieldelement;
    //    private readonly ECFieldElement ecc_gy_fieldelement;

    //    private SM2()
    //    {
    //        this.ecc_p = new BigInteger(ecc_param[0], 16);
    //        this.ecc_a = new BigInteger(ecc_param[1], 16);
    //        this.ecc_b = new BigInteger(ecc_param[2], 16);
    //        this.ecc_n = new BigInteger(ecc_param[3], 16);
    //        this.ecc_gx = new BigInteger(ecc_param[4], 16);
    //        this.ecc_gy = new BigInteger(ecc_param[5], 16);

    //        //this.ecc_gx_fieldelement = new ECFieldElement.Fp(this.ecc_p, this.ecc_gx);
    //        //this.ecc_gy_fieldelement = new ECFieldElement.Fp(this.ecc_p, this.ecc_gy);
    //        //this.ecc_curve = new ECCurve.Fp(this.ecc_p, this.ecc_a, this.ecc_b);
    //        //this.ecc_point_g = new ECPoint.Fp(this.ecc_curve,

    //        ecc_curve = new FpCurve(ecc_p, ecc_a, ecc_b, null, null);
    //        ecc_point_g = ecc_curve.CreatePoint(ecc_gx, ecc_gy);

    //        this.ecc_bc_spec = new ECDomainParameters(this.ecc_curve,
    //            this.ecc_point_g, this.ecc_n);


    //        ECKeyGenerationParameters ecc_ecgenparam = new ECKeyGenerationParameters(this.ecc_bc_spec,
    //            new SecureRandom());

    //        this.ecc_key_pair_generator = new ECKeyPairGenerator();
    //        this.ecc_key_pair_generator.Init(ecc_ecgenparam);
    //    }
    //}
}
