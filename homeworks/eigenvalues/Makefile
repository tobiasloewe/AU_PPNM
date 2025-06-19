CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono
#TIME = time --format "$$nsize %e %U" --append --output $@
MAX_R = 10
MAX_DR = 0.5

rmaxlist = $(shell seq 3 0.25 $(MAX_R))
drlist = $(shell seq 0.03 0.05 $(MAX_DR))

all: Out.txt plot.conv1.svg plot.conv2.svg plot.wavefunc1.svg plot.wavefunc2.svg plot.wavefunc3.svg; cat $<

Out.txt: runtests.exe
	mono $< -dim 10 >> $@;

wavefunc.txt: hatom.exe
	mono $< -rmax 10 -dr 0.05 -returnfunc 0 >$@;
	@echo " " >>$@;
	@echo " " >>$@;
	mono $< -rmax 10 -dr 0.05 -returnfunc 1 >>$@;
	@echo " " >>$@;
	@echo " " >>$@;
	mono $< -rmax 10 -dr 0.05 -returnfunc 2 >>$@;

convdata.txt: hatom.exe Makefile
#	rm convdata.txt
	@for rmaxv in $(rmaxlist); do mono $< -rmax $$rmaxv -dr 0.05 >>$@; done
	@echo "" >>$@; 
	@echo "" >>$@;
	@for drv in $(drlist); do mono $< -rmax 10 -dr $$drv >>$@; done

plot.wavefunc1.svg : wavefunc.txt Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key top ;\
		set grid;\
		set title "Hydrogen atom eigenfunction 1"; \
		set xlabel "r [Bohr]" ;\
		set ylabel "Psi" ;\
		plot  \
			"$<" index 0 using 1:2 with lines title "R_{1}(r)", \
			"$<" index 0 using 1:3 with lines title "\|R_{1}(r)|\|^2" \
	' | gnuplot

plot.wavefunc2.svg : wavefunc.txt Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key top ;\
		set grid;\
		set title "Hydrogen atom eigenfunction 2"; \
		set xlabel "r [Bohr]" ;\
		set ylabel "Psi" ;\
		plot  \
			"$<" index 1 using 1:2 with lines title "R_{2}(r)", \
			"$<" index 1 using 1:3 with lines title "\|R_{2}(r)|\|^2" \
	' | gnuplot

plot.wavefunc3.svg : wavefunc.txt Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key top ;\
		set grid;\
		set title "Hydrogen atom eigenfunction 3"; \
		set xlabel "r [Bohr]" ;\
		set ylabel "Psi" ;\
		plot  \
			"$<" index 2 using 1:2 with lines title "R_{3}(r)", \
			"$<" index 2 using 1:3 with lines title "\|R_{3}(r)|\|^2" \
	' | gnuplot

plot.conv1.svg : convdata.txt Makefile
	echo ' \
		set terminal svg; \
		set title "Convergence of rmax"; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "rmax" ;\
		set ylabel "lowest energy" ;\
		plot  \
			"$<" index 0 using 1:3 with lines title "H atom EVD with cyclic Jacobi" \
	' | tee log.conv1.gpi | gnuplot

plot.conv2.svg : convdata.txt Makefile
	echo ' \
		set terminal svg; \
		set title "Convergence of dr"; \
		set output "$@"; \
		set key top ;\
		set xlabel "dr" ;\
		set ylabel "lowest energy" ;\
		set xrange [*:*] reverse ;\
		plot  \
			"$<" index 1 using 2:3 with lines title "H atom EVD with cyclic Jacobi" \
	' | tee log.conv2.gpi | gnuplot

hatom.exe: hatom.cs matrix.dll jacobi.dll; $(MKEXE)	

runtests.exe: runtests.cs matrix.dll jacobi.dll; $(MKEXE)	

jacobi.dll: jacobi.cs matrix.dll; $(MKLIB)

matrix.dll : ../matrix/vector.cs ../matrix/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.txt *.svg *.gpi *.log

cleanouts:
	$(RM) *.txt *.svg *.gpi
