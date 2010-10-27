package n2f.sup.common.utils;

import javax.microedition.lcdui.Image;

import n2f.sup.utils.MemoryDispatcher;


public class GraphicsUtils {

	private final static int MAX_HEIGHT_FRAME = 25;
	
	private GraphicsUtils() {}
	
	
	private static int[] resizeImage(int[] pixels, int oldW, int oldH, double scaleW, double scaleH) {
        int newW = (int)(oldW * scaleW);
        int newH = (int)(oldH * scaleH);

	    //resize code /not optimized/
	    int[] lines = new int[newW*oldH];
	    
	    //?ыстрый алгоритм
	    if (newW < oldW){
	        for (int k = 0; k < oldH; k++) { // trough all lines
	          int i = k * oldW; // index in old pix
	          int j = k * newW; // index in new pix
	          int part = newW;
	          int addon = 0, r=0, g=0, b=0, a=0;
	          for (int m=0; m<newW; m++){ ///OPTI ijm!!! need???
	            int total = oldW;
	            int R=0, G=0, B=0, A=0;
	            if (addon!=0){
	              R=r*addon; G=g*addon; B=b*addon; A=a*addon;
	              total-=addon;
	            }
	            while (0<total){
	              a = (pixels[i] >> 24) & 0xff;
	              r = (pixels[i] >> 16) & 0xff;
	              g = (pixels[i] >>  8) & 0xff;
	              b =  pixels[i++]      & 0xff;
	              if (total>part){
	                R+=r*part; G+=g*part; B+=b*part; A+=a*part;
	              }
	              else{ 
	                R+=r*total; G+=g*total; B+=b*total; A+=a*total;
	                addon = part - total;
	                //set new pixel
	                lines[j++]=((R/oldW)<<16)|((G/oldW)<<8)|
	                    (B/oldW)|((A/oldW)<<24); // A??
	              }
	              total-=part;
	            }
	          }
	        }
	    } else { /// newW > oldW
	        int part = oldW;
	        for (int k = 0; k < oldH; k++) { // trough all lines
	          int i = k * oldW; // index in old pix
	          int j = k * newW; // index in new pix
	          int total= 0;
	          int r=0, g=0, b=0, a=0;
	          for (int m=0; m<newW; m++){
	            int R=0, G=0, B=0, A=0;
	            if (total>=part){
	              R=r*part; G=g*part; B=b*part; A=a*part;
	              total-=part;
	            }
	            else{
	              if (0!=total){
	                R=r*total; G=g*total; B=b*total; A=a*total;
	              }
	              a = (pixels[i] >> 24) & 0xff;
	              r = (pixels[i] >> 16) & 0xff;
	              g = (pixels[i] >>  8) & 0xff;
	              b =  pixels[i++]      & 0xff;
	              int addon = part - total;
	              R+=r*addon; G+=g*addon; B+=b*addon; A+=a*addon;
	              total=newW - addon;
	            }
	            //set new pixel
	            lines[j++]=((R/oldW)<<16)|((G/oldW)<<8)|
	               (B/oldW)|((A/oldW)<<24); // A??
	          }
	        }
	    }
	    
	    pixels = null;
	    MemoryDispatcher.gc();
	    
	    int[] columns = new int[newW * newH];
	    //проходим по столбцам
	    if (newH < oldH) {
	        for (int k = 0; k < newW; k++) { // trough columns
	        int i = k; // index in lines pix
	        int j = k; // index in new pix
	        int part = newH;
	        int addon = 0, r=0, g=0, b=0, a=0;
	        for (int m=0; m<newH; m++){
	          int total = oldH;
	          int R=0, G=0, B=0, A=0;
	          if (addon!=0){
	            R=r*addon; G=g*addon; B=b*addon; A=a;//*addon;
	            total-=addon;
	          }
	          while (0<total){
	        	//a = (lines[i] >> 24) & 0xff;// may no rotate
	            a =  lines[i] & 0xff000000;
	            r = (lines[i] >> 16) & 0xff;
	            g = (lines[i] >> 8)  & 0xff;
	            b =  lines[i]        & 0xff;
	            i+=newW;
	            if (total>part){
	              R+=r*part; G+=g*part; B+=b*part; A+=a;//*part;
	            }
	            else{
	              R+=r*total; G+=g*total; B+=b*total; A+=a;//*total;
	              addon = part - total;
	              //set new pixel
	              if (0!=A)
	              columns[j]=((R/oldH)<<16)|((G/oldH)<<8)|
	                 (B/oldH)|0xff000000; // A??
	              else
	              columns[j]=0;//((R/oldH)<<16)|((G/oldH)<<8)|(B/oldH); // A??
	              j+=newW;
	            }
	            total-=part;
	          }
	        }
	      }
	    } else {
	      int part = oldH;
	      for (int k = 0; k < newW; k++) { // trough all lines
	        int i = k; // index in old pix
	        int j = k; // index in new pix
	        int total= 0;
	        int r=0, g=0, b=0, a=0;
	        for (int m=0; m<newH; m++){
	          int R=0, G=0, B=0, A=0;
	          if (total>=part){
	            R=r*part; G=g*part; B=b*part; A=a;//*part;
	            total-=part;
	          }
	          else{
	            if (0!=total){
	              R=r*total; G=g*total; B=b*total; A=a;//*total;
	            }
	            //a = (lines[i] >> 24) & 0xff;// may no rotate
	            a =  lines[i] & 0xff000000;
	            r = (lines[i] >> 16) & 0xff;
	            g = (lines[i] >>  8) & 0xff;
	            b =  lines[i]        & 0xff;
	            i+=newW;
	            int addon = part - total;
	            R+=r*addon; G+=g*addon; B+=b*addon; A+=a;//*addon;
	            total=newH - addon;
	          }
	          //set new pixel
	          if (0!=A)
	          columns[j]=((R/oldH)<<16)|((G/oldH)<<8)|
	             (B/oldH)|0xff000000; // A??
	          else
	          columns[j]=0;//((R/oldH)<<16)|((G/oldH)<<8)|(B/oldH);

	          j+=newW;
	        }
	      }
	    }
	    lines = null;
	    MemoryDispatcher.gc();
	    
	    return columns;
	}	
	

	synchronized public static Image resizeImage(Image image, double scaleW, double scaleH) {
		if(scaleW == 1.0 && scaleH == 1.0) return image;
		
		image = resizeBrokeImage(image, scaleW, scaleH);	
		return image;
	}
	
//	synchronized public static byte[] getResizeImageBytes(Image image, double scaleW, double scaleH) {
//		image = resizeBrokeImage(image, scaleW, scaleH);
//	}
	
//	private static byte[] getBrokeImageBytes(Image image, double scaleW, double scaleH) {
//		System.out.println("resize scale: "+scaleW + "x"+ scaleH);
//		scaleW *= getCorrectionCoefficient(image.getWidth(), scaleW);
//		scaleH *= getCorrectionCoefficient(MAX_HEIGHT_FRAME, scaleH);
//
//		int COUNT = image.getHeight()/MAX_HEIGHT_FRAME + 1;
//		int retW = (int)(image.getWidth()*scaleW);
//		int retH = (int)(image.getHeight()*scaleH);
//		
//		int[] tempBuff = new int[image.getWidth()*MAX_HEIGHT_FRAME];
//		int[] retBuff = new int[retW*(retH+1)];
//		int shift = 0;
//		
//		
//		for(int i = 0; i < COUNT; i++) {
//			int height = (i == COUNT -1)? image.getHeight() - i*MAX_HEIGHT_FRAME : MAX_HEIGHT_FRAME;  
//			tempBuff = new int[image.getWidth()*height];
//			image.getRGB(tempBuff, 0, image.getWidth(), 0, i*MAX_HEIGHT_FRAME, image.getWidth(), height);
//			tempBuff = resizeImage(tempBuff, image.getWidth(), height, scaleW, scaleH);
//			System.arraycopy(tempBuff, 0, retBuff, shift, tempBuff.length);
//			shift += tempBuff.length;
//			tempBuff = null;
//			MemoryDispatcher.gc();
//		}
//		
//		//image = Image.createRGBImage(retBuff, retW, retH, true);
////		retBuff = null;
//		MemoryDispatcher.gc();
//		return retBuff;	
//	}
	
	private static Image resizeBrokeImage(Image image, double scaleW, double scaleH) {
//		System.out.println("resize scale: "+scaleW + "x"+ scaleH);
		scaleW *= getCorrectionCoefficient(image.getWidth(), scaleW);
		scaleH *= getCorrectionCoefficient(MAX_HEIGHT_FRAME, scaleH);

		int COUNT = image.getHeight()/MAX_HEIGHT_FRAME + 1;
		int retW = (int)(image.getWidth()*scaleW);
		int retH = (int)(image.getHeight()*scaleH);
		
		int[] tempBuff = new int[image.getWidth()*MAX_HEIGHT_FRAME];
		int[] retBuff = new int[retW*(retH+1)];
		int shift = 0;
		
		
		for(int i = 0; i < COUNT; i++) {
			int height = (i == COUNT -1)? image.getHeight() - i*MAX_HEIGHT_FRAME : MAX_HEIGHT_FRAME;  
			tempBuff = new int[image.getWidth()*height];
			image.getRGB(tempBuff, 0, image.getWidth(), 0, i*MAX_HEIGHT_FRAME, image.getWidth(), height);
			tempBuff = resizeImage(tempBuff, image.getWidth(), height, scaleW, scaleH);
			System.arraycopy(tempBuff, 0, retBuff, shift, tempBuff.length);
			shift += tempBuff.length;
			tempBuff = null;
			MemoryDispatcher.gc();
		}
		
		image = Image.createRGBImage(retBuff, retW, retH, true);
		retBuff = null;
		MemoryDispatcher.gc();
		return image;	
	}
	
	private static double getCorrectionCoefficient(int value, double coef) {
		double t = value*coef;
		return t < 1? (1/coef)/value: t/(int)t; 
	} 
	
    public static Image scaleImage(Image image, double scale) {
    	return resizeImage(image, scale, scale);
    }

}
