namespace Fractals {

  public class Mandelbrot {
    public static int M = 500;

    public static int f(double x, double y) {
      double xn = 0, yn = 0, x2 = 0, y2 = 0;
      for (int n = 0; n < M; n++) {
        yn = 2*xn*yn + y;
        xn = x2 - y2 + x;
        x2 = xn*xn;
        y2 = yn*yn;
        if (x2 + y2 > 4) return n;
      }
      return M;
    }
  }

}
