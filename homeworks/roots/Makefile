CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: plot.wavefunc.png plot.rmaxconvergence.png plot.rminconvergence.png plot.accconvergence.png



plot.accconvergence.png : Makefile Out.txt
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set title "acc convergence" ;\
			set grid; \
			set xlabel "rmin [Bohr]" ;\
			set ylabel "computed E [Hartree]" ;\
			plot  \
				"convergence.txt" index 2 using 1:2 with linespoints lw 2 title "num" \
		' | gnuplot

plot.rminconvergence.png : Makefile Out.txt
		echo ' \
			set terminal png; \
			set title "rmin convergence" ;\
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "rmin [Bohr]" ;\
			set ylabel "computed E [Hartree]" ;\
			plot  \
				"convergence.txt" index 1 using 1:2 with linespoints lw 2 title "num" \
		' | gnuplot

plot.rmaxconvergence.png : Makefile Out.txt
		echo ' \
			set terminal png; \
			set output "$@"; \
			set title "rmax convergence" ;\
			set key top ;\
			set grid; \
			set xlabel "rmax [Bohr]" ;\
			set ylabel "computed E [Hartree]" ;\
			set yrange [-1.0:2.5];\
			plot  \
				"convergence.txt" index 0 using 1:2 with lines lw 2 title "num" \
		' | gnuplot

plot.wavefunc.png : Makefile Out.txt
		echo ' \
			set terminal png; \
			set title "wavefunction" ;\
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "r [Bohr]" ;\
			set ylabel "wavefunc value" ;\
			plot  \
				"wavefunc.txt" index 0 using 1:2 with lines lw 2 title "numeric", \
				"wavefunc.txt" index 0using 1:3 with lines lw 2 title "exact" \
		' | gnuplot

Out.txt: main.exe shooting.exe
	$(RUN) $< >$@;
	$(RUN) shooting.exe >$@;

main.exe: main.cs matrix.dll roots.dll; $(MKEXE)

shooting.exe: shooting.cs matrix.dll roots.dll ode.dll genlist.dll; $(MKEXE)

debug.exe: debug.cs matrix.dll roots.dll ode.dll genlist.dll; $(MKEXE)

debug.txt: debug.exe
	$(RUN) $< >$@;

ode.dll: ../libraries/ode.cs matrix.dll genlist.dll; $(MKLIB)

roots.dll: ../libraries/roots.cs matrix.dll qr.dll; $(MKLIB)

qr.dll: ../libraries/qr.cs matrix.dll; $(MKLIB)

genlist.dll: ../libraries/genlist.cs ode.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png