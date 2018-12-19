using Common;
using DAL;
using Model;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace 拉网科技数据展示平台.Ashx
{
    /// <summary>
    /// FileRecordInfo 的摘要说明
    /// </summary>
    public class FileRecordInfo : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            string action = context.Request.QueryString["action"];

            switch (action)
            {
                case "getinlist": GetInList(context); break;
                case "getoutlist": GetOutList(context); break;
                case "getFileModel": GetFileModel(context); break;
                case "fileinput": FileUpload(context); break;
                case "fileoutput": FileOutput(context); break;

            }
        }


        /// <summary>
        /// 导出表格
        /// </summary>
        /// <param name="context"></param>
        private void FileOutput(HttpContext context)
        {
            string startdate = context.Request.Form["datemin"];
            string enddate = context.Request.Form["datemax"];

            if (!string.IsNullOrWhiteSpace(startdate) && !string.IsNullOrWhiteSpace(enddate))
            {
                DateTime start = Convert.ToDateTime(startdate).Date;
                DateTime end = Convert.ToDateTime(enddate).Date;
                if (start < end)
                {

                    //先生成表格头
                    //获取导出路径,文件名为当前时间加随机数
                    string urlpath = "file/model/" + DateTime.Now.ToString("yyyyMMdd-HH-mm-ss-") + Guid.NewGuid().ToString() + ".xls";
                    string savepath = context.Server.MapPath(urlpath);

                    //第一步需要生成文件
                    //第二步把生成文件的路径给前台，前台下载

                    #region  导出数据
                    //创建工作本
                    HSSFWorkbook workbook = new HSSFWorkbook();

                    DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                    SummaryInformation si = PropertySetFactory.CreateSummaryInformation();

                    dsi.Company = "南京刷拉拉网络科技有限公司";
                    dsi.Manager = "Office Word 2003/2007";

                    si.Author = "拉网科技交易数据平台1.0";
                    si.Subject = "交易数据";
                    si.Title = start.ToShortDateString() + "-" + end.ToShortDateString() + "交易数据报表";

                    workbook.DocumentSummaryInformation = dsi;
                    workbook.SummaryInformation = si;

                    /****************/
                    //日期样式
                    /****************/
                    //表格列头样式操作（水平垂直居中，绿色背景，字体颜色白色，黑体，）
                    var styleHeader3 = workbook.CreateCellStyle();
                    styleHeader3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平居中  
                    styleHeader3.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中  
                    styleHeader3.FillForegroundColor = HSSFColor.Green.Index;//前景色  
                    styleHeader3.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;//填充方式 AltBars
                    HSSFFont fontHeader3 = (HSSFFont)workbook.CreateFont();
                    fontHeader3.FontName = "黑体";//字体  
                    fontHeader3.FontHeightInPoints = 10;//字号  
                    fontHeader3.Color = HSSFColor.White.Index; //颜色 
                    fontHeader3.IsBold = true;//加粗  
                    styleHeader3.SetFont(fontHeader3);
                    /*****************/
                    /****************/



                    /****************/
                    //表格列头样式1
                    /****************/
                    //表格列头样式操作（水平垂直居中，蓝色背景，字体颜色白色，黑体，字体大小16）
                    var styleHeader1 = workbook.CreateCellStyle();
                    styleHeader1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平居中  
                    styleHeader1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中  
                    styleHeader1.FillForegroundColor = HSSFColor.Blue.Index;//前景色  
                    styleHeader1.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;//填充方式 AltBars
                    HSSFFont fontHeader1 = (HSSFFont)workbook.CreateFont();
                    fontHeader1.FontName = "黑体";//字体  
                    fontHeader1.FontHeightInPoints = 12;//字号  
                    fontHeader1.Color = HSSFColor.White.Index; //颜色 
                    fontHeader1.IsBold = true;//加粗  
                    styleHeader1.SetFont(fontHeader1);
                    /*****************/
                    /****************/

                    /****************/
                    //表格列头样式2
                    /****************/
                    //表格列头样式操作（水平垂直居中，绿色背景，字体颜色白色，黑体，字体大小16）
                    var styleHeader2 = workbook.CreateCellStyle();
                    styleHeader2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平居中  
                    styleHeader2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中  
                    styleHeader2.FillForegroundColor = HSSFColor.Green.Index;//前景色  
                    styleHeader2.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;//填充方式 AltBars
                    HSSFFont fontHeader2 = (HSSFFont)workbook.CreateFont();
                    fontHeader2.FontName = "黑体";//字体  
                    fontHeader2.FontHeightInPoints = 12;//字号  
                    fontHeader2.Color = HSSFColor.White.Index; //颜色 
                    fontHeader2.IsBold = true;//加粗  
                    styleHeader2.SetFont(fontHeader2);
                    /*****************/
                    /****************/

                    //样式数组
                    ICellStyle[] cellstyleArr = { styleHeader1, styleHeader2 };

                    //表格的第一行是POS商户总
                    //表格的第二行是POS商户后台分

                    /***********************创建工作表，在一个工作本里面可以有多张表*************************/
                    /********************************************************/

                    //创建工作表
                    ISheet sheet = workbook.CreateSheet("交易数据记录");
                    //获取所有POS商户总
                    List<MerchantName> listMName = new List<MerchantName>();
                    listMName = MerchantNameDal.GetList(new MerchantName());


                    //创建POS商户总，第一行
                    IRow row0 = sheet.CreateRow(0);
                    //合并单元格，第一列的1,2两行合并，里面内容为交易日期
                    sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));

                    //交易日期
                    ICell cellDate = row0.CreateCell(0);
                    cellDate.SetCellValue("交易日期");
                    sheet.SetColumnWidth(0, 15 * 256);
                    cellDate.CellStyle = cellstyleArr[1];


                    //创建POS商户分，第二行
                    IRow row1 = sheet.CreateRow(1);


                    //保存分后台id 
                    List<int> idarray = new List<int>();
                    idarray.Add(0);

                    int k = 0;
                    //遍历POS总
                    for (int i = 0; i < listMName.Count; i++)
                    {
                        //根据当前查询的Pos商户总，获取该总商户下面的后台
                        List<MerchantRear> listMRName = new List<MerchantRear>();
                        MerchantRear mrqury = new MerchantRear();
                        mrqury.FkID = listMName[i].ID;
                        listMRName = MerchantRearDal.GetList(mrqury);
                        //有分后台才进行创建
                        if (listMRName.Count > 0)
                        {
                            int j = 0;
                            for (j = 0; j < listMRName.Count; j++)
                            {
                                k++;
                                ICell cell1 = row1.CreateCell(k);
                                cell1.SetCellValue(listMRName[j].Name + "_" + listMRName[j].ID);
                                cell1.CellStyle = cellstyleArr[i % 2];
                                sheet.SetColumnWidth(k, 20 * 256);
                                idarray.Add(listMRName[j].ID);
                            }
                            if (j > 1)
                            {
                                //两个后台及以上才需要合并单元格，第一列的1,2两行合并，里面内容为Pos总商户名称
                                sheet.AddMergedRegion(new CellRangeAddress(0, 0, k - j + 1, k));
                            }
                            ICell cell0 = row0.CreateCell(k - j + 1);
                            cell0.SetCellValue(listMName[i].Name + "_" + listMName[i].ID);
                            cell0.CellStyle = cellstyleArr[i % 2];
                        }
                    }


                    //记录当前已经写入的行
                    int irow = 2;

                    //生成表格数据
                    for (DateTime itime = start; itime <= end; itime = itime.AddDays(1))
                    {
                        List<MainData> md = new List<MainData>();
                        md = MainDataDal.SearchByDate(itime.Date);
                        if (md.Count > 0)
                        {
                            //创建行
                            IRow rowdata = sheet.CreateRow(irow);

                            //记录日期
                            ICell cell0 = rowdata.CreateCell(0);
                            cell0.SetCellValue(itime.ToShortDateString());
                            cell0.CellStyle = styleHeader3;

                            //遍历表格列，有数据的填上，没有数据填0
                            for (int i = 1; i < idarray.Count; i++)
                            { 
                                MainData mdmoney = md.Where(u => u.FKID == idarray[i]).FirstOrDefault();
                                decimal money = mdmoney != null ? mdmoney.Money : 0;

                                ICell cell = rowdata.CreateCell(i);

                                cell.SetCellValue(money.ToString());
                            }

                            irow++;
                        }
                    }

                    sheet.CreateFreezePane(1, 2, 1, 2); 

                    try
                    {
                        FileStream file = new FileStream(savepath, FileMode.Create, FileAccess.Write);
                        workbook.Write(file);
                        file.Dispose();
                        //将文件导出记录保存到文件表中
                        FileRecord frsave = new FileRecord();
                        frsave.Type = 2;
                        frsave.Url = "../Ashx/" + urlpath;
                        frsave.FileName = DateTime.Now.ToString("yyyyMMdd-HH-mm-ss-") + "的导出数据.xls";
                        frsave.FileTime = DateTime.Now;
                        frsave.OperatorName = context.Session["Name"].ToString();
                        frsave.Count = 0;
                        frsave.State = "1";
                        FileRecordDal.Insert(frsave);

                        context.Response.Write("../Ashx/" + urlpath);
                    }
                    catch
                    {
                        context.Response.Write("false");
                    }



                    #endregion



                }

            }
            else
            {
                context.Response.Write("false");
            }
        }
        

        /// <summary>
        /// 查询导入记录
        /// </summary>
        /// <param name="context"></param>
        private void GetInList(HttpContext context)
        {
            string json = JsonHelper.ListToJson<FileRecord>(FileRecordDal.GetFileList(new FileRecord(1)));
            context.Response.Write(json);
        }

        /// <summary>
        /// 查询导出记录
        /// </summary>
        /// <param name="context"></param>
        private void GetOutList(HttpContext context)
        {
            string json = JsonHelper.ListToJson<FileRecord>(FileRecordDal.GetFileList(new FileRecord(2)));
            context.Response.Write(json);
        }



        /// <summary>
        /// 获取文件模板
        /// </summary>
        /// <param name="context"></param>
        private void GetFileModel(HttpContext context)
        {

            //获取导出路径,文件名为当前时间加随机数
            string urlpath = "file/model/" + DateTime.Now.ToString("yyyyMMdd-HH-mm-ss-") + Guid.NewGuid().ToString() + ".xls";
            string savepath = context.Server.MapPath(urlpath);

            //第一步需要生成文件
            //第二步把生成文件的路径给前台，前台下载

            #region  生成模板
            //创建工作本
            HSSFWorkbook workbook = new HSSFWorkbook();

            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();

            dsi.Company = "南京刷拉拉网络科技有限公司";
            dsi.Manager = "Office Word 2003/2007";

            si.Author = "拉网科技交易数据平台1.0";
            si.Subject = "交易数据模板";
            si.Title = "交易数据报表";

            workbook.DocumentSummaryInformation = dsi;
            workbook.SummaryInformation = si;

            /****************/
            //表格列头样式1
            /****************/
            //表格列头样式操作（水平垂直居中，蓝色背景，字体颜色白色，黑体，字体大小16）
            var styleHeader1 = workbook.CreateCellStyle();
            styleHeader1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平居中  
            styleHeader1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中  
            styleHeader1.FillForegroundColor = HSSFColor.Blue.Index;//前景色  
            styleHeader1.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;//填充方式 AltBars
            HSSFFont fontHeader1 = (HSSFFont)workbook.CreateFont();
            fontHeader1.FontName = "黑体";//字体  
            fontHeader1.FontHeightInPoints = 12;//字号  
            fontHeader1.Color = HSSFColor.White.Index; //颜色 
            fontHeader1.IsBold = true;//加粗  
            styleHeader1.SetFont(fontHeader1);
            /*****************/
            /****************/

            /****************/
            //表格列头样式2
            /****************/
            //表格列头样式操作（水平垂直居中，绿色背景，字体颜色白色，黑体，字体大小16）
            var styleHeader2 = workbook.CreateCellStyle();
            styleHeader2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;//水平居中  
            styleHeader2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//垂直居中  
            styleHeader2.FillForegroundColor = HSSFColor.Green.Index;//前景色  
            styleHeader2.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;//填充方式 AltBars
            HSSFFont fontHeader2 = (HSSFFont)workbook.CreateFont();
            fontHeader2.FontName = "黑体";//字体  
            fontHeader2.FontHeightInPoints = 12;//字号  
            fontHeader2.Color = HSSFColor.White.Index; //颜色 
            fontHeader2.IsBold = true;//加粗  
            styleHeader2.SetFont(fontHeader2);
            /*****************/
            /****************/

            //样式数组
            ICellStyle[] cellstyleArr = { styleHeader1, styleHeader2 };

            //表格的第一行是POS商户总
            //表格的第二行是POS商户后台分

            /***********************创建工作表，在一个工作本里面可以有多张表*************************/
            /********************************************************/

            //创建工作表
            ISheet sheet = workbook.CreateSheet("交易数据记录");
            //获取所有POS商户总
            List<MerchantName> listMName = new List<MerchantName>();
            listMName = MerchantNameDal.GetList(new MerchantName());


            //创建POS商户总，第一行
            IRow row0 = sheet.CreateRow(0);
            //合并单元格，第一列的1,2两行合并，里面内容为交易日期
            sheet.AddMergedRegion(new CellRangeAddress(0, 1, 0, 0));

            //交易日期
            ICell cellDate = row0.CreateCell(0);
            cellDate.SetCellValue("交易日期");
            sheet.SetColumnWidth(0, 15 * 256);
            cellDate.CellStyle = cellstyleArr[1];


            //创建POS商户分，第二行
            IRow row1 = sheet.CreateRow(1);

            int k = 0;
            //遍历POS总
            for (int i = 0; i < listMName.Count; i++)
            {
                //根据当前查询的Pos商户总，获取该总商户下面的后台
                List<MerchantRear> listMRName = new List<MerchantRear>();
                MerchantRear mrqury = new MerchantRear();
                mrqury.FkID = listMName[i].ID;
                listMRName = MerchantRearDal.GetList(mrqury);
                //有分后台才进行创建
                if (listMRName.Count > 0)
                {
                    int j = 0;
                    for (j = 0; j < listMRName.Count; j++)
                    {
                        k++;
                        ICell cell1 = row1.CreateCell(k);
                        cell1.SetCellValue(listMRName[j].Name + "_" + listMRName[j].ID);
                        cell1.CellStyle = cellstyleArr[i % 2];
                        sheet.SetColumnWidth(k, 20 * 256);
                    }
                    if (j > 1)
                    {
                        //两个后台及以上才需要合并单元格，第一列的1,2两行合并，里面内容为Pos总商户名称
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, k - j + 1, k));
                    }
                    ICell cell0 = row0.CreateCell(k - j + 1);
                    cell0.SetCellValue(listMName[i].Name + "_" + listMName[i].ID);
                    cell0.CellStyle = cellstyleArr[i % 2];
                }
            }
            try
            {
                FileStream file = new FileStream(savepath, FileMode.Create, FileAccess.Write);
                workbook.Write(file);
                file.Dispose();
                //将文件导出记录保存到文件表中
                FileRecord frsave = new FileRecord();
                frsave.Type = 2;
                frsave.Url = "../Ashx/" + urlpath;
                frsave.FileName = DateTime.Now.ToString("yyyyMMdd-HH-mm-ss-")+"导入模板.xls";
                frsave.FileTime = DateTime.Now;
                frsave.OperatorName = context.Session["Name"].ToString();
                frsave.Count = 0;
                frsave.State = "1";
                FileRecordDal.Insert(frsave);

                context.Response.Write("../Ashx/" + urlpath);
            }
            catch
            {
                context.Response.Write("error");
            }




            #endregion

        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="context"></param>
        private void FileUpload(HttpContext context)
        {
            if (context.Request.Files.Count > 0 && context.Request.Files[0].ContentLength > 1)
            {
                try
                {
                    //只要上传了文件，先把文件保存了，再解析存入数据库
                    HttpPostedFile file = context.Request.Files[0];
                    string urlpath = "file/infile/" + DateTime.Now.ToString("yyyyMMdd-HH-mm-ss-") + file.FileName;
                    file.SaveAs(context.Server.MapPath(urlpath));


                    string OName = context.Session["Name"].ToString();

                    var importCore = new ExcelImportCore();
                    importCore.LoadFile(context.Server.MapPath(urlpath));
                    DataSet ds = importCore.GetAllTables(false);


                    //DataSet ds = ExcelSqlConnection(context.Server.MapPath(urlpath), "tranportTask");

                    string errinfo = "";

                    //表头错误判断
                    bool headflag = false;
                    //录入数据量计算
                    int datacount = 0;
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {  
                        //获取了一个sheet页所有内容
                        DataTable dtsheet = ds.Tables[i];

                        //读取表头数据并校验是否正确,如果全部正确，还需要将这些id存到数组中
                        List<int> idarr = new List<int>();
                        idarr.Add(0);

                        //除了表头两行，需要有数据才可以录入
                        if (dtsheet.Rows.Count > 2)
                        {
                            
                            //第0列是交易日期，从第二行第二列开始验证商户后台是否正确
                            for (int j = 1; j < dtsheet.Columns.Count; j++)
                            {
                                string headcheck = dtsheet.Rows[1][j].ToString();
                                if (headcheck.Contains("_"))
                                {
                                    string idstr = headcheck.Split('_')[1];
                                    if (IsNumeric(idstr))
                                    {
                                        int id = Convert.ToInt32(idstr);
                                        MerchantRear mrid = new MerchantRear();
                                        mrid.ID = id;
                                        if (MerchantRearDal.GetList(mrid).Count == 1)
                                        {
                                            idarr.Add(id);
                                        }
                                        else
                                        {
                                            errinfo = "第2行第" + (j + 1).ToString() + "列表头数据有错误_id不存在！";
                                            headflag = true;
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        errinfo = "第2行第" + (j + 1).ToString() + "列表头数据有错误！";
                                        headflag = true;
                                        break;
                                    }

                                }
                                else
                                {
                                    errinfo = "第2行第" + (j + 1).ToString() + "列表头数据有错误！";
                                    headflag = true;
                                    break;
                                }
                            }
                            if (headflag)
                            {
                                break;
                            }

                        }
                        else
                        {
                            errinfo = "第" + i.ToString() + "个sheet页表格数据为空！";
                            break;
                        }


                        if (!headflag)
                        {
                            //录入表格内容
                            //从表格第3行第1列开始遍历
                            for (int r = 2; r < dtsheet.Rows.Count; r++)
                            {
                                //每一行第一列是时间，获取时间
                                string datestr = dtsheet.Rows[r][0].ToString();
                                DateTime date = new DateTime();
                                if (string.IsNullOrWhiteSpace(datestr))
                                {
                                    errinfo = "第" + r.ToString() + "行第1列的记录时间为空！";
                                    break;
                                }
                                else
                                {
                                    date = Convert.ToDateTime(datestr);
                                }

                                for (int c = 1; c < dtsheet.Columns.Count; c++)
                                {
                                    string moneystr = dtsheet.Rows[r][c].ToString();
                                    decimal money = 0;
                                    if (!string.IsNullOrWhiteSpace(moneystr))
                                    {
                                        money = Convert.ToDecimal(moneystr);
                                    }

                                    MainData newmd = new MainData();
                                    newmd.FKID = idarr[c];
                                    newmd.Money = money;
                                    newmd.Date = date;
                                    newmd.RegistrarName = OName;

                                    MainDataDal.Add(newmd);
                                    datacount++;
                                }
                            }

                        }
                        
                    }

                    //将文件导入记录保存到文件表中
                    FileRecord frsave = new FileRecord();
                    frsave.Type = 1;
                    frsave.Url = "../Ashx/" + urlpath;
                    frsave.FileName = file.FileName;
                    frsave.FileTime = DateTime.Now;
                    frsave.OperatorName = OName;
                    frsave.Count = datacount;
                    frsave.State = "1";
                    FileRecordDal.Insert(frsave);

                    if (headflag)
                    {
                        context.Response.Write("{ \"code\":\"1\",\"msg\":\"" + errinfo + "\" }");
                    }
                    else
                    {
                        context.Response.Write("{ \"code\":\"0\",\"msg\":\"OK!您共录入：" + datacount.ToString() + "条交易数据\" }");
                    }

                }
                catch
                {
                    context.Response.Write("{ \"code\":\"1\",\"msg\":\"发生未知错误！请联系开发人员！\" }");
                }



                

                
               


            }
        }

        /// <summary>
        /// 判断一个字符串是否是纯数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool IsNumeric(string str) //接收一个string类型的参数,保存到str里
        {
            if (str == null || str.Length == 0)    //验证这个参数是否为空
                return false;                           //是，就返回False
            ASCIIEncoding ascii = new ASCIIEncoding();//new ASCIIEncoding 的实例
            byte[] bytestr = ascii.GetBytes(str);         //把string类型的参数保存到数组里

            foreach (byte c in bytestr)                   //遍历这个数组里的内容
            {
                if (c < 48 || c > 57)                          //判断是否为数字
                {
                    return false;                              //不是，就返回False
                }
            }
            return true;                                        //是，就返回True
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}