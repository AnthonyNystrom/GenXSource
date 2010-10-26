using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenMediImage
{
    public enum FileType
    {
        Normal,
        PPM,
        RAW,
        DICOM,
        Analyze,
        Inter,
        ECAT,
        Vision,
        Genisis
    }

    public enum ThumbnailFileType
    {       
        AllImages,
        CommonImages,
        DICOM
    }
}
