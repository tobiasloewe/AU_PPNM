CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: plot.wavefunc.png plot.rmaxconvergence.png

plot.rmaxconvergence.png : Makefile out.txt
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "rmax [Bohr]" ;\
			set ylabel "computed E [Hartree]" ;\
			set yrange [-1.0:2.5];\
			plot  \
				"convergence.txt" index 0 using 1:2 with lines lw 2 title "num" \
		' | gnuplot

plot.wavefunc.png : Makefile out.txt
		echo ' \
			set terminal png; \
			set output "$@"; \
			set key top ;\
			set grid; \
			set xlabel "r [Bohr]" ;\
			set ylabel "wavefunc value" ;\
			plot  \
				"wavefunc.txt" index 0 using 1:2 with lines lw 2 title "numeric", \
				"wavefunc.txt" index 0using 1:3 with lines lw 2 title "exact" \
		' | gnuplot

out.txt: main.exe shooting.exe
	$(RUN) $< >$@;
	$(RUN) shooting.exe >$@;

main.exe: main.cs matrix.dll roots.dll; $(MKEXE)

shooting.exe: shooting.cs matrix.dll roots.dll ode.dll genlist.dll; $(MKEXE)

ode.dll: ../libraries/ode.cs matrix.dll genlist.dll; $(MKLIB)

roots.dll: ../libraries/roots.cs matrix.dll qr.dll; $(MKLIB)

qr.dll: ../libraries/qr.cs matrix.dll; $(MKLIB)

genlist.dll: ../libraries/genlist.cs ode.dll; $(MKLIB)

matrix.dll : ../libraries/vector.cs ../libraries/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.svg *.txt *.png