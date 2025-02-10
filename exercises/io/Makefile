CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 

Out.txt: main.exe
	echo "1   2\t3\n4" | mono main.exe -numbers:1,2,3,4,5 1 -input:input.txt -output:output.txt> $@ -weirdNumbers 2>> $@

main.exe: main.cs; $(MKEXE)

clean:
	rm -f *.exe *.dll Out.txt