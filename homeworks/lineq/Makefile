CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

#nthreadlist = $(shell seq 1 $(MAX_THREADS))
#@for nthreads in $(nthreadlist); do $(TIME) mono $< -terms $(N) -threads $$nthreads >>$@; done

all: out.txt; cat $<

out.txt: main.exe
	$(RUN) $< -dims 5,5 > $@

main.exe: main.cs matrix.dll qr.dll; $(MKEXE)

qr.dll: qr.cs matrix.dll; $(MKLIB)

matrix.dll : vector.cs matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut*