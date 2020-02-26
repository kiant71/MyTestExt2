using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace MyTestExt.ConsoleApp
{
    public partial class OrmLiteTest
    {

        /// <summary>
        /// 协同 - 模板
        /// </summary>
        [Alias("T_OLTE")]
        public class OrmLiteTestModel
        {
            /// <summary>
            /// 模版编号(-100000开始为系统默认模版)
            /// </summary>
            [PrimaryKey]
            public long TestID { get; set; }

            /// <summary>
            /// 归属公司（系统创建的公用模版则是0）
            /// </summary>
            public int CompanyID { get; set; }

            /// <summary>
            /// 模版名称
            /// </summary>
            public string TestName { get; set; }

            /// <summary>
            /// 模版类型(因为模版还有一种工程布局的)
            /// </summary>
            public short TestType { get; set; }
            
            /// <summary>
            /// 模板描述
            /// </summary>
            public string Remark { get; set; }



            public int ImageIndex { get; set; }

            [Ignore]
            public string ImageUrl { get; set; }

            [Ignore]
            public string ImageFullUrl { get; set; }

        }


        //[Alias("T_UCMT1")]
        public class TemplateFieldForDbModel
        {
            [PrimaryKey]
            public long TemplateID { get; set; }

            [Alias("CustmoFields")]
            public List<TemplateFieldModel> Items { get; set; }

            //public string CustmoFields { get; set; }
        }


        /// <summary>
        /// 模板字段 定义
        /// </summary>
        public class TemplateFieldModel
        {
            /// <summary>
            /// 字段名称
            /// </summary>
            public string FieldName { get; set; }

            /// <summary>
            /// 字段类型暂支持 1:日期  2:数量  3:单价 4:时间 5:金额   8:字符 10:数值  13:日期时间
            /// </summary>
            //public FieldType FieldType { get; set; }

            /// <summary>
            /// 字段标题
            /// </summary>
            public string FieldCaption { get; set; }

            /// <summary>
            /// 标题字段方向
            /// </summary>
            public TemplateFieldTitleDirectionType CaptionDirection { get; set; }

            /// <summary>
            /// 不能为空
            /// </summary>
            public bool NotNull { get; set; }

            /// <summary>
            /// 空值提示 
            /// </summary>
            public string NullPrompt { get; set; }

            /// <summary>
            /// 是否多行显示
            /// </summary>
            public bool MultiLine { get; set; }

            /// <summary>
            /// 是否有下拉选项
            /// </summary>
            public bool IsDropItem { get; set; }

            /// <summary>
            /// 下拉选项
            /// </summary>
            public List<TemplateFieldDropItemModel> DropDownItem { get; set; }

        }


        public class TemplateFieldDropItemModel
        {
            /// <summary>
            /// 字段值
            /// </summary>

            public string Code { get; set; }

            /// <summary>
            /// 显示值
            /// </summary>

            public string Name { get; set; }
        }

        /// <summary>
        /// 模板字段标题方向
        /// </summary>
        public enum TemplateFieldTitleDirectionType
        {
            /// <summary>
            /// 空
            /// </summary>
            None = 0,

            /// <summary>
            /// 左右
            /// </summary>
            LeftRight = 1,

            /// <summary>
            /// 上下
            /// </summary>
            UpDown = 2,
        }

    }
}
