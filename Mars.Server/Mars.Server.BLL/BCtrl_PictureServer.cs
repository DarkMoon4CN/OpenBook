using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.Entity;
using Mars.Server.DAO;
using System.Data;

namespace Mars.Server.BLL
{
    public class BCtrl_PictureServer
    {
        PictureServerDAO dao = new PictureServerDAO();
        PictureDAO picobj = new PictureDAO();

        public DataTable QueryStartPic()
        {
            return picobj.QueryStartPic();
        }

        public PictureServerEntity QueryPicServer(int picServerID)
        {
            return dao.QueryPicServer(picServerID);
        }      

        public int AddPicInfoToDB(int imgserverid, string path)
        {
            return picobj.AddPicInfoToDB(imgserverid, path);
        }

        public List<PictureEntity> QueryImages(List<int> picids)
        {
            if (picids.Count > 0)
                return picobj.QueryImages(picids);
            else
                return new List<PictureEntity>();
        }

        public PictureEntity QueryPictureEntity(int pictureID)
        {
            return picobj.QueryPictureEntity(pictureID);
        }

        public int InsertPicture(PictureEntity entity)
        {
            return picobj.Insert(entity);
        }
    }
}
