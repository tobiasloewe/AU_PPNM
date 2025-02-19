CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
TIME = time --portability --append --output $@

N=5e19

Out.txt : main.exe Makefile
	>$@
	$(TIME) mono $< -terms:$N -threads:1 >>$@
	$(TIME) mono $< -terms:$N -threads:2 >>$@
	$(TIME) mono $< -terms:$N -threads:3 >>$@
	$(TIME) mono $< -terms:$N -threads:4 >>$@
	printf "this was mode 0\n" >>$@
	$(TIME) mono $< -terms:$N -threads:1 -mode:1 2> log1.txt >>$@
	printf "this was mode 1\n" >>$@
	$(TIME) mono $< -terms:$N -threads:1 -mode:2 2> log2.txt >>$@
	printf "this was mode 2\n" >>$@
main.exe: main.cs; $(MKEXE)

clean:
	rm -f *.exe *.dll Out.txt