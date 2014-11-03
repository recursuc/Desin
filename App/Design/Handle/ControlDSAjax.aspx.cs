using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using RAD.Common;
using System.Xml;

namespace ReportSystem.Application.Design.Handle
{
    public partial class ControlDSAjax : System.Web.UI.Page
    {
        RAD.Form.Bussiness.FormService formClient = new RAD.Form.Bussiness.FormService();

        protected void Page_Load(object sender, EventArgs e)
        {
            //加载xml
            Response.Clear();
            Response.ContentType = "Text/Xml";
            System.Xml.XmlDocument xmlDel = new XmlDocument();
            xmlDel.Load(Request.InputStream);

            //创建返回Xml对象
            XmlDocument xmlReturn = SysFunction.getXmlDocModel();

            try
            {
                //根据参数获取信息
                XmlNode xnParams = xmlDel.SelectSingleNode("RAD/Doc/Data");
                XmlNode xnReturn = xmlReturn.SelectSingleNode("RAD/Doc/Data");

                foreach (XmlNode xnParam in xnParams.ChildNodes)
                {
                    switch (xnParam.Attributes["ParamType"].Value)
                    {
                        case "GetDBSourceItems":
                            {
                                //取xml数据
                                string strDataSource = "";
                                string strValueMember = "";
                                string strTextMember = "";
                                strDataSource = xnParam.Attributes["DataSource"].Value;
                                strValueMember = xnParam.Attributes["DbValueColumn"].Value;
                                strTextMember = xnParam.Attributes["DbTextColumn"].Value;

                                DataTable dt = this.GetDictionaryDataByParm(strDataSource);

                                if (strDataSource != "" && strValueMember != "" && strTextMember != "")
                                {
                                    XmlElement xeDBSource = xmlReturn.CreateElement("DBSource");

                                    //生成需要返回的xml数据
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        XmlNode newNode = xmlReturn.CreateNode(XmlNodeType.Element, "Item", "");

                                        XmlAttribute xa = xmlReturn.CreateAttribute("id");
                                        xa.Value = dr[strValueMember].ToString();
                                        newNode.Attributes.Append(xa);

                                        xa = xmlReturn.CreateAttribute("name");
                                        xa.Value = dr[strTextMember].ToString();
                                        newNode.Attributes.Append(xa);

                                        xeDBSource.AppendChild(newNode);
                                    }
                                    xnReturn.AppendChild(xeDBSource);
                                }
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                xmlReturn = SysFunction.getXmlDocWithResult("1", ex.Message);
                //throw new Exception(ex.Message, ex);
            }

            //返回获取的信息
            xmlReturn.Save(Response.OutputStream);
        }

        /// <summary>
        /// 通过指定数据源，返回含有指定字段的DataTable数据。
        /// </summary>
        /// <param name="strDataSource">指定数据源。如：select * from pnet_base_data where parent_id = 'test'。</param>
        /// <returns>返回含有指定值字段和文本字段的DataTable数据</returns>
        private DataTable GetDictionaryDataByParm(string dataSourceSql)
        {
            DataTable dictionaryData = new DataTable();

            try
            {
                dictionaryData = formClient.GetDictionaryDataByParm(dataSourceSql);
            }
            catch (Exception err)
            {
                throw err;
            }

            return dictionaryData;
        }
    }
}