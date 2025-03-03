CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono
#TIME = time --format "$$nsize %e %U" --append --output $@
#MAX_SIZE = 2000

#nsizelist = $(shell seq 100 100 $(MAX_SIZE))

all: out.txt Makefile; cat $<

out.txt: main.exe
	mono $< -dim 10 -mode test >$@;
	mono $< -mode Hswave -rmax 30.0 -dr 1.0 >>$@;

main.exe: main.cs matrix.dll jacobi.dll; $(MKEXE)	

jacobi.dll: jacobi.cs matrix.dll; $(MKLIB)

matrix.dll : vector.cs matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut*