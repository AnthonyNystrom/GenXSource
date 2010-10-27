using System;
using System.Windows.Forms;

using Leadtools;

namespace Leadtools.Demos
{
   public static class Support
   {
      public const string MedicalServerKey = "";

      public static void Unlock()
      {
         RasterSupport.Unlock(RasterSupportType.Abc, "");
         RasterSupport.Unlock(RasterSupportType.AbicRead, "");
         RasterSupport.Unlock(RasterSupportType.AbicSave, "");
         RasterSupport.Unlock(RasterSupportType.Barcodes1D, "");
         RasterSupport.Unlock(RasterSupportType.Barcodes1DSilver, "");
         RasterSupport.Unlock(RasterSupportType.BarcodesDataMatrixRead, "");
         RasterSupport.Unlock(RasterSupportType.BarcodesDataMatrixWrite, "");
         RasterSupport.Unlock(RasterSupportType.BarcodesPdfRead, "");
         RasterSupport.Unlock(RasterSupportType.BarcodesPdfWrite, "");
         RasterSupport.Unlock(RasterSupportType.BarcodesQRRead, "");
         RasterSupport.Unlock(RasterSupportType.BarcodesQRWrite, "");
         RasterSupport.Unlock(RasterSupportType.Bitonal, "");
         RasterSupport.Unlock(RasterSupportType.Cmw, "");
         RasterSupport.Unlock(RasterSupportType.Dicom, "");
         RasterSupport.Unlock(RasterSupportType.Document, "");
         RasterSupport.Unlock(RasterSupportType.DocumentWriters, "");
         RasterSupport.Unlock(RasterSupportType.DocumentWritersPdf, "");
         RasterSupport.Unlock(RasterSupportType.ExtGray, "");
         RasterSupport.Unlock(RasterSupportType.Forms, "");
         RasterSupport.Unlock(RasterSupportType.IcrPlus, "");
         RasterSupport.Unlock(RasterSupportType.IcrProfessional, "");
         RasterSupport.Unlock(RasterSupportType.J2k, "");
         RasterSupport.Unlock(RasterSupportType.Jbig2, "");
         RasterSupport.Unlock(RasterSupportType.Jpip, "");
         RasterSupport.Unlock(RasterSupportType.Pro, "");
         RasterSupport.Unlock(RasterSupportType.LeadOmr, "");
         RasterSupport.Unlock(RasterSupportType.MediaWriter, "");
         RasterSupport.Unlock(RasterSupportType.Medical, "");
         RasterSupport.Unlock(RasterSupportType.Medical3d, "");
         RasterSupport.Unlock(RasterSupportType.MedicalNet, "");
         RasterSupport.Unlock(RasterSupportType.MedicalServer, MedicalServerKey);
         RasterSupport.Unlock(RasterSupportType.Mobile, "");
         RasterSupport.Unlock(RasterSupportType.Nitf, "");
         RasterSupport.Unlock(RasterSupportType.OcrAdvantage, "");
         RasterSupport.Unlock(RasterSupportType.OcrAdvantagePdfLeadOutput, "");
         RasterSupport.Unlock(RasterSupportType.OcrArabic, "");
         RasterSupport.Unlock(RasterSupportType.OcrPlus, "");
         RasterSupport.Unlock(RasterSupportType.OcrPlusPdfOutput, "");
         RasterSupport.Unlock(RasterSupportType.OcrPlusPdfLeadOutput, "");
         RasterSupport.Unlock(RasterSupportType.OcrProfessional, "");
         RasterSupport.Unlock(RasterSupportType.OcrProfessionalAsian, "");
         RasterSupport.Unlock(RasterSupportType.OcrProfessionalPdfOutput, "");
         RasterSupport.Unlock(RasterSupportType.OcrProfessionalPdfLeadOutput, "");
         RasterSupport.Unlock(RasterSupportType.PdfAdvanced, "");
         RasterSupport.Unlock(RasterSupportType.PdfRead, "");
         RasterSupport.Unlock(RasterSupportType.PdfSave, "");
         RasterSupport.Unlock(RasterSupportType.PrintDriver, "");
         RasterSupport.Unlock(RasterSupportType.Vector, "");          
      }
   }
}
