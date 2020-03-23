using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyTestExt.ConsoleApp
{
    public class SHA1withRSA
    {
        public void Do()
        {
            var signBaseStr = "developerId=1583822860011324861rtick=15845147546081signType=rsa/openapi/v2/credentialVerify/enterprise/identity4/74d1d436eb5ea6a5b97de77142c6a9cb";
            var _privateKey = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAL2qUuz5n2FYH0YJ6eLg/klf90b1UGdpdoPEMpFd4Ds4GFD1Bobs8TnCZyjw1kmI8510eNv4vEdMjssvxztQl4P0Bx8nS7HaiDhGPhRkGjkyO8nhi0raBvqiuaXcT1ziGNIi8Fyb42r7AJWY4eLB00dFLUhYM5cT3MuFEEsFF+2lAgMBAAECgYBm7ELL7jobLSLrcv3E7KuRTc52ZzEWrRhvMMzwLa089Zfcdtrv5SySV3h7sxXWGcd5rnbXiAdD+buTb34CJh3RwOuRK1cTugNcBF94tgxahdVFXGYeBztGMAbLgqgikqKUrb3Uu1UB6xZNavYoZs7s7hXTSErnWQ2fy43sahvZEQJBAPSK89KK052KO4KxU0b55B5YGR9yEjF75cBXkVcY5jFUNohYDiL23gudPLOzU4nNO9fVtZxuAcRZQm4GRa/mt48CQQDGjSvFVTeauG/wPZC69naFBOxvNO6BmwXUnOhtdgBUpn7ANaSzEt1/aO3yxnoMhaEHc7kZelIM0mTUt1O8gA2LAkAgB7M3+IcPM3PoAmHuAf0nHFLuE3rekGPfdZjL168O9wDNivsPCVa8HrQ8tVhTzFXyR0OqYZ5JoYdZ8eheydFxAkAd3zdqpPuMp5sPxfN0bYg/UxJPWONZVH14E3NUpKBAHByNxoRU/M2eUacbE4lhQOmNOXfrwV9+1lidOEe84HcpAkEA6sMaXebz252lWzUwMwV+M6unNfjlNGpSV33zHLxyO6kYY/Nm8X43QAkIl0EBxa8eraWMR5Xpmdhm5tpOQzZHPA==";

            //var signBaseStr = "developerId=aaaaaartick=14840209310001signType=rsa/openapi/v2/user/reg/fb5939fd51b1a929a6e2b75b187827f7";
            //var _privateKey = "MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDhujrkPM9Wdy306+7K+k+vdjEcrvnbjztW8z0KE47NkcWwpeN0NxwsRx11s/Zdg6vp3K6Tj+/Kb5CA+V1+PrHXhZYSPXDZ/GPdUUYyzmiAH4yeY2lnSN+SFXWw5qLdMbE2U7EJ3IK2rOzdhetelOktEi4XLJrLowWDfUp9SXHU3BxbeAqWpg9KtHCBnapyvtlblYBSbGDP0D+cbuoHQR0fpDdrxuZNcqyOW0Vo4u+N35YYVnsXATCGaBRM0y2olW4mcBcKhSPFNPgGxhwVUpd8mENM8gk+JrAKDeMiib69qlKffukmSRwM4+R0XiRRF+q3JbWslEaZiYrdjejymfQ/AgMBAAECggEBAJuLIJm17Ym/hkOHcH0eD1rxLtZ9HF9XS6SW6DDRWU3+bSNUrG+gqiE4v+wjnljCKuxzzTiRrsLoLcMyEmK0AQhqXQckn2cWrBoNrznDUe83zkY2aqNpa+XWM8s6om+ZlmvBL/WDpe8LKg1cAtyh8CvGo0wEyIYaR+pbo8DBo1bxKlISWKxK6nYtw43V/3gj5TPtS/56+J5cV0czLPfiP8BLHJC+vVAdRwxEo515fs+3EGSt9HKYujLdVf/qlSBlW0ueagBcumXuMAPvoNKc3hQ9U5MSo5E3G4v3ZLmLFkYK2u4eLi0MKLcstF+mnSa5K/XPa6qpR3FlRvFFfpo0VIECgYEA+89BfBxAqJHEfL6syKWHH/9UjrfrTc2OjNkkl3xULunWD/W2e7CqU1bCAiiphvkNlA4Ap+8mTsH7eQv+fml9jn1C4Je8+SQkC6IhO1ejlJdHNGcHSaSS8qrPziz0LjTVqJulZyrL0a3CPRydEEbcG/dzyUGSBopd2Z6tuPkO2bkCgYEA5XvcVxF7RFdMOrawDAhs7zytDuxpc7uHMAth+K/veOArI6FDiWlXV2T5Fm7n2Fyk1sFgbiiNIAMlT8LkEy5BHwJTI3/CWJlO1m6IR875m6C6c8yfNAlZcCtM3KzA3++KsWbL+Bm5e4MQLIK6nINILEQY8zVsLPOpchTbSlMjWbcCgYADbWK6ybenk7RU7lNzt60LJnKELsSpHm8eQ6ZX2X1b1hrvxKxjKphm4ftqBBuqlqE0rqwbnQmscL93ek5QcicfVV0fqXENAwzqi55gLGUxlI2HjQ9wVSka3HBPohUAu/9ceCHcMZzskazfIBCTNCOyzb9psdbG+AMm/x3mMp2dIQKBgCf06AmU9qSQmqpCcua1dNo281781OOczE4WPUnCwGyIg5phktPTLqz93M0GJIIsp1rpMrQbhW8EfTOlGHcqf5Y5nmY48YrhrrNxbXMrW5S4aJ6PC01RL2xzbZ+iyLZ1C+4VmwAo4n3Z9S/61yk3RpLGjJ2UOLCfAGrnrMMFNMuDAoGBAKGHWNn3+F5uUYFRLzVwrSYV/0SeAS7cRwSeYPhPdp4cl1XgNnmZ5cBq3E3nsdzAADnivEDoBqgwgABHatSey+WcQA+jjvPBJ/Jcs01PtzVJtAzlYHV+2KoVOQHkeDQ+fwsWhoXDj6U0/iosozqbFY0bxHNTikwmyk86uv13JP/n";
            var signRsaStr = Convert.ToBase64String(RsaSign(Encoding.UTF8.GetBytes(signBaseStr), _privateKey));
        }

                #region SHA1withRSA

        public byte[] RsaSign(byte[] data, string privateKey)
        {
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            return rsa.SignData(data, sh);
        }

        private static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
        {
            byte[] pkcs8privatekey;
            pkcs8privatekey = Convert.FromBase64String(pemstr);
            if (pkcs8privatekey != null)
            {
                RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
                return rsa;
            }
            else
                return null;
        }

        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading    
            byte bt = 0;
            ushort twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)    //data read as little endian order (actual data order for Sequence is 30 81)    
                    binr.ReadByte();    //advance 1 byte    
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes    
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);        //read the Sequence OID    
                if (!CompareBytearrays(seq, SeqOID))    //make sure Sequence for OID is correct    
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)    //expect an Octet string    
                    return null;

                bt = binr.ReadByte();        //read next byte, or next 2 bytes is  0x81 or 0x82; otherwise bt is the byte count    
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();
                //------ at this stage, the remaining sequence should be the RSA private key    

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }

        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }


        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // ---------  Set up stream to decode the asn.1 encoded RSA private key  ------    
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);    //wrap Memory Stream with BinaryReader for easy reading    
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)    //data read as little endian order (actual data order for Sequence is 30 81)    
                    binr.ReadByte();    //advance 1 byte    
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes    
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)    //version number    
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;


                //------  all private key components are Integer sequences ----    
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                // ------- create RSACryptoServiceProvider instance and initialize with public key -----    
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)        //expect integer    
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();    // data size in next byte    
            else
            if (bt == 0x82)
            {
                highbyte = binr.ReadByte();    // data size in next 2 bytes    
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;        // we already have the data size    
            }



            while (binr.ReadByte() == 0x00)
            {    //remove high order zeros in data    
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);        //last ReadByte wasn't a removed zero, so back up a byte    
            return count;
        }

        #endregion

    }
}
