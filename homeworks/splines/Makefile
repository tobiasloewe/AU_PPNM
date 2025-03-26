CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: plot.splines.png plot.ints.png;

plot.ints.png : data.txt out.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "x" ;\
			set ylabel "y" ;\
			plot  \
				"out.txt" index 0 using 1:4 pt 3 ps 1 title "cubic splines", \
				"out.txt" index 0 using 1:5 pt 3 ps 1 title "lin spline int", \
				"out.txt" index 0 using 1:6 pt 3 ps 1 title "quad spline int", \
				"out.txt" index 0 using 1:7 pt 3 ps 1 title "cubic spline int", \
				"$<" index 0 pt 1 ps 3 title "Cos(x) data" \
		' | gnuplot

plot.splines.png : data.txt out.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "x" ;\
			set ylabel "y" ;\
			set yrange [-1.5:1.5]; \
			plot  \
				"out.txt" index 0 using 1:2 pt 7 ps 1 title "linear splines", \
				"out.txt" index 0 using 1:3 pt 5 ps 1 title "quadratic splines", \
				"out.txt" index 0 using 1:4 pt 4 ps 1 title "cubic splines", \
				"$<" index 0 pt 1 ps 3 title "Cos(x) data" \
		' | gnuplot

out.txt: main.exe
	mono $< -data data.txt >$@;

main.exe: main.cs fit.dll matrix.dll; $(MKEXE)

fit.dll: fit.cs qr.dll matrix.dll; $(MKLIB)

qr.dll: ../libraries/qr.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg