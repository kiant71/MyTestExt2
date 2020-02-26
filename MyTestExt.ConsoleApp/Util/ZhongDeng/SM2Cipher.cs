using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;

namespace MyTestExt.ConsoleApp.Util.ZhongDeng
{
    //public class SM2Cipher
    //{
    //    private int ct;
    //    private ECPoint p2;
    //    private SM3DigestExt sm3keybase;
    //    private SM3DigestExt sm3c3;
    //    private byte[] key;
    //    private byte keyOff;

    //    public SM2Cipher()
    //    {
    //        this.ct = 1;
    //        this.key = new byte[32];
    //        this.keyOff = 0;
    //    }


    //    private void reset()
    //    {
    //        this.sm3keybase = new SM3DigestExt();
    //        this.sm3c3 = new SM3DigestExt();

    //        byte[] p = ZhongDengUtil.byteConvert32Bytes(this.p2.Normalize().XCoord.ToBigInteger());
    //        this.sm3keybase.BlockUpdate(p, 0, p.Length);
    //        this.sm3c3.BlockUpdate(p, 0, p.Length);

    //        p = ZhongDengUtil.byteConvert32Bytes(this.p2.Normalize().YCoord.ToBigInteger());
    //        this.sm3keybase.BlockUpdate(p, 0, p.Length);
    //        this.ct = 1;
    //        nextKey();
    //    }


    //    private void nextKey()
    //    {
    //        SM3DigestExt sm3keycur = new SM3DigestExt(this.sm3keybase);
    //        sm3keycur.Update((byte)(this.ct >> 24 & 0xFF));
    //        sm3keycur.Update((byte)(this.ct >> 16 & 0xFF));
    //        sm3keycur.Update((byte)(this.ct >> 8 & 0xFF));
    //        sm3keycur.Update((byte)(this.ct & 0xFF));
    //        sm3keycur.DoFinal(this.key, 0);
    //        this.keyOff = 0;
    //        this.ct++;
    //    }


    //    public ECPoint Init_enc(SM2 sm2, ECPoint userKey)
    //    {
    //        AsymmetricCipherKeyPair key = sm2.ecc_key_pair_generator.GenerateKeyPair();
    //        ECPrivateKeyParameters ecpriv = (ECPrivateKeyParameters)key.Private;
    //        ECPublicKeyParameters ecpub = (ECPublicKeyParameters)key.Public;
    //        BigInteger k = ecpriv.D;
    //        ECPoint c1 = ecpub.Q;
    //        this.p2 = userKey.Multiply(k);
    //        reset();
    //        return c1;
    //    }


    //    public void Encrypt(byte[] data)
    //    {
    //        this.sm3c3.BlockUpdate(data, 0, data.Length);
    //        for (int i = 0; i < data.Length; i++)
    //        {

    //            if (this.keyOff == this.key.Length)
    //            {
    //                nextKey();
    //            }
    //            //this.keyOff = (byte)(this.keyOff + 1); data[i] = (byte)(data[i] ^ this.key[this.keyOff]);
    //            data[i] ^= key[keyOff++]; // note.
    //        }
    //    }


    //    public void Init_dec(BigInteger userD, ECPoint c1)
    //    {
    //        this.p2 = c1.Multiply(userD);
    //        reset();
    //    }


    //    public void Decrypt(byte[] data)
    //    {
    //        for (int i = 0; i < data.Length; i++)
    //        {

    //            if (this.keyOff == this.key.Length)
    //            {
    //                nextKey();
    //            }
    //            //this.keyOff = (byte)(this.keyOff + 1); data[i] = (byte)(data[i] ^ this.key[this.keyOff]);
    //            data[i] ^= key[keyOff++];  // note.
    //        }

    //        this.sm3c3.BlockUpdate(data, 0, data.Length);
    //    }


    //    public void Dofinal(byte[] c3)
    //    {
    //        byte[] p = ZhongDengUtil.byteConvert32Bytes(this.p2.Normalize().YCoord.ToBigInteger());
    //        this.sm3c3.BlockUpdate(p, 0, p.Length);
    //        this.sm3c3.DoFinal(c3, 0);
    //        reset();
    //    }
    //}
}
