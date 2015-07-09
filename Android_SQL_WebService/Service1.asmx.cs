using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Android_SQL_WebService
{
    /// <summary>
    /// Service1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {

        DBOperation dbOperation = new DBOperation();

        [WebMethod(Description = "添加录单信息")]
        public bool insertActDaily(string fActType, string fAddFlag, string fWorkerCardNr, string fMachineCardNr, string fMSAutoNr_)
        {
            return dbOperation.insertActDaily(fActType, fAddFlag, fWorkerCardNr, fMachineCardNr, fMSAutoNr_);
        }

        [WebMethod(Description = "获取所有员工信息")]
        public string[] selectAllEmployeeInfor()
        {
            return dbOperation.selectAllEmployeeInfor().ToArray();
        }

        [WebMethod(Description = "获取工作列表信息")]
        public string[] selectAllWorklistInfor()
        {
            return dbOperation.selectAllWorklistInfor().ToArray();
        }
    }
}
