using System;

namespace Next2Friends.WebServices.Ask
{
    public sealed class AskResponse
    {
        public Int32 AskQuestionID { get; set; }
        public String Question { get; set; }
        public String PhotoBase64Binary { get; set; }
        public Int32[] ResponseValues { get; set; }
        public Double Average { get; set; }
        public Int32 ResponseType { get; set; }
        public String[] CustomResponses { get; set; }
    }
}
