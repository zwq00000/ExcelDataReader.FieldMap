using System;
using System.ComponentModel.DataAnnotations;

namespace ExcelDataReader.FieldMaps.Tests
{
    public class StudentDraft {

        public int RowNum { get; set; }

        //
        // 摘要:
        //     JCXS010101 学号, 人员号
        [Key]
        [StringLength (20)]
        public string XH { get; set; }
        //
        // 摘要:
        //     行政班编号 JYBZ.MIFVS.ZZJX.ZZJX0202.XZBDM
        [Required]
        [StringLength (10)]
        public string XZBDM { get; set; }
        //
        // 摘要:
        //     JCTB020101 姓名, 姓名
        [Required]
        [StringLength (36)]
        public string XM { get; set; }
        //
        // 摘要:
        //     JCTB020103 姓名拼音, 姓名拼音
        [StringLength (60)]
        public string XMPY { get; set; }
        //
        // 摘要:
        //     JCTB020105 性别码, 性别码
        [Required]
        [StringLength (1)]
        public string XBM { get; set; }
        //
        // 摘要:
        //     JCTB020106 出生日期
        [DataType (DataType.Date)]
        [Required]
        public DateTime CSRQ { get; set; }
        //
        // 摘要:
        //     JCTB020112 身份证件号, 身份证件号
        [Required]
        [StringLength (20)]
        public string SFZJH { get; set; }
        //
        // 摘要:
        //     ZZZS010004 入学年月, 年月
        [Required]
        [StringLength (6)]
        public string RXNY { get; set; }
        //
        // 摘要:
        //     ZZJX010102 专业代码
        [Required]
        [StringLength (6)]
        public string ZYDM { get; set; }
        //
        // 摘要:
        //     ZZJX010103 专业名称
        [Required]
        [StringLength (50)]
        public string ZYMC { get; set; }
        //
        // 摘要:
        //     ZZXS010105 学生联系电话, 移动电话
        [Phone]
        [StringLength (30)]
        public string XSLXDH { get; set; }
        //
        // 摘要:
        //     JCTB010106 电子信箱, 电子信箱
        [EmailAddress]
        [StringLength (40)]
        public string DZXX { get; set; }
        //
        // 摘要:
        //     ZXXS010114 学籍号
        [StringLength (30)]
        public string XJH { get; set; }
    }
}