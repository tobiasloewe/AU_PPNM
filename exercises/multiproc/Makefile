CODE = $(filter %.cs,$^)
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
MKLIB = mcs -out:$@ -target:library $(DLLS) $(CODE) 
MKEXE = mcs -out:$@ -target:exe $(DLLS) $(CODE) 
#TIME = time --portability --append --output $@
TIME = time --format "$$nthreads %e %U" --append --output $@
nthreadlist = 1 2 3 4 5 6 7 8
N=5e8

Out.txt : main.exe Makefile
	>$@
	
	@for nthreads in $(nthreadlist); do $(TIME) mono $< -terms $(N) -threads $$nt >>$@; done
	
	printf "\n this was manual threading\n" >>$@
	$(TIME) mono $< -terms $N -threads 1 -mode 1 2>> log1.txt >>$@
	printf "this was mode 1\n" >>$@
	$(TIME) mono $< -terms $N -threads 1 -mode 2 2>> log2.txt >>$@
	printf "this was mode 2\n" >>$@
main.exe: main.cs; $(MKEXE)

clean:
	rm -f *.exe *.dll Out.txt
