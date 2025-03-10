CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono
#TIME = time --format "$$nsize %e %U" --append --output $@

all: out.txt; cat $<

out.txt: runtests.exe
	mono $< -dim 6,3 > $@;

runtests.exe: runtests.cs matrix.dll jacobi.dll qr.dll; $(MKEXE)

jacobi.dll: jacobi.cs matrix.dll; $(MKLIB)

qr.dll: qr.cs matrix.dll; $(MKLIB)

matrix.dll : ../matrix/vector.cs ../matrix/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut*

cleanouts:
	$(RM) *.txt *.svg *.gpi
