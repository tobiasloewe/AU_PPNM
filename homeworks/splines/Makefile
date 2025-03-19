CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
CSC = mcs
RUN = mono

all: out.txt plot.fit.svg plot.fiterror.svg; cat $<

out.txt: main.exe
	mono $< $@;

main.exe: main.cs matrix.dll jacobi.dll qr.dll fit.dll; $(MKEXE)

jacobi.dll: jacobi.cs matrix.dll; $(MKLIB)

fit.dll: fit.cs qr.dll matrix.dll; $(MKLIB)

qr.dll: qr.cs matrix.dll; $(MKLIB)

matrix.dll : ../matrix/vector.cs ../matrix/matrix.cs; $(MKLIB)

clean:
	$(RM) *.dll *.exe [Oo]ut*

cleanouts:
	$(RM) *.txt *.svg *.gpi *.svg
