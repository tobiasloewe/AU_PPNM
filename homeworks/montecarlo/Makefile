CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

MIN_N = 100
MAX_N = 100000000
nlist = $(shell awk 'BEGIN { for (i=$(MIN_N); i<=$(MAX_N); i*=10) print i }')

all:plot.mcwitherror.png plot.errors.png mcAndError.txt

plot.mcwitherror.png : mcAndError.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set grid;\
		set xlabel "Number of points" ;\
		set ylabel "Calculated Value" ;\
		set logscale xy; \
		plot  \
			"$<" index 0 using 1:2:3 with yerrorbars title "mc", \
			3.1415 with lines lw 2 title "Pi" \
	' | tee log.wavefunc.gpi | gnuplot

plot.errors.png : mcAndError.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set grid;\
		set xlabel "number of points" ;\
		set ylabel "Error" ;\
		set logscale xy; \
		plot  \
			"$<" index 0 using 1:3 with linespoints lw 2 title "estimated error", \
			"$<" index 0 using 1:4 with linespoints lw 2 title "actual error" \
	' | gnuplot

mcAndError.txt: mcerrortest.exe Makefile
	@for thisN in $(nlist); do mono $< -N $$thisN >>$@; done


out.txt: mcerrortest.exe
	mono $< >$@;

mcerrortest.exe: mcerrortest.cs matrix.dll montecarlo.dll; $(MKEXE)

montecarlo.dll: ../libraries/montecarlo.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png