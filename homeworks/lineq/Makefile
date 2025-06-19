CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono
TIME = time --format "$$nsize %e %U" --append --output $@
MAX_SIZE = 800

nsizelist = $(shell seq 100 100 $(MAX_SIZE))

all: Out.txt plot.time.svg Makefile; cat $<

plot.time.svg : timingdata.txt Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "N" ;\
		set ylabel "O(N)" ;\
		set title "QR Decomposition computing time" ;\
		plot  \
			"$<" index 0 with lines title "GS QR and Backsub" \
	' | tee log.time.gpi | gnuplot

timingdata.txt: runqr.exe
	@for nsize in $(nsizelist); do echo "($$nsize)"; $(TIME) mono $< -dims $$nsize,$$nsize >>$@; done

Out.txt: testqr.exe testlinsolve.exe
	mono $< -dims 20,5 > $@;
	mono testlinsolve.exe -dims 7,7 >> $@;

testqr.exe: testqr.cs matrix.dll qr.dll; $(MKEXE)

runqr.exe: runqr.cs matrix.dll qr.dll; $(MKEXE)

testlinsolve.exe: testlinsolve.cs matrix.dll qr.dll; $(MKEXE)

qr.dll: qr.cs matrix.dll; $(MKLIB)

matrix.dll : vector.cs matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut* *.txt *.svg *.gpi