CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono
#TIME = time --format "$$nsize %e %U" --append --output $@
MAX_R = 20
MAX_DR = 1

rmaxlist = $(shell seq 3 0.25 $(MAX_R))
drlist = $(shell seq 0.01 0.02 $(MAX_DR))

all: out.txt plot.conv1.svg plot.conv2.svg plot.wavefunc.svg; cat $<

out.txt: runtests.exe
	mono $< -dim 10 >> $@;

wavefunc.txt: hatom.exe
	mono $< -rmax 10 -dr 0.05 -returnfunc 0 >>$@;

convdata.txt: hatom.exe Makefile
#	rm convdata.txt
	@for rmaxv in $(rmaxlist); do mono $< -rmax $$rmaxv -dr 0.05 >>$@; done
	@echo "" >>$@; 
	@echo "" >>$@;
	@for drv in $(drlist); do mono $< -rmax 10 -dr $$drv >>$@; done

plot.wavefunc.svg : wavefunc.txt Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key top ;\
		set grid;\
		set xlabel "x" ;\
		set ylabel "Psi" ;\
		plot  \
			"$<" index 0 with lines title "H atom S orbital" \
	' | tee log.wavefunc.gpi | gnuplot

plot.conv1.svg : convdata.txt Makefile
	echo ' \
		set terminal svg; \
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
	$(RM) *.dll *.exe [Oo]ut*

cleanouts:
	$(RM) *.txt *.svg *.gpi
