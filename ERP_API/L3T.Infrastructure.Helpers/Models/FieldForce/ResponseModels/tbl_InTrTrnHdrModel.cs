using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class tbl_InTrTrnHdrModel
    {
        [Key]
        public int AutoSl { get; set; }
        public string Trn_Hdr_Type { get; set; }
        public string Trn_Hdr_Code { get; set; }
        public string Trn_Hdr_Ref { get; set; }
        public string Trn_Hdr_Pcode { get; set; }
        public string Trn_Hdr_Dcode { get; set; }
        public string Trn_Hdr_Acode { get; set; }
        public DateTime Trn_Hdr_DATE { get; set; }
        public string Trn_Hdr_Com1 { get; set; }
        public string Trn_Hdr_Com2 { get; set; }
        public string Trn_Hdr_Com3 { get; set; }
        public string Trn_Hdr_Com4 { get; set; }
        public string Trn_Hdr_Com5 { get; set; }
        public string Trn_Hdr_Com6 { get; set; }
        public string Trn_Hdr_Com7 { get; set; }
        public string Trn_Hdr_Com8 { get; set; }
        public string Trn_Hdr_Com9 { get; set; }
        public string Trn_Hdr_Com10 { get; set; }
        public decimal Trn_Hdr_Value { get; set; }
        public string Trn_Hdr_HRPB_Flag { get; set; }
        public string Trn_Hdr_Ent_Prd { get; set; }
        public string Trn_Hdr_Opr_Code { get; set; }
        public string Trn_Hdr_Prd_Cld { get; set; }
        public string Trn_Hdr_Exp_Typ { get; set; }
        public string Trn_Hdr_Led_Int { get; set; }
        public string Trn_Hdr_DC_No { get; set; }
        public string Trn_Hdr_EI_Flg { get; set; }
        public string Trn_Hdr_Cno { get; set; }
        public string T_C1 { get; set; }
        public string T_C2 { get; set; }
        public string T_Fl { get; set; }
        public int T_In { get; set; }
        public decimal Trn_Hdr_exc_duty { get; set; }
        public DateTime Trn_Hdr_Dc_Date { get; set; }
        public DateTime Trn_Hdr_CI_Date { get; set; }
        public string Trn_Hdr_Pass_No { get; set; }
        public string Emp_Code { get; set; }
    }
}
