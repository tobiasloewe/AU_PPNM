CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: plot.fit.svg;

plot.fit.svg : data.txt out.txt Makefile
		echo ' \
			set terminal svg; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "x" ;\
			set ylabel "y" ;\
			plot  \
				"out.txt" index 0 using 1:2 pt 7 ps 1 title "linear spline", \
				"out.txt" index 0 using 1:3 pt 5 ps 1 title "quad spline", \
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