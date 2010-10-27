using System;

namespace Next2Friends.WebServices.Tag
{
    public sealed class TagUpdate
    {
        public TagUpdate()
        {
        }

        public TagUpdate(Int32 length)
        {
            TagValidationString = new String[length];
            DeviceTagID = new String[length];
        }

        public String[] TagValidationString { get; set; }
        public String[] DeviceTagID { get; set; }

        public Int32 CheckArraylength()
        {
            if (TagValidationString.Length != DeviceTagID.Length)
                return 0;
            return DeviceTagID.Length;
        }
    }
}
