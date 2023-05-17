using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolThongBaoBaiDangMoi
{
    public class ChoTotModel
    {
        public List<SubjectModel> ads { get; set; }
    }
    public class SubjectModel
    {
        public string subject { get; set; }
        public string list_id { get; set; }
        public string price { get; set; }
        public List<Params> @params { get; set; }
    }

    public class Params
    {
        public string id { get; set; }
        public string value { get; set; }
        public string label { get; set; }
    }



    public class LastDataModel
    {
        public LastDataModel()
        {
            Region = string.Empty;
            Subject = string.Empty;
            ID = string.Empty;
        }

        public string Region { get; set; }
        public string Subject { get; set; }
        public string ID { get; set; }
        public string LinkGet { get; set; }
    }
}
