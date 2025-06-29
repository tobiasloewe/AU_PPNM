CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

MIN_N = 100
MAX_N = 1000000
nlist = $(shell awk 'BEGIN { for (i=$(MIN_N); i<=$(MAX_N); i*=10) print i }')

all: Out.txt plot.unitCircle.png plot.gaussian2D.png

plot.gaussian2D.png : gaussian2D.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key bottom left ;\
		set title "Error in Monte-Carlo 2D Gaussian Calculation"; \
		set grid;\
		set xlabel "number of points" ;\
		set ylabel "Error" ;\
		set logscale xy; \
		plot  \
			"$<" index 0 using 1:3 with linespoints lw 2 title "estimated error", \
			"$<" index 0 using 1:4 with linespoints lw 2 title "actual error", \
			"$<" index 0 using 1:5 with linespoints lw 2 title "quasi estimated error", \
			"$<" index 0 using 1:6 with linespoints lw 2 title "quasi actual error" \
	' | gnuplot

plot.unitCircle.png : unitCircle.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set title "Error in Monte-Carlo Pi Calculation"; \
		set grid;\
		set xlabel "number of points" ;\
		set ylabel "Error" ;\
		set logscale xy; \
		set yrange [0.0000000001:1]; \
		plot  \
			"$<" index 0 using 1:3 with linespoints lw 2 title "plain estimated error", \
			"$<" index 0 using 1:4 with linespoints lw 2 title "plain actual error", \
			"$<" index 0 using 1:5 with linespoints lw 2 title "quasi estimated error", \
			"$<" index 0 using 1:6 with linespoints lw 2 title "quasi actual error" \
	' | gnuplot

Out.txt: mcerrortests.exe hardintegral.exe strata.exe
	mono $< -minN $(MIN_N) -maxN $(MAX_N) >$@;
	mono hardintegral.exe -N 10000 >>$@;
	mono strata.exe -N 10000 >>$@;

mcerrortests.exe: mcerrortests.cs matrix.dll montecarlo.dll; $(MKEXE)

hardintegral.exe: hardintegral.cs matrix.dll montecarlo.dll sfuns.dll; $(MKEXE)

strata.exe: strata.cs matrix.dll montecarlo.dll; $(MKEXE)

genlist.dll: ../libraries/genlist.cs; $(MKLIB)

montecarlo.dll: ../libraries/montecarlo.cs matrix.dll genlist.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

sfuns.dll : ../libraries/sfuns.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png