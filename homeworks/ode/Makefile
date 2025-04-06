CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono


all: plot.harmonic.png plot.pendulum.png plot.orbit.png;

plot.orbit.png : out.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set grid; \
		set xlabel "x" ;\
		set ylabel "y" ;\
		plot  \
			"$<" index 2 using 1:2 with lines notitle\
	' | gnuplot

plot.harmonic.png : out.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set grid; \
		set xlabel "x" ;\
		set ylabel "y" ;\
		plot  \
			"$<" index 0 using 1:2 with lines lw 2 lc rgb "red" title "u1", \
			"$<" index 0 using 1:3 with lines lw 2 lc rgb "blue" title "u2"\
	' | gnuplot

plot.pendulum.png : out.txt Makefile
	echo ' \
		set terminal png; \
		set output "$@"; \
		set key top ;\
		set grid; \
		set xlabel "x" ;\
		set ylabel "y" ;\
		plot  \
			"$<" index 1 using 1:2 with lines lw 2 lc rgb "red" title "u1", \
			"$<" index 1 using 1:3 with lines lw 2 lc rgb "blue" title "u2"\
	' | gnuplot

out.txt: main.exe
	mono $< 2> orbits.txt >$@;

orbits.txt: out.txt; 

main.exe: main.cs matrix.dll ode.dll genlist.dll; $(MKEXE)

ode.dll : ode.cs genlist.dll matrix.dll; $(MKLIB)

genlist.dll : ../libraries/genlist.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.png