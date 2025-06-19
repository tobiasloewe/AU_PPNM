CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono


all: plot.harmonic.png plot.pendulum.png plot.orbits.png Out.txt;

plot.orbits.png : orbit1.txt orbit2.txt orbit3.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set title "Planetary Orbits"; \
		set xlabel "x"; \
		set ylabel "y"; \
		set grid; \
		set yrange [-2.5:2.5]; \
		set xrange [-2.5:2.5]; \
		plot  \
			"orbit1.txt" index 0 using 1:2 with lines lw 2 lc rgb "blue" title "circular", \
			"orbit2.txt" index 0 using 1:2 with lines lw 2 lc rgb "red" title "elliptic", \
			"orbit3.txt" index 0 using 1:2 with lines lw 2 lc rgb "orange" title "relativistic", \
	' | gnuplot


plot.harmonic.png : harmonic.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set grid; \
		set title "Harmonic Oscillator"; \
		set xlabel "x" ;\
		set ylabel "y" ;\
		plot  \
			"$<" index 0 using 1:2 with lines lw 2 lc rgb "red" title "u1", \
			"$<" index 0 using 1:3 with lines lw 2 lc rgb "blue" title "u2"\
	' | gnuplot

plot.pendulum.png : pendulum.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set grid; \
		set title "Damped Pendulum"; \
		set xlabel "x" ;\
		set ylabel "y" ;\
		plot  \
			"$<" index 0 using 1:2 with lines lw 2 lc rgb "red" title "u1", \
			"$<" index 0 using 1:3 with lines lw 2 lc rgb "blue" title "u2"\
	' | gnuplot

Out.txt: main.exe
	mono $< >$@;
	@echo 'Calculation done' >>$@;

pendulum.txt: Out.txt;

harmonic.txt: Out.txt; 

orbit1.txt: Out.txt; 

orbit2.txt: Out.txt;

orbit3.txt: Out.txt;

main.exe: main.cs matrix.dll ode.dll genlist.dll; $(MKEXE)

ode.dll : ode.cs genlist.dll matrix.dll; $(MKLIB)

genlist.dll : ../libraries/genlist.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt