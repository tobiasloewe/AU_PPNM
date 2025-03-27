CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: plot.linear.png plot.quadratic.png plot.cubic.png;

plot.linear.png : data.txt out.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "x" ;\
			set ylabel "y" ;\
			set yrange [-1.5:1.5]; \
			plot  \
				"out.txt" index 0 using 1:2 pt 4 ps 1 title "lin spline", \
				"out.txt" index 0 using 1:5 pt 4 ps 1 title "integral", \
				"$<" index 0 pt 2 ps 2.5 lc rgb "red" title "Cos(x) data" \
		' | gnuplot

plot.quadratic.png : data.txt out.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "x" ;\
			set ylabel "y" ;\
			set yrange [-1.5:1.5]; \
			plot  \
				"out.txt" index 0 using 1:3 pt 4 ps 1 title "quadratic splines", \
				"out.txt" index 0 using 1:6 pt 4 ps 1 title "integral", \
				"$<" index 0 pt 2 ps 2.5 lc rgb "red" title "Cos(x) data" \
		' | gnuplot

plot.cubic.png : data.txt out.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "x" ;\
			set ylabel "y" ;\
			set yrange [-1.5:1.5]; \
			plot  \
				"out.txt" index 0 using 1:4 pt 4 ps 1 title "cubic splines", \
				"out.txt" index 0 using 1:7 pt 4 ps 1 title "integral", \
				"$<" index 0 pt 2 ps 2.5 lc rgb "red" title "Cos(x) data" \
		' | gnuplot

out.txt: main.exe
	mono $< -data data.txt >$@;

main.exe: main.cs fit.dll matrix.dll; $(MKEXE)

fit.dll: fit.cs qr.dll matrix.dll; $(MKLIB)

qr.dll: ../libraries/qr.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg