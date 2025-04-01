CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

plot.ode1.png : out.txt Makefile
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

all: out.txt;

out.txt: main.exe
	mono $< >$@; 

main.exe: main.cs matrix.dll ode.dll genlist.dll; $(MKEXE)

ode.dll : ode.cs genlist.dll matrix.dll; $(MKLIB)

genlist.dll : ../libraries/genlist.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg