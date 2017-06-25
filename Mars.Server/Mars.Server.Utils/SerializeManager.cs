using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Mars.Server.Utils
{
    public class SerializeManager<T> where T :class,new()
    {
        public static bool Serialize(string targetfile,T data)
        {
            try
            {
                using (FileStream fs = new FileStream(targetfile, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(fs, data);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        public static T Deserialize(string targetfile)
        {
            try
            {
                using (FileStream fs = new FileStream(targetfile, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(fs) as T;
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }
    }
}
