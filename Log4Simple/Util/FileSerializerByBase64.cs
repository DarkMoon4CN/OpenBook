using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.IO.Compression;
using Log4Simple.Core;

namespace Log4Simple.Util
{
    /// <summary>
    /// write local file ,content by Base64
    /// </summary>
    public class FileSerializerByBase64
    {

        
        private const string SplitChar = "\r\n";

        #region 序列化对象

        /// <summary>
        /// serialize objs
        /// </summary>
        /// <param name="objs">objects</param>
        /// use default splitchar
        /// <returns></returns>
        public static string Serializer(object[] objs)
        {
            return Serializer(objs, SplitChar);
        }

        /// <summary>
        /// serialize objs
        /// </summary>
        /// <param name="objs">objects</param>
        /// <param name="splitChar">object string by split char </param>
        /// <returns></returns>
        public static string Serializer(object[] objs,string splitChar)
        {
            string ret = "";
            if (string.IsNullOrEmpty(splitChar))
                splitChar = SplitChar;

            foreach (var led in objs)
            {
                ret += Serializer(led) + splitChar;
            }

            return ret;
        }       

        /// <summary>
        /// serializer object
        /// </summary>
        /// <param name="obj">object 2 serializer</param>
        /// <returns></returns>
        public static string Serializer(object obj)
        {
            var formatter = new BinaryFormatter();
            byte[] buffer;
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, obj);
                buffer = ms.ToArray();
                ms.Close();
            }
            using (var ms = new MemoryStream())
            {
                using (GZipStream gzipstream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gzipstream.Write(buffer, 0, buffer.Length);
                    gzipstream.Close();
                }
                ms.Close();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        #endregion

        #region 反序列化对象
        /// <summary>
        /// deserialize objects
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="base64String">base64String</param>
        /// <returns></returns>
        public static T[] Deserializes<T>(string base64String)
        {

            string[] objstrings = base64String.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            T[] objs = new T[objstrings.Length];
            for (int i = 0; i < objstrings.Length; i++)
            {
                if (objstrings[i].Length > 0)
                    objs[i] = Deserialize<T>(Convert.FromBase64String(objstrings[i]));
                else
                    objs[i] = default(T);
            }

            return objs;
        }

        /// <summary>
        /// deserializer object
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="base64String">base64String</param>
        /// <returns></returns>
        public static T Deserialize<T>(string base64String)
        {
            return Deserialize<T>(Convert.FromBase64String(base64String), true);
        }

        /// <summary>
        /// return deserialize object
        /// </summary>
        /// <typeparam name="T">return object</typeparam>
        /// <param name="base64String">base64String</param>
        /// <param name="isAgreeError">when error to process: true = agree error,false = throw exceptoin</param>
        /// <returns></returns>
        public static T Deserialize<T>(string base64String, bool isAgreeError)
        {
            return Deserialize<T>(Convert.FromBase64String(base64String), isAgreeError);
        }

        /// <summary>
        /// return deserialize object
        /// </summary>
        /// <typeparam name="T">return object</typeparam>
        /// <param name="bytes">binary of object</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes)
        {
            return Deserialize<T>(bytes, false);
        }

        /// <summary>
        /// return deserialize object
        /// </summary>
        /// <typeparam name="T">return object</typeparam>
        /// <param name="bytes">binary of object</param>
        /// <param name="isAgreeError">when error to process: true = agree error,false = throw exceptoin</param>
        /// <returns></returns>
        public static T Deserialize<T>(byte[] bytes, bool isAgreeError)
        {

            T t = default(T);

            using (var ms = new MemoryStream(bytes))
            {

                ms.Position = 0;

                using (var gzipstream = new GZipStream(ms, CompressionMode.Decompress))
                {

                    byte[] buffer = new byte[4096];  

                    int offset = 0;

                    using (var mswrite = new MemoryStream())
                    {

                        while ((offset = gzipstream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            mswrite.Write(buffer, 0, offset);
                        }
                        BinaryFormatter sfFormatter = new BinaryFormatter();

                        mswrite.Position = 0;                

                        try
                        {
                            t = (T)sfFormatter.Deserialize(mswrite);
                        }
                        catch (Exception ex)
                        {
                            if (isAgreeError)
                                throw ex;
                        }
                        mswrite.Close();
                    }
                    gzipstream.Close();
                }
                ms.Close();
            }

            return t;
        }


        #endregion



      
    }
}
