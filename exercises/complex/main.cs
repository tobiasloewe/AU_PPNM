using static System.Console;
using static System.Math;
static class main{

static int Main(){
    complex z1 = cmath.sqrt(-complex.One);
    complex expected1 = complex.I;

    WriteLine($"√-1 approx i: {expected1.approx(z1)}");
    WriteLine($"Computed: {z1}, Expected: {expected1}");

    complex ztest = cmath.arg(cmath.arg(-complex.One));
    complex ztest2 = cmath.exp(complex.I*ztest/2);
    WriteLine($"arg of -1 is {ztest}, exp is {ztest2}");
    WriteLine($"COMMENT: somehow by default sqrt uses other solution");

    complex z2 = cmath.log(complex.I);
    complex expected2 = new complex(0, PI / 2);
    WriteLine($"ln(i) approx iπ/2: {expected2.approx(z2)}");
    WriteLine($"Computed: {z2}, Expected: {expected2}");

    complex z3 = cmath.sqrt(complex.I);
    complex expected3 = new complex(1 / Sqrt(2), 1 / Sqrt(2));
    WriteLine($"√i approx (1/√2 + i/√2): {expected3.approx(z3)}");
    WriteLine($"Computed: {z3}, Expected: {expected3}");

    complex z4 = cmath.exp(complex.I);
    complex expected4 = new complex(Cos(1), Sin(1));
    WriteLine($"e^i approx cos(1) + i sin(1): {expected4.approx(z4)}");
    WriteLine($"Computed: {z4}, Expected: {expected4}");

    complex z5 = cmath.exp(complex.I * PI);
    complex expected5 = -complex.One;
    WriteLine($"e^(iπ) approx -1: {expected5.approx(z5)}");
    WriteLine($"Computed: {z5}, Expected: {expected5}");

    complex z6 = cmath.pow(complex.I, complex.I);
    complex expected6 = new complex(Exp(-PI / 2), 0);
    WriteLine($"i^i approx e^(-π/2): {expected6.approx(z6)}");
    WriteLine($"Computed: {z6}, Expected: {expected6}");

    complex z7 = cmath.sin(complex.I * PI);
    complex expected7 = new complex(0, Sinh(PI));
    WriteLine($"sin(iπ) approx i sinh(π): {expected7.approx(z7)}");
    WriteLine($"Computed: {z7}, Expected: {expected7}");

    complex sinh_i = cmath.sinh(complex.I);
    complex cosh_i = cmath.cosh(complex.I);
    complex expectedSinhI = new complex(0, Sin(1));
    complex expectedCoshI = new complex(Cos(1), 0);

    WriteLine($"sinh(i) approx i sin(1): {expectedSinhI.approx(sinh_i)}");
    WriteLine($"Computed: {sinh_i}, Expected: {expectedSinhI}");

    WriteLine($"cosh(i) approx cos(1): {expectedCoshI.approx(cosh_i)}");
    WriteLine($"Computed: {cosh_i}, Expected: {expectedCoshI}");

    return 0;
}
}
