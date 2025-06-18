CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 

Out.txt: main.exe
	echo "8   4\t6\n1" | mono main.exe -numbers:1,2,3,4,5 -input:input.txt -output:output.txt -weirdNumbers 1> $@ 2>> $@

main.exe: main.cs; $(MKEXE)

clean:
	rm -f *.exe *.dll Out.txt