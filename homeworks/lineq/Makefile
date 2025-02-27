CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono
TIME = time --format "$$nsize %e %U" --append --output $@
MAX_SIZE = 1000

nsizelist = $(shell seq 10 100 $(MAX_SIZE))
#$(RUN) $< -dims 5,5 > $@

all: out.txt plot.comp.svg Makefile; cat $<

plot.comp.svg : out.txt Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "N" ;\
		set ylabel "O(N)" ;\
		plot  \
			"$<" index 0 with lines title "GS QR and Backsub" \
	' | tee log.comp.gpi | gnuplot

out.txt: main.exe
	@for nsize in $(nsizelist); do $(TIME) mono $< -dims $$nsize,$$nsize -verbose 0 >>$@; done

main.exe: main.cs matrix.dll qr.dll; $(MKEXE)

qr.dll: qr.cs matrix.dll; $(MKLIB)

matrix.dll : vector.cs matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut*