CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono


all: out.txt plot.erf1.png;

plot.erf1.png : erf1.txt Makefile
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "acc" ;\
			set ylabel "absolute error" ;\
			set logscale; \
			set format x "1e%T"; \
			set yrange [1e-17:*]; \
			set xrange [*:0.1] reverse; \
			plot  \
				"$<" index 0 using 1:2 with linespoints lw 2 title "Data", \
				2.22*1e-16 with lines lw 2 title "Machine Epsilon" \
		' | gnuplot

out.txt: main.exe
	mono $< >$@;

main.exe: main.cs matrix.dll quadratures.dll genlist.dll; $(MKEXE)

quadratures.dll : ../libraries/quadratures.cs; $(MKLIB)

genlist.dll : ../libraries/genlist.cs matrix.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png