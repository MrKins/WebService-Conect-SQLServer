using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
namespace Android_SQL_WebService
{/// <summary>
    /// 一个操作数据库的类，所有对SQLServer的操作都写在这个类中，使用的时候实例化一个然后直接调用就可以
    /// </summary>
    public class DBOperation : IDisposable
    {
        public static SqlConnection sqlCon;  //用于连接数据库

        //将下面的引号之间的内容换成上面记录下的属性中的连接字符串
        private String ConServerStr = @"Data Source=KINS;Initial Catalog=mms;Integrated Security=True";

        //默认构造函数
        public DBOperation()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection();
                sqlCon.ConnectionString = ConServerStr;
                sqlCon.Open();
            }
        }

        //关闭/销毁函数，相当于Close()
        public void Dispose()
        {
            if (sqlCon != null)
            {
                sqlCon.Close();
                sqlCon = null;
            }
        }

        /// <summary>
        /// 增加一条录单信息
        /// </summary>
        public bool insertActDaily(string fActType, string fAddFlag, string fWorkerCardNr, string fMachineCardNr, string fMSAutoNr_)
        {
            try
            {
                string sql = "insert into mms.dbo.mkActDaily (fId_,fActType,fAddFlag,fInputTime,fWorkerCardNr,fMachineCardNr,fMSAutoNr_,fInputType)values(NEWID(),'" + fActType + "','" + fAddFlag + "',GETDATE(),'" + fWorkerCardNr + "','" + fMachineCardNr + "','" + fMSAutoNr_ + "','1')";
                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                insertPost(fMSAutoNr_);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// 写入Post表，方便过账程序运行
        /// </summary>
        public bool insertPost(string fMSAutoNr_)
        {
            try
            {
                string sql = "Insert Into mkActPost (fId_,AddTime,fMSNr,fisPost) Select newID(),GETDATE(), y.fNr   ,0 From (Select top 1 x.fnr from (Select a.fNr from mkSchBase a Inner Join mkSchProduct b On a.fNr=b.fNr Where CONVERT(varchar(50),b.fAutoNr_) ='" + fMSAutoNr_ + "' Union All Select a.fNr from mkSchBase a Inner Join mkSchProduct b On a.fNr=b.fNr Inner Join mkSchDetail  c On b.fId_=c.fMSId_ Where c.fAutoNr_ ='" + fMSAutoNr_ + "') x) y";
                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }







        /// <summary>
        /// 获取所有员工的信息
        /// </summary>
        /// <returns>所有员工的信息</returns>
        public List<string> selectAllEmployeeInfor()
        {
            List<string> list = new List<string>();

            try
            {
                string sql = "Select fWorkerNr 员工编号,fWorkerName 员工姓名 From dbo.bsCenterWorker";
                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //将结果集信息添加到返回向量中
                    list.Add(reader[0].ToString());
                    list.Add(reader[1].ToString());
                }

                reader.Close();
                cmd.Dispose();

            }
            catch (Exception)
            {

            }
            return list;
        }


        /// <summary>
        /// 获取所有工作列表的信息
        /// </summary>
        /// <returns>获取所有工作列表的信息</returns>
        public List<string> selectAllWorklistInfor()
        {
            List<string> list = new List<string>();

            try
            {
                string sql = "Select x.fAutoNr_ 识别码, x.fMouldNr 模具编号, x.fProcName 工艺,x.fPieceName 工件,X.fWorkerNr 员工号, x.fWorkerName 员工,X.fMachineNr 机台号,x.fMachineName 机台, CONVERT(varchar(100), x.fSchBegDate, 23) 计划起工日期, CONVERT(varchar(100), x.fSchEndDate, 23) 计划完工日期 From ( Select b.fId_,CONVERT(Varchar(20),b.fAutoNr_) as fAutoNr_, b.fProcNr,b.fProcName,b.fPieceNr,b.fPieceName,b.fCenterNr,b.fCenterName, b.fSchBegDate,b.fSchEndDate,b.fSchHour, b.fWorkerNr,b.fWorkerName,b.fMachineNr,b.fMachineName, a.fNr,a.fMouldNr,a.fMouldName,b.fIsMachine From mkSchBase a Inner Join mkSchProduct b On a.fNr=b.fNr Where isNull(b.fisDetail,0)=0 And ISNULL(b.fIsEnd,0)=0  AND (b.fMachineNr Like 'L01') Union All Select c.fId_,c.fAutoNr_, c.fProcNr,c.fProcName,c.fPieceNr,c.fPieceName,b.fCenterNr,b.fCenterName, c.fSchBegDate,c.fSchEndDate,c.fSchHour, c.fWorkerNr,c.fWorkerName,c.fMachineNr,c.fMachineName, a.fNr,a.fMouldNr,a.fMouldName,b.fIsMachine From mkSchBase a Inner Join mkSchProduct b On a.fNr=b.fNr Inner Join mkSchDetail c On b.fId_ =c.fMSId_ Where isNull(b.fisDetail,0)=1 And ISNULL(c.fIsEnd,0)=0 AND (c.fMachineNr Like 'L01') ) X Order By x.fAutoNr_ ";
                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //将结果集信息添加到返回向量中
                    list.Add(reader[0].ToString());
                    list.Add(reader[1].ToString());
                    list.Add(reader[2].ToString());
                    list.Add(reader[3].ToString());
                    list.Add(reader[4].ToString());
                    list.Add(reader[5].ToString());
                    list.Add(reader[6].ToString());
                    list.Add(reader[7].ToString());
                    list.Add(reader[8].ToString());
                    list.Add(reader[9].ToString());
                }

                reader.Close();
                cmd.Dispose();

            }
            catch (Exception)
            {

            }
            return list;
        }
    }
}